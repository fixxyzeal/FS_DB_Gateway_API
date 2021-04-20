using BO.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceLB.IdentityService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FS_DB_GatewayAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(
           [FromBody] AuthRequestModel auth
           )
        {
            //Set IP
            if (string.IsNullOrEmpty(auth.IP))
            {
                auth.IP = Helpers.Helper.GetIP(this.HttpContext);
            }

            return Ok(await _identityService.Identity(auth).ConfigureAwait(false));
        }
    }
}