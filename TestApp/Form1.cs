using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Thrift.Protocol;
using Thrift.Transport;

namespace TestApp
{
    public partial class Form1 : Form
    {
        private AssemblyLoader a;
        private Client client;

        public Form1()
        {
            InitializeComponent();

            var items = new List<Item>
                            {
                                new Item("http://localhost:53959/HttpTest/json.thrift", "0"),
                                new Item("http://localhost:53959/HttpTest/binary.thrift", "1"),
                                new Item("http://localhost:53959/HttpTest/compact.thrift", "2"),
                                new Item("localhost:8080", "4")
                            };
            urlComboBox.DataSource = items;
            urlComboBox.DisplayMember = "Name";
            urlComboBox.ValueMember = "Id";

            var protocolItems = new List<Item>
                            {
                                new Item("TJSONProtocol", "0"),
                                new Item("TBinaryProtocol", "1"),
                                new Item("TCompactProtocol", "2"),
                            };
            protocolComboBox.DataSource = protocolItems;
            protocolComboBox.DisplayMember = "Name";
            protocolComboBox.ValueMember = "Id";

            methodComboBox.DisplayMember = "Name";
            methodComboBox.ValueMember = "Id";
            requestTreeView.BeforeLabelEdit += requestTreeView_BeforeLabelEdit;
            requestTreeView.AfterLabelEdit += requestTreeView_AfterLabelEdit;
        }

        private void requestTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var node = e.Node as MyTreeNode;
            if (node == null)
                throw new InvalidProgramException("All nodes should be MyTreeNodes.");

            node.UpdateThriftObj(e.Label);
        }

