using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExadelBonusPlus.Services.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ExadelBonusPlus.DataAccess
{
    public abstract class BaseRepository<TModel> : IRepository<TModel, Guid> 
        where TModel : IEntity<Guid>
    {
        private readonly MongoDbSettings _mongoDbSettings;
        private readonly MongoClient _mongoClient;
        protected readonly IMongoDatabase _database;

        protected private BaseRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mongoDbSettings = mongoDbSettings.Value;
            _mongoClient = new MongoClient(_mongoDbSettings.ConnectionString);
            _database = _mongoClient.GetDatabase(_mongoDbSettings.DatabaseName);
        }

        public virtual Task AddAsync(TModel model, CancellationToken cancellationToken)
        {
            return GetCollection().InsertOneAsync(model, cancellationToken);
        }
        
        public virtual async Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken)
        {

            return await GetCollection().Find(Builders<TModel>.Filter.Eq(new ExpressionFieldDefinition<TModel, bool>(x => x.IsDeleted), false)).ToListAsync(cancellationToken);
        }

        public virtual Task<TModel> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return GetCollection().Find(Builders<TModel>.Filter.Eq(new ExpressionFieldDefinition<TModel, Guid>(x => x.Id), id) 
                                        & Builders<TModel>.Filter.Eq(new ExpressionFieldDefinition<TModel, bool>(x => x.IsDeleted), false)).FirstAsync(cancellationToken);
        }

        public virtual Task UpdateAsync(Guid id, TModel obj,  CancellationToken cancellationToken)
        {
            return GetCollection().ReplaceOneAsync(Builders<TModel>.Filter.Eq(new ExpressionFieldDefinition<TModel, Guid>(x => x.Id), id), obj, new ReplaceOptions(), cancellationToken);
        }

        public virtual Task<TModel> RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            return GetCollection().FindOneAndUpdateAsync(Builders<TModel>.Filter.Eq(new ExpressionFieldDefinition<TModel, Guid>(x => x.Id), id),
                Builders<TModel>.Update.Set(new ExpressionFieldDefinition<TModel, bool>(x => x.IsDeleted), true), 
                new FindOneAndUpdateOptions<TModel, TModel>(),cancellationToken);
        }
        
        protected IMongoCollection<TModel> GetCollection()
        {
            return _database.GetCollection<TModel>(typeof(TModel).Name);
        }
    }
}
