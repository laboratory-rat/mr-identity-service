using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Interface;

namespace Infrastructure.Entities
{
    public class UserInvite : MREntity, IMREntity
    {
        public string UserId { get; set; }

        public bool IsByIdentity { get; set; }

        public string ProviderId { get; set; }
        public string ProviderName { get; set; }

        public string Code { get; set; }
    }
}
