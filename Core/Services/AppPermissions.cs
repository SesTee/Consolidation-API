using CommonLib;

using CoreLib.Interfaces;

using DomainClassLib.Data.Contexts;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLib.Services
{
    public class AppPermissions : IAppPermissions
    {
        private readonly AppPermsContext _context;

        public AppPermissions(AppPermsContext context)
        {
            _context = context;
        }

        public async Task CheckAuthorizationAsync(IHttpContextAccessor ctx, string appID)
        {
            var securityCheckOn = bool.Parse(Utility.AppConfiguration().GetSection("PermittedAppsAndIPs").GetSection("SecurityCheckOn").Value);
            if (securityCheckOn)
            {
                var controller = ctx.HttpContext.Request.Path.Value.Split('/')[2].ToLower();
                var method = ctx.HttpContext.Request.Path.Value.Split('/')[3].ToLower();

                var result0 = await _context.AppPermissions.Where(x => x.AppID == appID).FirstOrDefaultAsync();

                if (result0 == null)
                {
                    LoggerClassLib.ActivityLog<string>.Logger("UnauthorizedAccessException", "AppID = " + appID, "");
                    throw new UnauthorizedAccessException();
                }

                if (result0.AppServerIP != "*")
                {
                    var result = await _context.AppPermissions.Where(x => x.AppID == appID && x.AppServerIP == ctx.HttpContext.Connection.RemoteIpAddress.ToString())
                    .FirstOrDefaultAsync();

                    if (result == null)
                    {
                        LoggerClassLib.ActivityLog<string>.Logger("UnauthorizedAccessException", "AppID = " + appID, "");
                        throw new UnauthorizedAccessException();
                    }

                    if (result.AccessLevel != "*")
                    {
                        var result1 = await _context.ControllerAccess.Where(x => x.AppID == appID && x.ControllerName.ToLower() == controller).FirstOrDefaultAsync();
                        if (result1 == null)
                        {
                            throw new UnauthorizedAccessException();
                        }

                        if (result1.AccessLevel != "*")
                        {
                            var result2 = await _context.MethodAccess.Where(x => x.AppID == appID && x.ControllerName.ToLower() == controller && x.MethodName.ToLower() == method).FirstOrDefaultAsync();
                            if (result2 == null)
                            {
                                throw new UnauthorizedAccessException();
                            }
                        }

                    }
                }
            }
        }
        public async Task CheckAuthorizationAsync(HttpContext ctx, string appID)
        {
            //throw new UnauthorizedAccessException();
            var securityCheckOn = bool.Parse(Utility.AppConfiguration().GetSection("PermittedAppsAndIPs").GetSection("SecurityCheckOn").Value);
            if (securityCheckOn)
            {
                var controller = ctx.Request.Path.Value.Split('/')[2].ToLower();
                var method = ctx.Request.Path.Value.Split('/')[3].ToLower();

                var result0 = await _context.AppPermissions.Where(x => x.AppID == appID).FirstOrDefaultAsync();

                if (result0 == null)
                {
                    LoggerClassLib.ActivityLog<string>.Logger("UnauthorizedAccessException", "AppID = " + appID, "");
                    throw new UnauthorizedAccessException();
                }

                if (result0.AppServerIP != "*")
                {
                    var result = await _context.AppPermissions.Where(x => x.AppID == appID && x.AppServerIP == ctx.Connection.RemoteIpAddress.ToString())
                    .FirstOrDefaultAsync();

                    if (result == null)
                    {
                        LoggerClassLib.ActivityLog<string>.Logger("UnauthorizedAccessException", "AppID = " + appID, "");
                        throw new UnauthorizedAccessException();
                    }

                    if (result.AccessLevel != "*")
                    {
                        var result1 = await _context.ControllerAccess.Where(x => x.AppID == appID && x.ControllerName.ToLower() == controller).FirstOrDefaultAsync();
                        if (result1 == null)
                        {
                            throw new UnauthorizedAccessException();
                        }

                        if (result1.AccessLevel != "*")
                        {
                            var result2 = await _context.MethodAccess.Where(x => x.AppID == appID && x.ControllerName.ToLower() == controller && x.MethodName.ToLower() == method).FirstOrDefaultAsync();
                            if (result2 == null)
                            {
                                throw new UnauthorizedAccessException();
                            }
                        }

                    }
                }


            }
        }
        public async Task<bool> CheckValidRefreshTokenAsync(string refreshtoken, string token)
        {
            var result1 = await _context.TokenTbl.Where(x => x.Refresh == refreshtoken && x.Token == token).FirstOrDefaultAsync();

            if (result1 == null)
            {
                return false;
            }
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> SaveRefreshTokenAsync(string refreshtoken, string token)
        {
            try
            {
                var result1 = await _context.TokenTbl.AddAsync(new DomainClassLib.Data.Entities.AppPerm.TokenTbl { Refresh = refreshtoken, Token = token, DateCreated = DateTime.Now });
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> DeleteRefreshTokenAsync(string refreshtoken, string token)
        {
            try
            {
                var result1 = await _context.TokenTbl.Where(x => x.Refresh == refreshtoken && x.Token == token).FirstOrDefaultAsync();
                var result2 = _context.TokenTbl.Remove(result1);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}


