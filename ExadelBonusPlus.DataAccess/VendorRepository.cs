using ExadelBonusPlus.Services.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Bson;

namespace ExadelBonusPlus.DataAccess
{
    public class VendorRepository : BaseRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(IOptions<MongoDbSettings> mongoDbSettings) : base(mongoDbSettings)
        {

        }
        public async Task<List<Vendor>> SearchVendorByNameAsync(string name, CancellationToken cancellationToken)
        {
            var filter = Builders<Vendor>.Filter.Regex(
                new ExpressionFieldDefinition<Vendor, string>(x => x.Name),
                new BsonRegularExpression($"^{name}", "i"));

            return await GetCollection().Find(filter).ToListAsync(cancellationToken);
        }

    }
}
