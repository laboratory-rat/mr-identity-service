using System.Collections.Generic;

namespace MRIdentityClient.Models.User
{
    public class ApproveLogin
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public List<string> Roles { get; set; }
    }
}
