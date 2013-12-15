using Thrift.Protocol;
using Thrift.Transport;

namespace HttpTest
{
    public class CompactHttpHandler : THttpHandler
    {
        public CompactHttpHandler()
            : base(CreateProcessor(), CreateJsonFactory())
        {
        }

        private static Twitter.Processor CreateProcessor()
        {
            return new Twitter.Processor(new TwitterImplementation());
        }

        private static TCompactProtocol.Factory CreateJsonFactory()
        {
            return new TCompactProtocol.Factory();
        }
    }
}