using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ExadelBonusPlus.Services.Models
{
    public class BonusStatisticCompanyNameResolver : IValueResolver<Bonus, BonusStatisticDto, string>
    {
        private readonly IVendorService _vendorService;
        private readonly IMapper _mapper;
        public BonusStatisticCompanyNameResolver(IVendorService vendorService, IMapper mapper)
        {
            _vendorService = vendorService;
            _mapper = mapper;
        }

        public  string Resolve(Bonus source, BonusStatisticDto destination, string destMember, ResolutionContext context)
        {
                try
                {
                    return _mapper.Map<VendorDto>(_vendorService.GetVendorByIdAsync(source.CompanyId).GetAwaiter()
                        .GetResult()).Name;
                }
                catch
                {
                    //Vendor does not finded by Id
                    return String.Empty;
                }
        }
    }
}
