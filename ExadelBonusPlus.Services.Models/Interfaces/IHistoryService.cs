using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExadelBonusPlus.Services.Models
{
    public interface IHistoryService
    {
        Task<HistoryDto> AddHistory(AddHistoryDTO model, CancellationToken cancellationToken = default);
        Task<HistoryDto> DeleteHistory(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<HistoryDto>> GetAllHistory(CancellationToken cancellationToken = default);
        Task<HistoryDto> GetHistoryById(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserHistoryDto>> GetUserHistoryByUsageDate(Guid userId ,DateTime usageDateStart, DateTime usegeDateEnd, CancellationToken cancellationToken = default);
        Task<IEnumerable<BonusHistoryDto>> GetBonusHistoryByUsageDate(Guid vendorId ,DateTime usageDateStart, DateTime usegeDateEnd, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserHistoryDto>> GetUserAllHistory(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserHistoryDto>> GetUserAllHistoryWithousRepetiotins(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<BonusHistoryDto>> GetBonusAllHistory(Guid bonusId, CancellationToken cancellationToken = default);
        Task<int> GetCountHistoryByBonusIdAsync(Guid bonusId, CancellationToken cancellationToken = default);
        Task<UserHistoryDto> EstimateBonus(Guid historyId, int estimate, CancellationToken cancellationToken = default);

    }
}
