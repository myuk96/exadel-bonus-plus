using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExadelBonusPlus.Services.Models;
using ExadelBonusPlus.Services.Properties;
using Microsoft.Extensions.Primitives;

namespace ExadelBonusPlus.Services
{
    public class BonusService : IBonusService
    {
        private readonly IBonusRepository _bonusRepository;
        private readonly IVendorService _vendorService;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        public BonusService(IBonusRepository bonusRepository, IMapper mapper, IVendorService vendorService, IHistoryRepository historyRepository)
        {
            _bonusRepository = bonusRepository;
            _mapper = mapper;
            _vendorService = vendorService;
            _historyRepository = historyRepository;
        }

        public async Task<BonusDto> AddBonusAsync(AddBonusDto model, Guid userId, CancellationToken cancellationToken = default)
        {
            if (model is null)
            {
                throw new ArgumentNullException("", Resources.ModelIsNull);
            }

            VendorDto vendorDto = null;
            try
            {
                vendorDto = await _vendorService.GetVendorByIdAsync(model.CompanyId, cancellationToken);
            }
            catch
            {
                throw new ArgumentException(Resources.VendorFindbyIdError);
            }

            var bonus = _mapper.Map<Bonus>(model);
            bonus.SetInitialValues();
            bonus.CreatorId = userId;
            
            await _bonusRepository.AddAsync(bonus, cancellationToken);
            return _mapper.Map<BonusDto>(bonus);
        }

        public async Task<List<BonusDto>> FindAllBonusesAsync(CancellationToken cancellationToken)
        {
            var result = await _bonusRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<BonusDto>>(result);
        }
        
        public async Task<List<BonusDto>> FindBonusesAsync(BonusFilter bonusFilter, CancellationToken cancellationToken)
        {
            var sortBy = bonusFilter?.SortBy ?? "Title";
            if(sortBy != null & typeof(Bonus).GetProperty(sortBy) == null)
                throw new ArgumentException(Resources.PropertyDoesNotExist);

            var result = await _bonusRepository.GetBonusesAsync(bonusFilter, cancellationToken);
            return _mapper.Map<List<BonusDto>>(result);
        }

        public async Task<BonusDto> FindBonusByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }
            var result = await _bonusRepository.GetByIdAsync(id, cancellationToken);
            return result is null ? throw new ArgumentException("", Resources.FindbyIdError) : _mapper.Map<BonusDto>(result);
        }

        public async Task<BonusDto> UpdateBonusAsync(Guid id, UpdateBonusDto model, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }
            if (model is null)
            {
                throw new ArgumentNullException("", Resources.ModelIsNull);
            }
            var bonus = _mapper.Map<Bonus>(model);

            await _bonusRepository.UpdateAsync(id, bonus, cancellationToken);

            return _mapper.Map<BonusDto>(bonus); ;
        }

        public async Task<BonusDto> DeleteBonusAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }

            
            var histories= await _historyRepository.GetBonusHistoryByUsageDate(id, DateTime.MinValue, DateTime.MaxValue,
                    cancellationToken);
            if(histories.Any())
            {
                throw new ArgumentException(Resources.UnableToRemoveBonus);
            }
            
            var result =await _bonusRepository.RemoveAsync(id, cancellationToken);

            return _mapper.Map<BonusDto>(result);
        }

        public async Task<BonusDto> ActivateBonusAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }

            var result =  await _bonusRepository.ActivateBonusAsync(id, cancellationToken);

            return _mapper.Map<BonusDto>(result);
        }

        public async Task<BonusDto> DeactivateBonusAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }

            var result = await _bonusRepository.DeactivateBonusAsync(id, cancellationToken);

            return _mapper.Map<BonusDto>(result);
        }

        public async Task<BonusDto> UpdateBonusRatingAsync(Guid id, double rating, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }

            var result = await _bonusRepository.UpdateBonusRatingAsync(id, rating, cancellationToken);

            return _mapper.Map<BonusDto>(result);
        }

        public async Task<IEnumerable<string>> GetBonusTagsAsync(CancellationToken cancellationToken)
        {
            return  await _bonusRepository.GetBonusTagsAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetCitiesAsync(CancellationToken cancellationToken)
        {
            return await _bonusRepository.GetCitiesAsync(cancellationToken);
        }

        public async Task<List<BonusStatisticDto>> GetBonusStatisticAsync(BonusFilter bonusFilter, CancellationToken cancellationToken)
        {
            var sortBy = bonusFilter?.SortBy ?? "Title";
            if (sortBy != null & typeof(Bonus).GetProperty(sortBy) == null)
                throw new ArgumentException(Resources.PropertyDoesNotExist);

            var result = await _bonusRepository.GetBonusStatisticAsync(bonusFilter, cancellationToken);
            return _mapper.Map<List<BonusStatisticDto>>(result);
        }
    }
}
