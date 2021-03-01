using AutoMapper;

namespace ExadelBonusPlus.Services.Models
{
    public class BonusResolver : IValueResolver<Bonus, BonusDto, VendorDto>, IValueResolver<History, HistoryDto, BonusDto>, IValueResolver<History, UserHistoryDto, BonusDto>
    {
        private readonly IVendorService _vendorService;
        private readonly IBonusService _bonusService;
        private readonly IMapper _mapper;
        public BonusResolver(IVendorService vendorService, IBonusService bonusService, IMapper mapper)
        {
            _vendorService = vendorService;
            _bonusService = bonusService;
            _mapper = mapper;
        }

        public  VendorDto Resolve(Bonus source, BonusDto destination, VendorDto destMember, ResolutionContext context)
        {
                try
                {
                    return _mapper.Map<VendorDto>(_vendorService.GetVendorByIdAsync(source.CompanyId).GetAwaiter()
                        .GetResult());
                }
                catch
                {
                    //Vendor does not finded by Id
                    return new VendorDto();
                }
        }

        public BonusDto Resolve(History source, HistoryDto destination, BonusDto destMember, ResolutionContext context)
        {
            try
            {
                return _mapper.Map<BonusDto>(_bonusService.FindBonusByIdAsync(source.BonusId).GetAwaiter()
                    .GetResult());
            }
            catch
            {
                //Vendor does not finded by Id
                return new BonusDto();
            }
        }

        public BonusDto Resolve(History source, UserHistoryDto destination, BonusDto destMember, ResolutionContext context)
        {
            try
            {
                return _mapper.Map<BonusDto>(_bonusService.FindBonusByIdAsync(source.BonusId).GetAwaiter()
                    .GetResult());
            }
            catch
            {
                //Vendor does not finded by Id
                return new BonusDto();
            }
        }
    }
}
