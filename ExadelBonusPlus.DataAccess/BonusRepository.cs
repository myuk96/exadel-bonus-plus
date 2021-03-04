using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using ExadelBonusPlus.Services;
using ExadelBonusPlus.Services.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace ExadelBonusPlus.DataAccess
{
    public class BonusRepository : BaseRepository<Bonus>, IBonusRepository
    {
        public BonusRepository(IOptions<MongoDbSettings> mongoDbSettings) : base(mongoDbSettings)
        {
        }
        public async Task<IEnumerable<Bonus>> GetBonusesAsync(BonusFilter bonusFilter, CancellationToken cancellationToken)
        {
            FilterDefinition<Bonus> filter =
                Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, bool>(x => x.IsDeleted), false);
            List<SortDefinition<Bonus>> sortBy = new List<SortDefinition<Bonus>>();

            if(bonusFilter?.IsActive != null)
            {
                filter &= Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, bool>(x => x.IsActive),
                    (bool)bonusFilter?.IsActive);
            }

            if (!String.IsNullOrEmpty(bonusFilter?.Title))
            {
                filter &= Builders<Bonus>.Filter.Regex(new ExpressionFieldDefinition<Bonus, string>(x => x.Title), new BsonRegularExpression(bonusFilter?.Title));
            }

            if (!String.IsNullOrEmpty(bonusFilter?.Type))
            {
                filter &= Builders<Bonus>.Filter.Eq(x => x.Type, bonusFilter.Type);
            }

            if (!String.IsNullOrEmpty(bonusFilter?.City))
            {
                filter &= Builders<Bonus>.Filter.ElemMatch(x => x.Locations, y => y.City.ToLower() == bonusFilter.City.ToLower());
            }

            if (bonusFilter?.CompanyId != null & bonusFilter?.CompanyId != Guid.Empty)
            {
                filter &= Builders<Bonus>.Filter.Eq(x => x.CompanyId, bonusFilter.CompanyId);
            }

            if (bonusFilter?.Tags != null && bonusFilter?.Tags.Count > 0)
            {
                filter &= Builders<Bonus>.Filter.AnyIn(b => b.Tags, bonusFilter?.Tags);
            }

            if (bonusFilter?.DateStart != null && bonusFilter?.DateStart != DateTime.MinValue)
            {
                filter &= Builders<Bonus>.Filter.Gte(x => x.DateStart, bonusFilter?.DateStart) |
                      Builders<Bonus>.Filter.Gte(x => x.DateEnd, bonusFilter?.DateStart);
            }

            if (bonusFilter?.DateEnd != null && bonusFilter?.DateEnd != DateTime.MinValue)
            {
                filter &= Builders<Bonus>.Filter.Lte(x => x.DateStart, bonusFilter?.DateEnd) |
                         Builders<Bonus>.Filter.Lte(x => x.DateEnd, bonusFilter?.DateEnd);
            }

            if ((bonusFilter?.LastCount ?? 0) != 0)
            {
                sortBy.Add(Builders<Bonus>.Sort.Descending("CreatedDate"));
            }
            sortBy.Add(Builders<Bonus>.Sort.Ascending(bonusFilter?.SortBy ?? "Title"));

            return await GetCollection().Find(filter).Sort(Builders<Bonus>.Sort.Combine(sortBy)).Limit(bonusFilter?.LastCount ?? 0).ToListAsync(cancellationToken);
        }

        public Task<Bonus> ActivateBonusAsync(Guid id,  CancellationToken cancellationToken)
        {
            return GetCollection().FindOneAndUpdateAsync(Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, Guid>(x => x.Id), id),
                                                         Builders<Bonus>.Update.Set(new ExpressionFieldDefinition<Bonus, bool>(x => x.IsActive), true),
                new FindOneAndUpdateOptions<Bonus, Bonus>(){ ReturnDocument = ReturnDocument.After }, cancellationToken);
        }

        public Task<Bonus> UpdateBonusRatingAsync(Guid id, double rating, CancellationToken cancellationToken)
        {
            return GetCollection().FindOneAndUpdateAsync(Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, Guid>(x => x.Id), id),
                Builders<Bonus>.Update.Set(new ExpressionFieldDefinition<Bonus, double>(x => x.Rating), rating),
                new FindOneAndUpdateOptions<Bonus, Bonus>() { ReturnDocument = ReturnDocument.After }, cancellationToken);
        }

        public Task<Bonus> DeactivateBonusAsync(Guid id, CancellationToken cancellationToken)
        {
            return GetCollection().FindOneAndUpdateAsync(Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, Guid>(x => x.Id), id),
                Builders<Bonus>.Update.Set(new ExpressionFieldDefinition<Bonus, bool>(x => x.IsActive), false),
                new FindOneAndUpdateOptions<Bonus, Bonus>() { ReturnDocument = ReturnDocument.After }, cancellationToken);
        }

        public async Task<IEnumerable<string>> GetBonusTagsAsync(CancellationToken cancellationToken)
        {
            FieldDefinition<Bonus, string> field = "Tags";
            return await GetCollection().Distinct<string>(field, Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, bool>(x => x.IsDeleted), false)
                                                                  & Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, bool>(x => x.IsActive), true)).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> GetCitiesAsync(CancellationToken cancellationToken)
        {
            FieldDefinition<Bonus, string> field = "Locations.City";
            return await GetCollection().Distinct<string>(field, Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, bool>(x => x.IsDeleted), false)
                                                                  & Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, bool>(x => x.IsActive), true)).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Bonus>> GetBonusStatisticAsync(BonusFilter bonusFilter, CancellationToken cancellationToken)
        {
            FilterDefinition<Bonus> filter =
                Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, bool>(x => x.IsDeleted), false);
            List<SortDefinition<Bonus>> sortBy = new List<SortDefinition<Bonus>>();

            if (bonusFilter?.IsActive != null)
            {
                filter &= Builders<Bonus>.Filter.Eq(new ExpressionFieldDefinition<Bonus, bool>(x => x.IsActive),
                    (bool)bonusFilter?.IsActive);
            }

            if (!String.IsNullOrEmpty(bonusFilter?.Title))
            {
                filter &= Builders<Bonus>.Filter.Regex(new ExpressionFieldDefinition<Bonus, string>(x => x.Title), new BsonRegularExpression(bonusFilter?.Title));
            }

            if (!String.IsNullOrEmpty(bonusFilter?.Type))
            {
                filter &= Builders<Bonus>.Filter.Eq(x => x.Type, bonusFilter.Type);
            }

            if (bonusFilter?.CompanyId != null & bonusFilter?.CompanyId != Guid.Empty)
            {
                filter &= Builders<Bonus>.Filter.Eq(x => x.CompanyId, bonusFilter.CompanyId);
            }

            if (!String.IsNullOrEmpty(bonusFilter?.City))
            {
                filter &= Builders<Bonus>.Filter.ElemMatch(x => x.Locations, y => y.City.ToLower() == bonusFilter.City.ToLower());
            }

            if (bonusFilter?.Tags != null && bonusFilter?.Tags.Count > 0)
            {
                filter &= Builders<Bonus>.Filter.AnyIn(b => b.Tags, bonusFilter?.Tags);
            }

            if (bonusFilter?.DateStart != null && bonusFilter?.DateStart != DateTime.MinValue)
            {
                filter &= Builders<Bonus>.Filter.Gte(x => x.DateStart, bonusFilter?.DateStart) |
                      Builders<Bonus>.Filter.Gte(x => x.DateEnd, bonusFilter?.DateStart);
            }

            if (bonusFilter?.DateEnd != null && bonusFilter?.DateEnd != DateTime.MinValue)
            {
                filter &= Builders<Bonus>.Filter.Lte(x => x.DateStart, bonusFilter?.DateEnd) |
                         Builders<Bonus>.Filter.Lte(x => x.DateEnd, bonusFilter?.DateEnd);
            }

            if ((bonusFilter?.LastCount ?? 0) != 0)
            {
                sortBy.Add(Builders<Bonus>.Sort.Descending("CreatedDate"));
            }
            sortBy.Add(Builders<Bonus>.Sort.Ascending(bonusFilter?.SortBy ?? "Title"));

            return await GetCollection().Find(filter).Sort(Builders<Bonus>.Sort.Combine(sortBy)).Limit(bonusFilter?.LastCount ?? 0).ToListAsync(cancellationToken);
        }
    }
}