using BO.Models.Mongo;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL
{
    public interface IMongoUnitOfWork
    {
        Task<T> CreateAsync<T>(string collectionName, T model);

        Task<IList<T>> GetAllAsync<T>(string collectionName);

        Task<IList<T>> GetAllAsync<T>(string collectionName, Expression<Func<T, bool>> match);

        Task<T> GetAsync<T>(string collectionName, Expression<Func<T, bool>> match);

        Task RemoveAsync<T>(string collectionName, Expression<Func<T, bool>> match);

        Task UpdateAsync<T>(string collectionName, Expression<Func<T, bool>> match, T model);
    }
}