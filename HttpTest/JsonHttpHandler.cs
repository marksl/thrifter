using Thrift.Protocol;
using Thrift.Transport;

namespace HttpTest
{
    public class JsonHttpHandler : THttpHandler
    {
        public JsonHttpHandler()
            : base(CreateProcessor(), CreateJsonFactory())
        {
        }

        private static Twitter.Processor CreateProcessor()
        {
            return new Twitter.Processor(new TwitterImplementation());
        }

        private static TJSONProtocol.Factory CreateJsonFactory()
        {
            return new TJSONProtocol.Factory();
        }
    }
}
