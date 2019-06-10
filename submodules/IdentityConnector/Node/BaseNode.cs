using MRIdentityClient.Tools;

namespace MRIdentityClient.Node
{
    public abstract class BaseNode
    {
        protected readonly IdentityClient _client;
        protected Navigator _navigator => _client?.Navigator;
        protected RequestSender _http => new RequestSender();

        public BaseNode(IdentityClient client)
        {
            _client = client;
        }
    }
}
