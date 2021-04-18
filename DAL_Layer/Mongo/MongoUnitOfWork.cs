using BO.Models.Mongo;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MongoUnitOfWork : IMongoUnitOfWork
    {
        private readonly IMongoDatabase _database;

        public MongoUnitOfWork(IDatabaseSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public async Task<IList<T>> GetAllAsync<T>(string collectionName)
        {
            var _model = _database.GetCollection<T>(collectionName);
            return await _model.Find(x => true).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IList<T>> GetAllAsync<T>(string collectionName, Expression<Func<T, bool>> match)
        {
            var _model = _database.GetCollection<T>(collectionName);
            return await _model.Find(match).ToListAsync().ConfigureAwait(false);
        }

        public async Task<T> GetAsync<T>(string collectionName, Expression<Func<T, bool>> match)
        {
            var _model = _database.GetCollection<T>(collectionName);
            return await _model.Find<T>(match).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<T> CreateAsync<T>(string collectionName, T model)
        {
            var _model = _database.GetCollection<T>(collectionName);
            await _model.InsertOneAsync(model).ConfigureAwait(false);
            return model;
        }

        public async Task UpdateAsync<T>(string collectionName, Expression<Func<T, bool>> match, T model)
        {
            var _model = _database.GetCollection<T>(collectionName);
            await _model.ReplaceOneAsync(match, model).ConfigureAwait(false);
        }

        public async Task RemoveAsync<T>(string collectionName, Expression<Func<T, bool>> match)
        {
            var _model = _database.GetCollection<T>(collectionName);
            await _model.DeleteOneAsync(match).ConfigureAwait(false);
        }
    }
}