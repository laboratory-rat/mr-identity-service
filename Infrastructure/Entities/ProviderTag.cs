using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Interface;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class ProviderTag : MREntity, IMREntity
    {
        public string Key { get; set; }
        public List<ProviderTagTranslation> Translations { get; set; }

        public string UserCreatedId { get; set; }
    }

    public class ProviderTagTranslation
    {
        public string LanguageCode { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
