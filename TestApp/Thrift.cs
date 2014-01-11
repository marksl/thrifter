using System;
using Thrift.Protocol;
using Thrift.Transport;

namespace TestApp
{
// Test update
    public static class Thrift
    {
        public static TProtocol GetProtocol(string protocolName, TTransport transport)
        {
            TProtocol protocol;
            switch (protocolName)
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

        public static TTransport GetTransport(string url)
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
                var parts = url.Split(new[] { ':' });

                transport = new TSocket(parts[0], Convert.ToInt32(parts[1]));
            }
            return transport;
        }

    }
}