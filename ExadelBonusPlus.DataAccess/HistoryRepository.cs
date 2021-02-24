using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExadelBonusPlus.Services.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ExadelBonusPlus.DataAccess
{
    public class HistoryRepository:BaseRepository<History>, IHistoryRepository
    {
        public HistoryRepository(IOptions<MongoDbSettings> mongoDbSettings) : base(mongoDbSettings)
        {
        }

        public async Task<IEnumerable<History>> GetBonusHistory(Guid bonusId, CancellationToken cancellationToken)
        {
            return await GetCollection().Find(Builders<History>.Filter.Eq("BonusId", bonusId)).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<History>> GetUserHistoryByUsageDate(Guid userId, DateTime usageDate, DateTime usageDateEnd, CancellationToken cancellationToken)
        {
            var filterUser = Builders<History>.Filter.Lte(x=>x.CreatorId, userId);
            var filterLess = Builders<History>.Filter.Lte(x => x.CreatedDate, usageDateEnd);
            var filterMore = Builders<History>.Filter.Gte(x => x.CreatedDate, usageDate);
            return await GetCollection().Find(filterLess & filterMore & filterUser).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<History>> GetUserHistory(Guid userId, CancellationToken cancellationToken)
        {
            return await GetCollection().Find(Builders<History>.Filter.Eq(x => x.CreatorId, userId)).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<History>> GetBonusHistoryByUsageDate(Guid bonusId, DateTime usageDate, DateTime usageDateEnd, CancellationToken cancellationToken = default)
        {
            var filterUser = Builders<History>.Filter.Lte(x=>x.BonusId, bonusId);
            var filterLess = Builders<History>.Filter.Lte(x => x.CreatedDate, usageDateEnd);
            var filterMore = Builders<History>.Filter.Gte(x => x.CreatedDate, usageDate);
            return await GetCollection().Find(filterLess & filterMore & filterUser).ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountHistoryByBonusIdAsync(Guid bonusId, CancellationToken cancellationToken = default)
        {
            var filterUser = Builders<History>.Filter.Eq(x => x.BonusId, bonusId);
            var result = await GetCollection().Find(filterUser).ToListAsync(cancellationToken);
            return result.Count();
        }
    }
}
