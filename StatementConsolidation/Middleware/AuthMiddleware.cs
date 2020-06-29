using CoreLib.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Middleware
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthMiddleWareExtention
    {
        public static IApplicationBuilder UseCustomAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }

    }
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task Invoke(HttpContext ctx, IAppPermissions appPermissions)
        {

            var JWToken = ctx.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(JWToken))
            {
                try
                {
                    var TrimmedToken = JWToken.Remove(0, 7);
                    string appid = "", username = "", role="";

                    var handler = new JwtSecurityTokenHandler();

                    var tokenS = handler.ReadToken(TrimmedToken) as JwtSecurityToken;

                    appid = tokenS.Claims.First(claim => claim.Type == "appID").Value;
                    username = tokenS.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
                    role = tokenS.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

                    await appPermissions.CheckAuthorizationAsync(ctx, appid);
                }
                catch (System.Exception)
                {
                    throw new UnauthorizedAccessException();
                }

            }
            else
            {

            }
            await _next(ctx);

        }
    }


}
