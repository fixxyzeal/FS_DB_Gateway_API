using BO.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLB.LogService
{
    public interface ILogService
    {
        Task AddErrorLog(LogViewModel log);
        Task AddInformationLog(LogViewModel log);
        Task DeleteLog(string id);
        Task<IEnumerable<BO.Models.Mongo.LogService>> GetLog(string type, string appName);
    }
}