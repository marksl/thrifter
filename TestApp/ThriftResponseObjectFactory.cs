using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace TestApp
{
    public static class ThriftResponseObjectFactory
    {
        private static void CreateResponseThriftObjects(object obj, IEnumerable<PropertyInfo> getParameters, ThriftObject parentObj)
        {
            foreach (var parameter in getParameters)
            {
                var newObj = parameter.GetValue(obj, null);

                CreateResponseThriftObjects(newObj, parameter.Name, parentObj);
            }
        }

        public static ThriftObject CreateResponseThriftObjects(object obj, string propertyName, ThriftObject parentObj)
        {
            var objType = obj.GetType();

            var t = new ThriftObject
            {
                Obj = obj, // Probably not needed
                ParentObj = parentObj,
                PropertyName = string.Format("{0}={1}", propertyName, obj.GetType().Name),
                ObjType = objType
            };
            if (parentObj != null)
            {
                parentObj.ChildObjs.Add(t);
            }

            if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = (IEnumerable)obj;

                foreach (var l in list)
                {
                    var t2 = new ThriftObject
                    {
                        Obj = l, // Probably not needed
                        ParentObj = t,
                        PropertyName = string.Format("{0}={1}", propertyName, l.GetType().Name),
                        ObjType = l.GetType()
                    };
                    t.ChildObjs.Add(t2);

                    CreateResponseThriftObjects(l, l.GetType().GetProperties(), t2);
                }

                return t;
            }

            if (objType.IsValueType || obj is string)
            {
                return t;
            }

            CreateResponseThriftObjects(obj, objType.GetProperties(), t);

            return t;
        }
    }
}