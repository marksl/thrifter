using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestApp
{
    public static class ThriftObjectFactory
    {
        private static void CreateThriftObjects(IEnumerable<PropertyInfo> getParameters, ThriftObject parentObj)
        {
            foreach (var parameter in getParameters)
            {
                Type type = parameter.PropertyType;
                CreateThriftObjects(type, parameter.Name, parentObj);
            }
        }

        public static List<ThriftObject> CreateThriftObjects(IEnumerable<ParameterInfo> getParameters)
        {
            return getParameters
                .Select(parameter => parameter.ParameterType)
                .Select(x => CreateThriftObjects(x, x.Name, null))
                .ToList();
        }

        private static ThriftObject CreateThriftObjects(Type type, string propertyName, ThriftObject parentObj)
        {
            object instance = null;
            if (type != typeof(string))
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

            if (type.IsValueType || type == typeof(string))
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


    }
}