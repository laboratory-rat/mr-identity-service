using Dal;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Infrastructure.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Infrastructure.Entities.Enum;
using MRApiCommon.Manager;

namespace Manager
{

    public class BaseManager : MRTokenAuthManager
    {
        protected readonly AppUserManager _appUserManager;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;

        protected bool _currentUserIsAdmin => _userRoles?.Contains(UserRoles.ADMIN) ?? false;

        protected AppUser _currentUser { get; set; }
        protected async Task<AppUser> GetCurrentUser()
        {
            if (_currentUser == null)
            {
                var email = _userEmail;
                if (string.IsNullOrWhiteSpace(email)) return null;

                _currentUser = await _appUserManager.FindByEmailAsync(email);
            }

            return _currentUser;
        }

        protected readonly string DEFAULT_LANGUAGE_CODE = "en";
        protected readonly int MAX_LIMIT = 15;

        public BaseManager(IHttpContextAccessor httpContextAccessor, AppUserManager appUserManager, IMapper mapper,
                           ILoggerFactory _loggerFactory) : base(httpContextAccessor)
        {
            _appUserManager = appUserManager;
            _mapper = mapper;
            _logger = _loggerFactory.CreateLogger(GetType());
        }
    }
}
