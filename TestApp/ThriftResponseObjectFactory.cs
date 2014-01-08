using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace TestApp
{
    public static class ThriftResponseObjectFactory
    {
        private static void CreateResponseThriftObjects(object obj, IEnumerable<PropertyInfo> getParameters, ThriftObject parentObj)
        {
            if (obj.GetType().Name == "RuntimeModule")
                return;

            foreach (var parameter in getParameters)
            {
                if (parameter.Name == "TargetSite")
                    continue;

                try
                {
                    var newObj = parameter.GetValue(obj, null);

                    CreateResponseThriftObjects(newObj, parameter.Name, parentObj);
                }
                catch{}
            }
        }

        public static ThriftObject CreateResponseThriftObjects(object obj, string propertyName, ThriftObject parentObj)
        {
            if (obj == null)
                return null;

            if (propertyName == "TargetSite")
                return null;

            var objType = obj.GetType();

            var t = new ThriftObject(parentObj, obj, objType, string.Format("{0}={1}", propertyName, obj.GetType().Name));
            if (parentObj != null)
            {
                parentObj.ChildObjs.Add(t);
            }

            if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = (IEnumerable)obj;

                foreach (var l in list)
                {
                    var t2 = new ThriftObject(t, l, l.GetType(), string.Format("{0}={1}", propertyName, l.GetType().Name));
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