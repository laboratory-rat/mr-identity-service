using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MRMongoTools.Extensions.Identity.Manager;

namespace Dal
{
    public class AppUserManager : MRUserManager<AppUser>
    {
        public AppUserManager(AppUserRepository store) 
            : base(store, null, new PasswordHasher<AppUser>(), null, null, 
                  new UpperInvariantLookupNormalizer(), null, null, 
                  new LoggerFactory().CreateLogger<AppUserManager>()) { }
    }
}
