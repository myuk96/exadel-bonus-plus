using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExadelBonusPlus.Services.Models
{
    public interface IHistoryRepository : IRepository<History, Guid>
    {
        public Task<IEnumerable<History>> GetUserHistory(Guid userId, CancellationToken cancellationToken = default);
        public Task<IEnumerable<History>> GetBonusHistory(Guid bonusId, CancellationToken cancellationToken = default);

        public Task<IEnumerable<History>> GetUserHistoryByUsageDate(Guid userId, DateTime usageDate, DateTime usageDateEnd,
            CancellationToken cancellationToken = default);
        public Task<IEnumerable<History>> GetBonusHistoryByUsageDate(Guid bonusId, DateTime usageDate, DateTime usageDateEnd,
            CancellationToken cancellationToken = default);
        public Task<int> GetCountHistoryByBonusIdAsync(Guid bonusId, CancellationToken cancellationToken = default);
        public Task<IEnumerable<History>> GetEstmatatedHistoryByBonusIdAsync(Guid bonusId, CancellationToken cancellationToken = default);

    }
}
