using Infrastructure.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Enum;
using MRApiCommon.Infrastructure.Interface;
using MRApiCommon.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal
{
    public class ProviderTagRepository : MRMongoRepository<ProviderTag>, IMRRepository<ProviderTag>
    {
        public ProviderTagRepository(IOptions<MRDbOptions> options) : base(options) { }

        public async Task<IEnumerable<ProviderTag>> Search(int skip, int limit, string q)
        {
            var query = _builder
                .Eq(x => x.State, MREntityState.Active)
                .Skip(skip)
                .Limit(limit)
                .Sorting(x => x.CreateTime, true);

            if (!string.IsNullOrEmpty(q))
            {
                q = q.Trim().ToLowerInvariant();
                query
                    .Or(
                        _builder.FilterBuilder.Regex(x => x.Key, new MongoDB.Bson.BsonRegularExpression(q, "i")),
                        _builder.FilterBuilder.ElemMatch(x => x.Translations, x => x.Name.ToLowerInvariant().Contains(q))
                    );
            }


            return await GetByQuery(query);
        }
    }
}
