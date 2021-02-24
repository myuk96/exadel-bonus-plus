using System;
using AutoMapper;

namespace ExadelBonusPlus.Services.Models
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<Bonus, BonusDto>()
                .ForMember(dest => dest.Company,
                    opt =>
                    {
                        opt.MapFrom<BonusResolver>();
                    })
                .ReverseMap();
            CreateMap<AddBonusDto, Bonus>().AfterMap((src, dest) => dest.Id = Guid.NewGuid());
            CreateMap<UpdateBonusDto, Bonus>().ReverseMap();
            CreateMap<UpdateBonusDto, BonusDto>().ReverseMap();
            CreateMap<Bonus, BonusStatisticDto>().ForMember(dest => dest.CompanyName,
                opt =>
                {
                    opt.MapFrom<BonusStatisticCompanyNameResolver>();
                }).ForMember(dest => dest.Visits,
                opt =>
                {
                    opt.MapFrom<BonusStatisticVisitsResolver>();
                });

            //Only for tests
            CreateMap<BonusDto, AddBonusDto>();
            CreateMap<UserInfoDTO, ApplicationUser>();

            CreateMap<UpdateUserDTO, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserInfoDTO>()
                .ForMember(dest => dest.Roles,
                    opt => opt.MapFrom<UserResolver>())
                .ReverseMap();
            CreateMap<ApplicationUser, UserInfoHistoryDto>()
                .ReverseMap();
            CreateMap<ApplicationRole, RoleDTO>().ReverseMap();

            CreateMap<AddHistoryDTO, History>().AfterMap((src, dest) =>
            {
                dest.BonusId = src.BonusId;
                dest.CreatorId = src.UserId;

            }).ReverseMap();
            
            CreateMap<History, HistoryDto>()
                .ForMember(d => d.UsegeDate, opt =>
                    opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UserInfo,
                    opt => opt.MapFrom<UserResolver>())
                .ForMember(dest=>dest.Bonus, opt=>
                    opt.MapFrom<BonusResolver>())
                 .ReverseMap();
          
            CreateMap<History, BonusHistoryDto>()
                .ForMember(d => d.UsageDate, opt =>
                    opt.MapFrom(src=>src.CreatedDate))
                .ForMember(dest => dest.UserInfo,
                    opt => opt.MapFrom<UserResolver>())
                .ReverseMap();
            CreateMap<History, UserHistoryDto>()
                .ForMember(d => d.UsageDate, opt => opt.MapFrom(src=>src.CreatedDate))
                .ForMember(dest => dest.BonusDto, opt =>
                    opt.MapFrom<BonusResolver>())
                .ReverseMap();

            //From model to dto
            CreateMap<Vendor, VendorDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<AddVendorDto, Vendor>()
                .AfterMap((src, dest) => dest.Id = Guid.NewGuid());

        }
    }
}
