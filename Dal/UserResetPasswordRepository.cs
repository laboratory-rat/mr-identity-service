using Infrastructure.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Enum;
using MRApiCommon.Infrastructure.Interface;
using MRApiCommon.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal
{
    public class UserResetPasswordRepository : MRMongoRepository<UserResetPassword>, IMRRepository<UserResetPassword>
    {
        public UserResetPasswordRepository(IOptions<MRDbOptions> options) : base(options) { }

        public async Task<UserResetPassword> GetByUser(string userId, string code)
        {
            var query = _builder
                .Eq(x => x.UserId, userId)
                .Eq(x => x.State, MREntityState.Active)
                .Eq(x => x.Code, code)
                .Sorting(x => x.CreateTime, false);

            return await GetByQueryFirst(query);
        }

        public async Task ResetAllCodes(string userId)
        {
            var query = _builder
                .Eq(x => x.UserId, userId)
                .Eq(x => x.State, MREntityState.Active)
                .UpdateSet(x => x.UpdateTime, DateTime.UtcNow)
                .UpdateSet(x => x.State, MREntityState.Archived);

            await UpdateManyByQuery(query);
        }
    }
}
