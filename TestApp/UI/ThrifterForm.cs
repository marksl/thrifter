using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TestApp.UI
{
    public partial class ThrifterForm : Form
    {
        private AssemblyLoader _assemblyLoader;
        private ClassAndMethod _classAndMethod;

        public ThrifterForm()
        {
            InitializeComponent();

            thriftText.KeyUp += ThriftText_KeyUp;

            var items = new List<ThrifterComboBoxItem>
                            {
                                new ThrifterComboBoxItem("http://localhost:53959/HttpTest/json.thrift", "0"),
                                new ThrifterComboBoxItem("http://localhost:53959/HttpTest/binary.thrift", "1"),
                                new ThrifterComboBoxItem("http://localhost:53959/HttpTest/compact.thrift", "2"),
                                new ThrifterComboBoxItem("localhost:8080", "4")
                            };
            urlComboBox.DataSource = items;
            urlComboBox.DisplayMember = "Name";
            urlComboBox.ValueMember = "Id";

            var protocolItems = new List<ThrifterComboBoxItem>
                            {
                                new ThrifterComboBoxItem("TJSONProtocol", "0"),
                                new ThrifterComboBoxItem("TBinaryProtocol", "1"),
                                new ThrifterComboBoxItem("TCompactProtocol", "2"),
                            };
            protocolComboBox.DataSource = protocolItems;
            protocolComboBox.DisplayMember = "Name";
            protocolComboBox.ValueMember = "Id";

            methodComboBox.DisplayMember = "Name";
            methodComboBox.ValueMember = "Id";
            requestTreeView.BeforeLabelEdit += RequestTreeView_BeforeLabelEdit;
            requestTreeView.AfterLabelEdit += RequestTreeView_AfterLabelEdit;
        }

        private void ThriftText_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers == Keys.Control)
                && e.KeyCode == Keys.A)
            {
                thriftText.SelectAll();
            }
        }

        private static void RequestTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var node = e.Node as ThrifterTreeNode;
            if (node == null)
                throw new InvalidProgramException("All nodes should be MyTreeNodes.");

            node.UpdateThriftObj(e.Label);
        }

        static void RequestTreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                e.CancelEdit = true;
            }
        }

        private void Process_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            errorProvider1.Clear();

            requestTreeView.Nodes.Clear();
            responseTreeView.Nodes.Clear();

            _assemblyLoader = null;

            var generator = new CSharpGenerator(thriftText.Text);
            var buildResult = generator.Build();
            if (!buildResult.Success)
            {
                errorProvider1.SetError(thriftText, buildResult.FailureMessage);

                return;
            }

            _assemblyLoader = new AssemblyLoader(buildResult.AssemblyName);
            _assemblyLoader.Load();

            methodComboBox.DataSource = _assemblyLoader.MethodNames.Select(x => new ThrifterComboBoxItem(x, x)).ToList();

            Cursor.Current = Cursors.Default;
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (_classAndMethod != null)
            {
                var url = ((ThrifterComboBoxItem) urlComboBox.SelectedItem).Name;
                var protocolName = ((ThrifterComboBoxItem)protocolComboBox.SelectedItem).Name;

                Cursor.Current = Cursors.WaitCursor;

                responseTreeView.Nodes.Clear();
                responseTreeView.BeginUpdate();

                var parameters = GetParameters();
                var method = new ThriftMethod(_classAndMethod, parameters);
                var response = method.Invoke(url, protocolName);
                if (response != null)
                {
                    TreeNode[] nodes = ConvertToTreeNodes(new List<ThriftObject> { response});
                    responseTreeView.Nodes.AddRange(nodes);
                }

                responseTreeView.EndUpdate();

                Cursor.Current = Cursors.Default;
            }
        }

        private void MethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _classAndMethod = _assemblyLoader.GetClient((string) methodComboBox.SelectedValue);

            Cursor.Current = Cursors.WaitCursor;

            List<ThriftObject> objs = ThriftObjectFactory.CreateThriftObjects(_classAndMethod.Method.GetParameters());
            requestTreeView.Nodes.Clear();
            requestTreeView.BeginUpdate();

            TreeNode[] nodes = ConvertToTreeNodes(objs);
            requestTreeView.Nodes.AddRange(nodes);

            requestTreeView.EndUpdate();
            
            Cursor.Current = Cursors.Default;
        }

        private static TreeNode[] ConvertToTreeNodes(IEnumerable<ThriftObject> objs)
        {
            var nodes = new List<ThrifterTreeNode>();
            foreach (var thriftObject in objs)
            {
                var node = new ThrifterTreeNode(thriftObject);
                var childNodes = ConvertToTreeNodes(thriftObject.ChildObjs).ToArray();
                node.Nodes.AddRange(childNodes);
                nodes.Add(node);
            }

            return nodes.Cast<TreeNode>().ToArray();
        }

        object[] GetParameters()
        {
            var parameters = new List<object>();

// ReSharper disable LoopCanBeConvertedToQuery
            foreach (ThrifterTreeNode node in requestTreeView.Nodes)
// ReSharper restore LoopCanBeConvertedToQuery
            {
                parameters.Add(node.ThriftObject.GetObject());
            }

            return parameters.ToArray();
        }
    }
}