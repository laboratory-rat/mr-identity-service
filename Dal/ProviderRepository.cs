using Infrastructure.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Enum;
using MRApiCommon.Infrastructure.Interface;
using MRApiCommon.Options;
using MRApiCommon.Tools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal
{
    public class ProviderRepository : MRMongoRepository<Provider>, IMRRepository<Provider>
    {
        public ProviderRepository(IOptions<MRDbOptions> options) : base(options) { }

        public async Task<Provider> GetShortBySlug(string slug)
        {
            slug = slug.Trim().ToLowerInvariant();

            var query = _builder
                .Eq(x => x.Slug, slug)
                .Eq(x => x.State, MREntityState.Active)
                    .ProjectionInclude(x => x.Id)
                    .ProjectionInclude(x => x.Name)
                    .ProjectionInclude(x => x.IsVisible)
                    .ProjectionInclude(x => x.IsLoginEnabled)
                    .ProjectionInclude(x => x.Slug)
                    .ProjectionInclude(x => x.UpdateTime)
                    .ProjectionInclude(x => x.CreateTime)
                    .ProjectionInclude(x => x.State);

            return await GetByQueryFirst(query);
        }
        public async Task<Provider> GetShortById(string id)
        {
            var query = _builder
                .Eq(x => x.Id, id)
                .Eq(x => x.State, MREntityState.Active)
                    .ProjectionInclude(x => x.Id)
                    .ProjectionInclude(x => x.Name)
                    .ProjectionInclude(x => x.IsVisible)
                    .ProjectionInclude(x => x.IsLoginEnabled)
                    .ProjectionInclude(x => x.Slug)
                    .ProjectionInclude(x => x.UpdateTime)
                    .ProjectionInclude(x => x.CreateTime)
                    .ProjectionInclude(x => x.State);

            return await GetByQueryFirst(query);
        }

        #region Roles

        public async Task<List<ProviderRole>> GetRolesById(string id)
        {
            var query = _builder
                .Eq(x => x.Id, id)
                .Eq(x => x.State, MREntityState.Active)
                    .ProjectionInclude(x => x.Roles)
                    .ProjectionInclude(x => x.Id);

            return (await GetByQueryFirst(query))?.Roles
                ?? new List<ProviderRole>();
        }
        public async Task<List<ProviderRole>> GetRolesBySlug(string slug)
        {
            var query = _getSlugQuery(slug)
                .ProjectionInclude(x => x.Roles)
                .ProjectionInclude(x => x.Id);

            return (await GetByQueryFirst(query))?.Roles
                ?? new List<ProviderRole>();
        }

        public async Task<bool> RoleNameExistsBySlug(string slug, string name)
        {
            var query = _builder
                .Eq(x => x.Slug, slug)
                .Eq(x => x.State, MREntityState.Active)
                .Match(x => x.Roles, x => x.Name == name);

            return await ExistsOne(query);
        }

        public async Task<bool> InsertRole(string id, ProviderRole role)
        {
            var query = _getIdQuery(id)
                .UpdateSet(x => x.UpdateTime, DateTime.UtcNow)
                .UpdateAddToSet(x => x.Roles, role);

            await UpdateByQuery(query);
            return true;
        }

        public async Task<bool> InsertRoleBySlug(string slug, ProviderRole role)
        {
            var query = _getSlugQuery(slug)
                .UpdateSet(x => x.UpdateTime, DateTime.UtcNow)
                .UpdateAddToSet(x => x.Roles, role);

            await UpdateByQuery(query);
            return true;
        }


        public async Task<bool> RemoveRole(string id, string roleName)
        {
            var query = _getIdQuery(id)
                .Match(x => x.Roles, x => x.Name == roleName)
                .UpdatePullWhere(x => x.Roles, x => x.Name == roleName);

            await UpdateByQuery(query);
            return true;
        }
        public async Task<bool> RemoveRoleBySlug(string slug, string roleName)
        {
            var query = _getSlugQuery(slug)
                               .Match(x => x.Roles, x => x.Name == roleName)
                .UpdatePullWhere(x => x.Roles, x => x.Name == roleName);

            await UpdateByQuery(query);
            return true;
        }

        #endregion Roles

        #region Fingerprints

        public async Task<bool> InsertFingerprint(string id, ProviderFingerprint fingerpeint)
        {
            var query = _getIdQuery(id)
                .UpdateSet(x => x.UpdateTime, DateTime.UtcNow)
                .UpdateAddToSet(x => x.Fingerprints, fingerpeint);

            await UpdateByQuery(query);
            return true;
        }

        public async Task<bool> RemoveFingerprint(string id, string fingerprintName)
        {
            var query = _getIdQuery(id)
                .UpdatePullWhere(x => x.Fingerprints, x => x.Name == fingerprintName);

            await UpdateByQuery(query);
            return true;
        }

        public async Task UpdateFingerprints(Provider entity)
        {
            var query = _builder
                .Eq(x => x.Id, entity.Id)
                .UpdateSet(x => x.UpdateTime, DateTime.UtcNow)
                .UpdateSet(x => x.Fingerprints, entity.Fingerprints);

            await UpdateByQuery(query);
        }

        public async Task<Provider> GetByFingerprint(string fingerprint)
        {
            var query = _builder
                .Eq(x => x.State, MREntityState.Active)
                .Match(x => x.Fingerprints, x => x.Fingerprint == fingerprint);

            return await GetByQueryFirst(query);
        }

        #endregion Fingerprints

        #region Workers

        public async Task<long> UserInWorkersCount(string userId)
        {
            var query = _builder
                .Eq(x => x.State, MREntityState.Active)
                .Match(x => x.Workers, x => x.UserId == userId);

            return await Count(query);
        }

        public async Task<bool> IsWorkerInRoleBySlug(string slug, string userId, ProviderWorkerRole role)
        {
            var query = _builder
                .Eq(x => x.Slug, slug)
                .Eq(x => x.State, MREntityState.Active)
                .Match(x => x.Workers, x => x.UserId == userId && x.Roles.Contains(role));

            return await ExistsOne(query);
        }

        public async Task<bool> IsWorkerExistsBySlug(string slug, string userId)
        {
            var query = _builder
                .Eq(x => x.Slug, slug)
                .Eq(x => x.State, MREntityState.Active)
                .Match(x => x.Workers, x => x.UserId == userId);

            return await ExistsOne(query);
        }

        public async Task<List<ProviderWorker>> GetWorkersBySlug(string slug)
        {
            var query = _getSlugQuery(slug)
                .ProjectionInclude(x => x.Id)
                .ProjectionInclude(x => x.Workers);

            return (await GetByQueryFirst(query))?.Workers;
        }

        public async Task<bool> InsertWorkersBySlug(string slug, ProviderWorker worker) => await InsertWorkersBySlug(slug, new List<ProviderWorker>() { worker });
        public async Task<bool> InsertWorkersBySlug(string slug, IEnumerable<ProviderWorker> workers)
        {
            var query = _getSlugQuery(slug)
                .UpdateAddToSetEach(x => x.Workers, workers);

            await UpdateByQuery(query);
            return true;
        }

        public async Task<bool> InsertWorkersById(string id, IEnumerable<ProviderWorker> workers)
        {
            var query = _getIdQuery(id)
                .UpdateAddToSetEach(x => x.Workers, workers);

            await UpdateByQuery(query);
            return true;
        }

        public async Task<bool> RemoveWorkerBySlug(string slug, string userId)
        {
            var query = _getSlugQuery(slug)
                .Match(x => x.Workers, x => x.UserId == userId)
                .UpdatePullWhere(x => x.Workers, x => x.UserId == userId);

            await UpdateByQuery(query);
            return true;
        }

        public async Task<bool> RemoveWorker(string id, string userId)
        {
            var query = _getIdQuery(id)
                .Match(x => x.Workers, x => x.UserId == userId)
                .UpdatePullWhere(x => x.Workers, x => x.UserId == userId);

            await UpdateByQuery(query);
            return true;
        }

        public async Task<bool> UpdateWorkerBySlug(string slug, string userId, IEnumerable<ProviderWorkerRole> roles)
        {
            var query = _builder
                .Eq(x => x.Slug, slug)
                .Eq(x => x.State, MREntityState.Active)
                .Match(x => x.Workers, x => x.UserId == userId)
                .UpdateSet(x => x.Workers[-1].Roles, roles);

            await UpdateByQuery(query);
            return true;
        }

        public async Task<bool> UpdateWorker(string id, string userId, IEnumerable<ProviderWorkerRole> roles)
        {
            var query = _builder
                .Eq(x => x.Id, id)
                .Eq(x => x.State, MREntityState.Active)
                .Match(x => x.Workers, x => x.UserId == userId)
                .UpdateSet(x => x.Workers[-1].Roles, roles);

            await UpdateByQuery(query);
            return true;
        }

        #endregion Workers

        public async Task<bool> ExistsWithOwner(string id, string userId)
            => await ExistsOne(_builder
                .Eq(x => x.Id, id)
                .Eq(x => x.State, MREntityState.Active)
                .Where(x => x.Owner != null && x.Owner.Id == userId));

        public async Task<bool> ExistsWithOwnerSlug(string slug, string userId)
            => await ExistsOne(_builder
                .Eq(x => x.Slug, slug)
                .Eq(x => x.State, MREntityState.Active)
                .Where(x => x.Owner != null && x.Owner.Id == userId));

        protected MongoQueryBuilder<Provider, string> _getIdQuery(string id) => _builder.Eq(x => x.Id, id).Eq(x => x.State, MREntityState.Active);
        protected MongoQueryBuilder<Provider, string> _getSlugQuery(string slug) => _builder.Eq(x => x.Slug, slug).Eq(x => x.State, MREntityState.Active);
    }
}
