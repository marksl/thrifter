using Thrift.Protocol;
using Thrift.Transport;

namespace HttpTest
{
    public class BinaryHttpHandler : THttpHandler
    {
        public BinaryHttpHandler()
            : base(CreateProcessor(), CreateJsonFactory())
        {
        }

        private static Twitter.Processor CreateProcessor()
        {
            return new Twitter.Processor(new TwitterImplementation());
        }

        private static TBinaryProtocol.Factory CreateJsonFactory()
        {
            return new TBinaryProtocol.Factory();
        }
    }
}