using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ExadelBonusPlus.Services.Models
{
    public class BonusStatisticVisitsResolver : IValueResolver<Bonus, BonusStatisticDto, int>
    {
        private readonly IHistoryService _historyService;
        private readonly IMapper _mapper;
        public BonusStatisticVisitsResolver(IHistoryService historyService, IMapper mapper)
        {
            _historyService = historyService;
            _mapper = mapper;
        }

        public  int Resolve(Bonus source, BonusStatisticDto destination, int destMember, ResolutionContext context)
        {
            try
            {
                return _historyService.GetCountHistoryByBonusIdAsync(source.Id).GetAwaiter()
                    .GetResult();
            }
            catch
            {
                // does not finded by Id
                return 0;
            }
        }
    }
}
