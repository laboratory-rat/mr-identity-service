using Infrastructure.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Interface;
using MRApiCommon.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal
{
    public class LanguageRepository : MRMongoRepository<Language>, IMRRepository<Language>
    {
        public LanguageRepository(IOptions<MRDbOptions> mongoDatabase) : base(mongoDatabase) { }

        public async Task<IEnumerable<Language>> Search(int skip, int limit, string q)
        {
            var query = _builder
                .Skip(skip)
                .Limit(limit);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query
                    .Or(
                        new FilterDefinition<Language>[] {
                            _builder.FilterBuilder.Regex(x => x.Code, new MongoDB.Bson.BsonRegularExpression(q, "i")),
                            _builder.FilterBuilder.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(q, "i")),
                            _builder.FilterBuilder.Regex(x => x.NativeName, new MongoDB.Bson.BsonRegularExpression(q, "i"))
                        });
            }

            return await GetByQuery(query);
        }
    }
}
