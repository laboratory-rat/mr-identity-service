using MRIdentityClient.Models.User;
using MRIdentityClient.Response;
using System.Threading.Tasks;

namespace MRIdentityClient.Node
{
    public class SignupNode : BaseNode
    {
        public SignupNode(IdentityClient client) : base(client) { }

        public async Task<IdentityResponse<ApproveLogin>> ApproveLogin(string token)
            => await _http.Put<ApproveLogin>(_navigator.ApproveLogin(token));
    }
}