        void requestTreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                e.CancelEdit = true;
            }
        }

        private void Process_Click(object sender, EventArgs e)
        {
            var g = new CSharpGenerator(thriftText.Text);
            string assembly = g.Generate();
            //string assembly = ".\\compiled\\TempThriftGen.dll";

            a = new AssemblyLoader(assembly);
            a.Load();


            methodComboBox.DataSource = a.MethodNames.Select(x => new Item(x, x)).ToList();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                var url = ((Item) urlComboBox.SelectedItem).Name;

                TTransport transport = GetTransport(url);
                TProtocol protocol = GetProtocol(transport);

                object instance = Activator.CreateInstance(client.ClientType, protocol);
                object[] parameters = GetParameters();


                //Cursor.Current = Cursors.WaitCursor;

                //List<ThriftObject> objs = new List<ThriftObject>();

                //responseTreeView.Nodes.Clear();
                //responseTreeView.BeginUpdate();

                //try
                //{
                //    object response = client.Method.Invoke(instance, parameters);
                //    if (response != null)
                //    {
                //        // AH OKAY... so I need to create thrift objects and populate the properties 
                //        // based on what is the current value of the thrift objects... hmmmm.. shouldn't be too hard
                //        var thriftObjects = CreateThriftObjects(response.GetType(), "Response", null);
                //        objs.Add(thriftObjects);
                //    }
                //}
                //catch (TargetInvocationException target)
                //{
                //    // AH OKAY... so I need to create thrift objects and populate the properties 
                //    // based on what is the current value of the thrift objects... hmmmm.. shouldn't be too hard
                //    var thriftObjects = CreateThriftObjects(target.InnerException.GetType(), "Response", null);
                //    objs.Add(thriftObjects);
                //}

                //MyTreeNode[] nodes = ConvertToTreeNodes(objs);
                //responseTreeView.Nodes.AddRange(nodes);

                //responseTreeView.EndUpdate();

                //Cursor.Current = Cursors.Default;
            }
        }

        private TProtocol GetProtocol(TTransport transport)
        {
            TProtocol protocol;
            switch (((Item) protocolComboBox.SelectedItem).Name)
            {
                case "TJSONProtocol":
                    protocol = new TJSONProtocol(transport);
                    break;

                case "TBinaryProtocol":
                    protocol = new TBinaryProtocol(transport);
                    break;

                case "TCompactProtocol":
                    protocol = new TCompactProtocol(transport);
                    break;

                default:
                    throw new InvalidOperationException();
            }
            return protocol;
        }

        private static TTransport GetTransport(string url)
        {
            TTransport transport;
            if (url.StartsWith("http"))
            {
                var uri = new Uri(url);
                var httpClient = new THttpClient(uri);
                transport = httpClient;
            }
            else
            {
                var parts = url.Split(new[] {':'});

                transport = new TSocket(parts[0], Convert.ToInt32(parts[1]));
            }
            return transport;
        }

        private void methodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            client = a.GetClient((string) methodComboBox.SelectedValue);

            Cursor.Current = Cursors.WaitCursor;

            List<ThriftObject> objs = CreateThriftObjects(client.Method.GetParameters());
            requestTreeView.Nodes.Clear();
            requestTreeView.BeginUpdate();

            MyTreeNode[] nodes = ConvertToTreeNodes(objs);
            requestTreeView.Nodes.AddRange(nodes);

            requestTreeView.EndUpdate();
            
            Cursor.Current = Cursors.Default;
        }

        private MyTreeNode[] ConvertToTreeNodes(List<ThriftObject> objs)
        {
            var nodes = new List<MyTreeNode>();
            foreach (var thriftObject in objs)
            {
                var node = new MyTreeNode(thriftObject);
                var childNodes = ConvertToTreeNodes(thriftObject.ChildObjs).Cast<TreeNode>().ToArray();
                node.Nodes.AddRange(childNodes);
                nodes.Add(node);
            }

            return nodes.ToArray();
        }


        private void CreateThriftObjects(IEnumerable<PropertyInfo> getParameters, ThriftObject parentObj)
        {
            foreach (var parameter in getParameters)
            {
                Type type = parameter.PropertyType;
                CreateThriftObjects(type, parameter.Name, parentObj);
            }
        }

        private List<ThriftObject> CreateThriftObjects(IEnumerable<ParameterInfo> getParameters)
        {
            return getParameters
                .Select(parameter => parameter.ParameterType)
                .Select(x => CreateThriftObjects(x, x.Name, null))
                .ToList();
        }

        private ThriftObject CreateThriftObjects(Type type, string propertyName, ThriftObject parentObj)
        {
            object instance = null;
            if (type != typeof (string))
            {
                instance = Activator.CreateInstance(type);
            }


            var t = new ThriftObject
                        {
                            Obj = instance,
                            ParentObj = parentObj,
                            PropertyName = propertyName,
                            ObjType = type
                        };
            if (parentObj != null)
            {
                parentObj.ChildObjs.Add(t);
            }

            if (type.IsValueType || type == typeof (string))
            {
                t.IsNull = false;
            }
            else
            {
                var props = type.GetProperties();

                CreateThriftObjects(props, t);
            }

            return t;
        }


        object[] GetParameters()
        {
            var parameters = new List<object>();

            foreach (MyTreeNode node in requestTreeView.Nodes)
            {
                parameters.Add(node.ThriftObject.GetObject());
            }

            return parameters.ToArray();
        }
    }

    public class Item
    {
        public Item(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class ThriftObject
    {
        public ThriftObject()
        {
            IsNull = true;
            ChildObjs = new List<ThriftObject>();
        }
        public string PropertyName { get; set; }

        public ThriftObject ParentObj { get; set; }
        public object Obj { get; set; }
        public Type ObjType { get; set; }
        public bool IsNull { get; set; }
        

        public List<ThriftObject> ChildObjs { get; set; } 

        public void SetProperty(string value)
         {
            try
            {
                //if (ParentObj == null)
                //{
                if (ObjType == typeof (int))
                    Obj = Convert.ToInt32(value);
                else if (ObjType == typeof (short))
                    Obj = Convert.ToInt16(value);
                else if (ObjType == typeof (long))
                    Obj = Convert.ToInt64(value);
                else if (ObjType == typeof (string))
                    Obj = value;
                else if (ObjType.IsEnum)
                    Obj = Enum.Parse(Obj.GetType(), value);
                else
                    throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                
            }

            LinkToParent();
        }

        private void LinkToParent()
        {
            IsNull = false;
            if (ParentObj != null)
            {
                ParentObj.LinkToParent();
            }
        }

        public object GetObject()
        {
            if (IsNull)
                return null;

            foreach (ThriftObject child in ChildObjs)
            {
                // Just load all it's properties
                child.GetObject();

                if (!child.IsNull)
                {
                    var propInfo = ObjType.GetProperty(child.PropertyName);
                    propInfo.SetValue(Obj, child.Obj, null);
                }
            }

            return Obj;
        }
    }

    public class MyTreeNode : TreeNode
    {
        public MyTreeNode(ThriftObject thriftObject)
        {
            ToolTipText = thriftObject.PropertyName;
            Text = thriftObject.PropertyName;

            _thriftObject = thriftObject;
        }

        private readonly ThriftObject _thriftObject;

        public ThriftObject ThriftObject
        {
            get { return _thriftObject; }
        }

        public void UpdateThriftObj(string newValue)
        {
            _thriftObject.SetProperty(newValue);
        }
    }

}