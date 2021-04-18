using AutoMapper;
using BO.StaticModels;
using BO.ViewModels;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLB.LogService
{
    public class LogService : ILogService
    {
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        private readonly IMapper _mapper;
        private readonly string LogServiceCollection = "LogService";

        public LogService(
            IMongoUnitOfWork mongoUnitOfWork,
            IMapper mapper)
        {
            _mongoUnitOfWork = mongoUnitOfWork;
            _mapper = mapper;
        }

        public async Task AddInformationLog(LogViewModel log)
        {
            //Auto map log model
            var logModel = _mapper.Map<BO.Models.Mongo.LogService>(log);

            //Set Data

            logModel.LogType = Formatter.InformationLogType;

            //Set Time UTC;
            logModel.CreatedDate = Formatter.DB_CreatedDate;

            await _mongoUnitOfWork
                .CreateAsync(LogServiceCollection, logModel)
                .ConfigureAwait(false);
        }

        public async Task AddErrorLog(LogViewModel log)
        {
            //Auto map log model
            var logModel = _mapper.Map<BO.Models.Mongo.LogService>(log);

            //Set Data

            logModel.LogType = Formatter.ErrorLogType;

            //Set Time UTC;
            logModel.CreatedDate = Formatter.DB_CreatedDate;

            await _mongoUnitOfWork
                .CreateAsync(LogServiceCollection, logModel)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<BO.Models.Mongo.LogService>> GetLog(string type, string appName)
        {
            Expression<Func<BO.Models.Mongo.LogService, bool>> match = x => true;
            //Build Query where condition

            if (!string.IsNullOrEmpty(appName) && !string.IsNullOrEmpty(type))
            {
                match = x => x.LogType == type && x.AppName == appName;
            }

            if (!string.IsNullOrEmpty(type))
            {
                match = x => x.LogType == type;
            }

            if (!string.IsNullOrEmpty(appName))
            {
                match = x => x.AppName == appName;
            }

            var result = await _mongoUnitOfWork
                .GetAllAsync(LogServiceCollection, match)
                .ConfigureAwait(false);
            return result;
        }

        public async Task DeleteLog(string id)
        {
            await _mongoUnitOfWork
                .RemoveAsync<BO.Models.Mongo.LogService>(LogServiceCollection, x => x.Id == id)
                .ConfigureAwait(false);
        }
    }
}