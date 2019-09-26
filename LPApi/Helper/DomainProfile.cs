using AutoMapper;
using IdentityApi.Extensions;
using Infrastructure.Entities;
using Infrastructure.Model.Common;
using Infrastructure.Model.Provider;
using Infrastructure.Model.User;
using MRApiCommon.Infrastructure.IdentityExtensions.Components;
using System;
using System.Linq;

namespace IdentityApi.Helper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<AppUser, UserLoginResponseModel>()
                .ForMember(x => x.ImageSrc, opt => opt.MapFrom(x => x.Image == null ? null : x.Image.Url));

            // common models
            CreateMap<Language, LanguageDisplayModel>();

            CreateMap<Image, ImageModel>();
            CreateMap<ImageModel, Image>();

            // user
            CreateMap<AppUser, UserShortDataModel>()
                .ForMember(x => x.AvatarSrc, opt => opt.MapFrom(z => z.Image == null ? null : z.Image.Url))
                .ForMember(x => x.CreatedTime, opt => opt.MapFrom(z => z.CreateTime.ToLocalTime()))
                .ForMember(x => x.UpdatedTime, opt => opt.MapFrom(z => z.UpdateTime.HasValue ? z.UpdateTime.Value.ToLocalTime() : DateTime.UtcNow));
            CreateMap<AppUser, UserDataModel>()
                .IncludeBase<AppUser, UserShortDataModel>()
                .ForMember(x => x.Roles, opt => opt.MapFrom(z => z.Roles.Select(x => x.Name).ToList()));
            CreateMap<UserSocial, UserDataSocialModel>()
                .ForMember(x => x.CreatedTime, opt => opt.MapFrom(z => z.CreatedTime.ToLocalTime()));
            CreateMap<AppUserProvider, UserDataProviderModel>();
            CreateMap<MRUserTel, UserDataTel>();
            CreateMap<AppUser, UserDisplayModel>();
            CreateMap<MRUserTel, UserTelDisplayModel>();

            CreateMap<UserSignupModel, AppUser>();

            // provider models
            CreateMap<ProviderCategory, ProviderCategoryDisplayModel>();
            CreateMap<CategoryUpdateModel, ProviderCategory>();
            CreateMap<CategoryTranslationUpdateModel, ProviderCategoryTranslation>();
            CreateMap<AppUser, ProviderOwner>();

            CreateMap<ProviderUpdateModel, Provider>()
                .ForMember(x => x.Tags, opt => opt.Ignore())
                .ForMember(x => x.Category, opt => opt.Ignore());
            CreateMap<ProviderTranslationUpdateModel, ProviderTranslation>();
            CreateMap<ProviderSocialUpdateModel, ProviderSocial>();

            CreateMap<Provider, ProviderUpdateModel>();
            CreateMap<ProviderTranslation, ProviderTranslationUpdateModel>();
            CreateMap<ProviderSocial, ProviderSocialUpdateModel>();
            
            // provider fingerprint
            CreateMap<ProviderFingerprint, ProviderFingerprintDisplayModel>()
                .ForMember(x => x.FingerprintUpdateTime, t => t.MapFrom(x => x.FingerprintUpdateTime.ToLocalTime()));
            CreateMap<ProviderFingerprintCreateModel, ProviderFingerprint>()
                .ForMember(x => x.FingerprintUpdateTime, t => t.MapFrom(x => DateTime.UtcNow));

            // provider roles
            CreateMap<ProviderRole, ProviderRoleDisplayModel>();
            CreateMap<ProviderRoleCreateModel, ProviderRole>();

            // provider tag
            CreateMap<ProviderTag, ProviderTagDisplayModel>();
            CreateMap<ProviderTagCreateModel, ProviderTag>();
            CreateMap<ProviderTagTranslationCreateModel, ProviderTagTranslation>();

            // provider worker
            CreateMap<ProviderWorker, ProviderWorkerDisplayModel>();
            CreateMap<ProviderWorkerUpdateModel, ProviderWorker>();

            CreateMap<Provider, ProviderShortDisplayModel>()
                .ForMember(x => x.Category, t => t.Ignore())
                .ForMember(x => x.Tags, t => t.Ignore());
            CreateMap<ProviderCategory, ProviderCategoryDisplayModel>();
            CreateMap<ProviderSocial, ProviderSocialDisplayModel>();
            CreateMap<ProviderTag, ProviderTagDisplayModel>();
            CreateMap<Provider, ProviderDisplayModel>()
                .IncludeBase<Provider, ProviderShortDisplayModel>()
                .ForMember(x => x.Socials, t => t.Ignore());
        }
    }
}
