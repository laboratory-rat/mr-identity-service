using Infrastructure.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MRApiCommon.Infrastructure.Enum;
using MRApiCommon.Infrastructure.IdentityExtensions.Store;
using MRApiCommon.Options;
using MRApiCommon.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dal
{
    public class AppUserRepository : MRUserStore<AppUser>
    {
        public AppUserRepository(IOptions<MRDbOptions> settings) : base(settings) { }

        public async Task<IEnumerable<AppUser>> Get(int skip, int limit, string q)
        {
            var query = _builder
                .Sorting(x => x.CreateTime, true)
                .Limit(limit)
                .Skip(skip);

            if (!string.IsNullOrWhiteSpace(q))
            {
                query.Or(
                    _builder.FilterBuilder.Regex(x => x.FirstName, new MongoDB.Bson.BsonRegularExpression(q, "i")),
                    _builder.FilterBuilder.Regex(x => x.LastName, new MongoDB.Bson.BsonRegularExpression(q, "i")),
                    _builder.FilterBuilder.Regex(x => x.Email, new MongoDB.Bson.BsonRegularExpression(q, "i"))
                );
            }

            query = _shortUserProjection(query);

            return await GetByQuery(query);
        }

        public async Task<IEnumerable<AppUser>> GetShortById(IEnumerable<string> ids)
        {
            var query = _builder
                .Eq(x => x.State, MREntityState.Active)
                .In(x => x.Id, ids);

            query = _shortUserProjection(query);

            return await GetByQuery(query);
        }

        public async Task<AppUser> GetShortById(string id)
        {
            var query = _builder
                .Eq(x => x.State, MREntityState.Active)
                .Eq(x => x.Id, id);

            query = _shortUserProjection(query);

            return await GetByQueryFirst(query);
        }

        #region Provider

        public async Task<ICollection<AppUser>> GetByProvider(int skip, int limit, string providerId)
        {
            var query = _builder
                .Eq(x => x.State, MREntityState.Active)
                .Eq(x => x.Isblocked, false)
                .Match(x => x.ConnectedProviders, x => x.ProviderId == providerId)
                .Skip(skip)
                .Limit(limit)
                .Sorting(x => x.CreateTime, true);

            return (await GetByQuery(query))?.ToList()
                ?? new List<AppUser>();
        }

        public async Task<AppUserProvider> GetProvider(string id, string providerId)
        {
            var query = _clearBuilder
                .Eq(x => x.Id, id)
                .Match(x => x.ConnectedProviders, x => x.ProviderId == providerId);

            query.Projection = query
                .ProjectionBuilder
                .Include(x => x.ConnectedProviders)
                .ElemMatch(x => x.ConnectedProviders, x => x.ProviderId == providerId)
                .Slice(x => x.ConnectedProviders, 0, 1);

            return (await GetByQueryFirst(query))?.ConnectedProviders?.FirstOrDefault();
        }

        public async Task<UpdateResult> AddProvider(string id, AppUserProvider provider)
        {
            var query = _clearBuilder
                .Eq(x => x.Id, id)
                .UpdateAddToSet(x => x.ConnectedProviders, provider);

            await UpdateByQuery(query);
            return new UpdateResult.Acknowledged(1, 1, id);
        }

        public async Task<UpdateResult> UpdateProviderRoles(string id, string providerId, List<ProviderRole> roles)
        {
            var query = _clearBuilder
                .Eq(x => x.Id, id)
                .Match(x => x.ConnectedProviders, x => x.ProviderId == providerId)
                .UpdateSet(x => x.ConnectedProviders[-1].Roles, roles);

            await UpdateByQuery(query);
            return new UpdateResult.Acknowledged(1, 1, id);
        }

        public async Task<UpdateResult> AddProviderMeta(string id, string providerId, AppUserProviderMeta meta)
        {
            var query = _clearBuilder
                .Eq(x => x.Id, id)
                .Match(x => x.ConnectedProviders, x => x.ProviderId == providerId)
                .UpdateAddToSet(x => x.ConnectedProviders[-1].Metadata, meta);

            await UpdateByQuery(query);
            return new UpdateResult.Acknowledged(1, 1, id);
        }

        #endregion Provider

        protected MongoQueryBuilder<AppUser, string> _shortUserProjection(MongoQueryBuilder<AppUser, string> query)
            => query
                .ProjectionInclude(x => x.Image)
                .ProjectionInclude(z => z.Birthday)
                .ProjectionInclude(z => z.CreateTime)
                .ProjectionInclude(z => z.Email)
                .ProjectionInclude(z => z.FirstName)
                .ProjectionInclude(z => z.Id)
                .ProjectionInclude(z => z.Isblocked)
                .ProjectionInclude(z => z.IsEmailConfirmed)
                .ProjectionInclude(z => z.LastName)
                .ProjectionInclude(z => z.UpdateTime)
                .ProjectionInclude(z => z.Status)
                .ProjectionInclude(z => z.UserName);

    }
}
