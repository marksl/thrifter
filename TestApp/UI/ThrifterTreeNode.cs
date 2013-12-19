using System.Windows.Forms;

namespace TestApp.UI
{
    public class ThrifterTreeNode : TreeNode
    {
        public ThrifterTreeNode(ThriftObject thriftObject)
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