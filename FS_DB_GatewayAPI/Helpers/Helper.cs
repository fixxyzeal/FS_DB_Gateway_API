using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FS_DB_GatewayAPI.Helpers
{
    public static class Helper
    {
        public static string GetIP(HttpContext httpContext)
        {
            string ip = string.Empty;

            if (httpContext.Request.Headers != null)
            {
                //the X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a client
                //connecting to a web server through an HTTP proxy or load balancer

                var forwardedHeader = httpContext.Request.Headers["X-Forwarded-For"];
                if (!string.IsNullOrEmpty(forwardedHeader))
                    ip = forwardedHeader.FirstOrDefault();
            }

            //if this header not exists try get connection remote IP address
            if (string.IsNullOrEmpty(ip) && httpContext.Connection.RemoteIpAddress != null)
                ip = httpContext.Connection.RemoteIpAddress.ToString();

            return ip;
        }
    }
}