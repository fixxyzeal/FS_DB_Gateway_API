using BO.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLB.LogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FS_DB_GatewayAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LogServiceController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogServiceController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLog(
            [FromQuery] string appName,
            [FromQuery] string type)
        {
            return Ok(await _logService.GetLog(type, appName).ConfigureAwait(false));
        }

        [HttpPost("informationlog")]
        public async Task<IActionResult> AddInformationLog(
           [FromBody] LogViewModel log
           )
        {
            //Set IP
            if (string.IsNullOrEmpty(log.IP))
            {
                log.IP = Helpers.Helper.GetIP(this.HttpContext);
            }

            await _logService.AddInformationLog(log).ConfigureAwait(false);
            return Ok();
        }

        [HttpPost("errorlog")]
        public async Task<IActionResult> AddErrorLog(
         [FromBody] LogViewModel log
         )
        {
            //Set IP
            if (string.IsNullOrEmpty(log.IP))
            {
                log.IP = Helpers.Helper.GetIP(this.HttpContext);
            }

            await _logService.AddErrorLog(log).ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
         [FromRoute] string id
         )
        {
            await _logService.DeleteLog(id).ConfigureAwait(false);
            return Ok();
        }
    }
}