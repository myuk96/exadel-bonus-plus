using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using ExadelBonusPlus.Services.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ExadelBonusPlus.DataAccess
{
    public class TokenRefreshRepository : BaseRepository<TokenRefresh>, ITokenRefreshRepository
    {
        public TokenRefreshRepository(IOptions<MongoDbSettings> mongoDbSettings) : base(mongoDbSettings)
        {

        }
        public async Task<IEnumerable<TokenRefresh>> GetRefreshTokenByIpAddress(string ipAddress, CancellationToken cancellationToken)
        {
            return await GetCollection().Find(Builders<TokenRefresh>.Filter.Eq(x=>x.CreatedByIp, ipAddress)).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TokenRefresh>> GetRefreshTokenByToken(string token, CancellationToken cancellationToken = default)
        {
            return await GetCollection().Find(Builders<TokenRefresh>.Filter.Eq(x=>x.Token, token)).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TokenRefresh>> GetRefreshTokenByUserId(Guid userId, CancellationToken cancellationToken = default)
        {
            return await GetCollection().Find(Builders<TokenRefresh>.Filter.Eq(x=>x.CreatorId, userId)).ToListAsync(cancellationToken);
        }
    }
}
