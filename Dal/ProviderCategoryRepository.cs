using Infrastructure.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Interface;
using MRApiCommon.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal
{
    public class ProviderCategoryRepository : MRMongoRepository<ProviderCategory>, IMRRepository<ProviderCategory>
    {
        public ProviderCategoryRepository(IOptions<MRDbOptions> options) : base(options)
        {
        }

        public async Task<IEnumerable<ProviderCategory>> Search(int skip, int limit, string q)
        {
            var query = _builder
                .Eq(x => x.State, MRApiCommon.Infrastructure.Enum.MREntityState.Active)
                .Skip(skip)
                .Limit(limit);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim().ToLowerInvariant();
                query.Match(x => x.Translations, x => x.Name.ToLowerInvariant().Contains(q));
            }

            return await GetByQuery(query);
        }
    }
}
