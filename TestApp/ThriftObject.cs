using System;
using System.Collections.Generic;

namespace TestApp
{
    public class ThriftObject
    {
        public ThriftObject(ThriftObject parentObj, object obj, Type objType, string propertyName)
        {
            ParentObj = parentObj;
            Obj = obj;
            ObjType = objType;
            PropertyName = propertyName;

            IsNull = true;
            ChildObjs = new List<ThriftObject>();
        }

        public string PropertyName { get; private set; }
        public ThriftObject ParentObj { get; private set; }
        public object Obj { get; private set; }
        public Type ObjType { get; private set; }
        public bool IsNull { get; set; }
        
        public List<ThriftObject> ChildObjs { get; private set; } 

        public void SetProperty(string value)
        {
            try
            {
                if (ObjType == typeof(int))
                    Obj = Convert.ToInt32(value);
                else if (ObjType == typeof(short))
                    Obj = Convert.ToInt16(value);
                else if (ObjType == typeof(long))
                    Obj = Convert.ToInt64(value);
                else if (ObjType == typeof(string))
                    Obj = value;
                else if (ObjType == typeof(double))
                    Obj = Convert.ToDouble(value);
                else if (ObjType == typeof(bool))
                    Obj = Convert.ToBoolean(value);
                else if (ObjType == typeof (char))
                    Obj = Convert.ToChar(value);
                else if (ObjType.IsEnum)
                    Obj = Enum.Parse(Obj.GetType(), value);
            }
            catch
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
}