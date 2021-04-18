using BO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceLB.LogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FS_DB_GatewayAPI.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly MessageResult _messageResult;
        private readonly ILogService _logService;

        public ExceptionFilter(MessageResult messageResult, ILogService logService)
        {
            _messageResult = messageResult;
            _logService = logService;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            // Write log the exception

            string exceptionMsg = string.Format("Controller:{0} | Action:{1} | Exception:{2}", context.RouteData.Values["controller"],
                context.RouteData.Values["action"], context.Exception.GetBaseException().ToString());

            LogViewModel log = new LogViewModel()
            {
                AppName = "FS_DB_GatewayAPI",
                AppType = "ServiceAPI",
                Detail = exceptionMsg,
                IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                User = "System",
            };

            await _logService.AddErrorLog(log).ConfigureAwait(false);

            _messageResult.Success = false;
            _messageResult.Message = exceptionMsg;

            await Task
                .Run(() => context.Result = new ObjectResult(_messageResult)
                { StatusCode = (int)HttpStatusCode.InternalServerError })
                .ConfigureAwait(false);
        }
    }
}