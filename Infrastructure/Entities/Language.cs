using MRApiCommon.Infrastructure.Database;
using MRApiCommon.Infrastructure.Interface;

namespace Infrastructure.Entities
{
    public class Language : MREntity, IMREntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string NativeName { get; set; }
    }
}
