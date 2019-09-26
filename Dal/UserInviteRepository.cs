using Infrastructure.Entities;
using Microsoft.Extensions.Options;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Enum;
using MRApiCommon.Infrastructure.Interface;
using MRApiCommon.Options;
using System.Threading.Tasks;

namespace Dal
{
    public class UserInviteRepository : MRMongoRepository<UserInvite>, IMRRepository<UserInvite>
    {
        public UserInviteRepository(IOptions<MRDbOptions> options) : base(options) { }

        public async Task<UserInvite> GetByCode(string code)
        {
            var query = _builder
                .Eq(x => x.State, MREntityState.Active)
                .Eq(x => x.Code, code);

            return await GetByQueryFirst(query);
        }
    }
}
