using System;
using System.Reflection;
using Thrift.Protocol;
using Thrift.Transport;

namespace TestApp
{
    public class ThriftMethod
    {
        private readonly ClassAndMethod _classAndMethod;
        private readonly object[] _parameters;

        public ThriftMethod(ClassAndMethod classAndMethod, object[] parameters)
        {
            _classAndMethod = classAndMethod;
            _parameters = parameters;
        }

        public ThriftObject Invoke(string url, string protocolName)
        {
            TTransport transport = Thrift.GetTransport(url);
            TProtocol protocol = Thrift.GetProtocol(protocolName, transport);

            object instance = Activator.CreateInstance(_classAndMethod.ClassType, protocol);
            object[] parameters = _parameters;

            try
            {
                object response = _classAndMethod.Method.Invoke(instance, parameters);
                if (response != null)
                {
                    ThriftObject thriftObjects = ThriftResponseObjectFactory.CreateResponseThriftObjects(response, "Response", null);
                    return thriftObjects;
                }
            }
            catch (TargetInvocationException target)
            {
                var thriftObjects = ThriftResponseObjectFactory.CreateResponseThriftObjects(target.InnerException, "Response", null);
                return thriftObjects;
            }

            return null;
        }
    }
}