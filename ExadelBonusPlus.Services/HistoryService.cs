using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ExadelBonusPlus.Services.Models;
using ExadelBonusPlus.Services.Models.Interfaces;
using ExadelBonusPlus.Services.Properties;

namespace ExadelBonusPlus.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IBonusRepository _bonusRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public HistoryService(IHistoryRepository historyRepository,
                            IBonusRepository bonusRepository,
                            IEmailService emailService,
                            IMapper mapper)
        {
            _historyRepository = historyRepository;
            _bonusRepository = bonusRepository;
            _emailService = emailService;
            _mapper = mapper;

        }
        public async Task<HistoryDto> AddHistory(AddHistoryDTO model, CancellationToken cancellationToken = default)
        {
            if (model is null)
            {
                throw new ArgumentNullException("", Resources.ModelIsNull);
            }
            var history = _mapper.Map<History>(model);
            history.Rating = -1;
            history.CreatedDate = DateTime.UtcNow;
             await _historyRepository.AddAsync(history, cancellationToken);
             _emailService.SendEmailAsync(history);
            return _mapper.Map<HistoryDto>(history);
        }

        public async Task<HistoryDto> DeleteHistory(Guid id,  CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }
            var returnModel = await _historyRepository.GetByIdAsync(id, cancellationToken);
            if (returnModel is null)
            {
                throw new ArgumentNullException("", Resources.DeleteError);
            }

            await _historyRepository.RemoveAsync(id, cancellationToken);
            var history = await _historyRepository.GetByIdAsync(id, cancellationToken);

            return !(history is null) ? _mapper.Map<HistoryDto>(history) : throw new ArgumentNullException("", Resources.DeleteError);
        }

        public async Task<IEnumerable<HistoryDto>> GetAllHistory(CancellationToken cancellationToken = default)
        {
             var history = await _historyRepository.GetAllAsync(cancellationToken);
            return history is null ? throw new ArgumentNullException("", Resources.FindError) : _mapper.Map<List<HistoryDto>>(history);
        }

        public async Task<HistoryDto> GetHistoryById(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }
            var history = await  _historyRepository.GetByIdAsync(id,cancellationToken);
            return !(history is null) ? _mapper.Map<HistoryDto>(history) : throw new ArgumentNullException("", Resources.FindbyIdError);
        }
        
        public async Task<IEnumerable<UserHistoryDto>> GetUserHistoryByUsageDate(Guid userId, DateTime usageDateStart, DateTime usegeDateEnd,  CancellationToken cancellationToken = default)
        {
            var history = await _historyRepository.GetUserHistoryByUsageDate(userId, usageDateStart, usegeDateEnd, cancellationToken);
            return !(history is null) ? _mapper.Map<List<UserHistoryDto>>(history) : throw new ArgumentNullException("", Resources.FindError);
        }

        public async Task<IEnumerable<BonusHistoryDto>> GetBonusHistoryByUsageDate(Guid bonusId, DateTime usageDateStart, DateTime usegeDateEnd, CancellationToken cancellationToken = default)
        {
            var history = await _historyRepository.GetBonusHistoryByUsageDate(bonusId, usageDateStart, usegeDateEnd, cancellationToken);
            return !(history is null) ? _mapper.Map<List<BonusHistoryDto>>(history) : throw new ArgumentNullException("", Resources.FindError);
        }

        public async Task<IEnumerable<UserHistoryDto>> GetUserAllHistory(Guid userId, CancellationToken cancellationToken = default)
        {
            var history = await _historyRepository.GetUserHistory(userId, cancellationToken);
            return !(history is null) ? _mapper.Map<List<UserHistoryDto>>(history) : throw new ArgumentNullException("", Resources.FindError);
        }

        public async Task<IEnumerable<BonusHistoryDto>> GetBonusAllHistory(Guid bonusId, CancellationToken cancellationToken = default)
        {
            var history = await _historyRepository.GetBonusHistory(bonusId, cancellationToken);
            return !(history is null) ? _mapper.Map<List<BonusHistoryDto>>(history) : throw new ArgumentNullException("", Resources.FindError);

        }

        public async Task<int> GetCountHistoryByBonusIdAsync(Guid bonusId, CancellationToken cancellationToken = default)
        {
            return await _historyRepository.GetCountHistoryByBonusIdAsync(bonusId, cancellationToken);
        }

        public async Task<UserHistoryDto> EstimateBonus(Guid historyId, int estimate, CancellationToken cancellationToken = default)
        {
            var history =  await _historyRepository.GetByIdAsync(historyId, cancellationToken);
            if (history is null)
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }
            history.Rating = estimate;
            var bonus = await _bonusRepository.GetByIdAsync(history.BonusId, cancellationToken);
            if (bonus is null )
            {
                throw new ArgumentNullException("", Resources.IdentifierIsNull);
            }

            double avg;
            if (bonus.Rating == 0)
            {
                avg = estimate;
            }
            else
            {
                int countEst = await _historyRepository.GetCountHistoryByBonusIdAsync(bonus.Id, cancellationToken);
                avg = (bonus.Rating + estimate) / (countEst + 1);
            }

            await _bonusRepository.UpdateBonusRatingAsync(bonus.Id, avg, cancellationToken);
            await _historyRepository.UpdateAsync(historyId, history, cancellationToken);

            return _mapper.Map<UserHistoryDto>(history);
        }
    }
}
