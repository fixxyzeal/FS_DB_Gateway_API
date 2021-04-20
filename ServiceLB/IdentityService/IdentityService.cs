using BO.Models.Mongo;
using BO.StaticModels;
using BO.ViewModels;
using DAL;
using Microsoft.IdentityModel.Tokens;
using ServiceLB.LogService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLB.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        private readonly ILogService _logService;
        private readonly string UserCollection = "UserIdentity";

        public IdentityService(
            IMongoUnitOfWork mongoUnitOfWork,
            ILogService logService)
        {
            _mongoUnitOfWork = mongoUnitOfWork;
            _logService = logService;
        }

        public async Task<AuthResultViewModel> Identity(AuthRequestModel auth)
        {
            var user = await _mongoUnitOfWork.GetAsync<UserIdentity>(UserCollection,
                x => (x.Email == auth.Email || x.UserName == auth.UserName)
                && x.Password == auth.Password)
                .ConfigureAwait(false);

            if (user == null)
            {
                // Write information log
                await _logService
               .AddInformationLog(new LogViewModel
               {
                   AppName = auth.AppName,
                   AppType = auth.AppType ?? string.Empty,
                   User = auth.UserName,
                   IP = auth.IP ?? string.Empty,
                   Detail = "Log in Failed"
               })
               .ConfigureAwait(false);

                return new AuthResultViewModel { Access_Token = string.Empty, Expires_In = 0 };
            }

            //Generate Token
            var result = GenerateJSONWebToken(user);

            //Write information log

            await _logService
                .AddInformationLog(new LogViewModel
                {
                    AppName = user.AppName,
                    AppType = auth.AppType ?? string.Empty,
                    User = user.UserName,
                    IP = auth.IP ?? string.Empty,
                    Detail = "Log in Compelete"
                })
                .ConfigureAwait(false);

            return result;
        }

        private AuthResultViewModel GenerateJSONWebToken(UserIdentity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            int minexp = 15;
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role ),
                    new Claim(ClaimTypes.Name, user.UserName ),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id)
                };

            var expires = DateTime.Now.AddMinutes(minexp);

            var jwt = new JwtSecurityToken(
              "FS",
              "FS Studio",
              claims,
              expires: expires,
              signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthResultViewModel { Access_Token = token, Expires_In = (int)TimeSpan.FromMinutes(minexp).TotalSeconds };
        }
    }
}