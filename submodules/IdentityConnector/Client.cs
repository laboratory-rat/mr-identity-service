
using MRIdentityClient.Node;
using MRIdentityClient.Tools;

namespace MRIdentityClient
{
    public class IdentityClient
    {
        public readonly Navigator Navigator;
        public readonly string Secret;

        public SignupNode Signup { get; set; }

        protected IdentityClient() : this(string.Empty) { }

        public IdentityClient(string secret)
        {
            Secret = secret;
            Navigator = new Navigator(Secret);

            Signup = new SignupNode(this);
        }
    }
}
