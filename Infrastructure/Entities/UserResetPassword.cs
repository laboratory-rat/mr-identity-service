using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Interface;

namespace Infrastructure.Entities
{
    public class UserResetPassword : MREntity, IMREntity
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}
