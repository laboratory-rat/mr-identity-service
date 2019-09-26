using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Interface;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class ProviderCategory : MREntity, IMREntity
    {
        public string Slug { get; set; }
        public List<ProviderCategoryTranslation> Translations { get; set; }
    }

    public class ProviderCategoryTranslation
    {
        public string LanguageCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
    }
}
