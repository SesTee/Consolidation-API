using CommonLib;
using LoggerClassLib.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Services
{
    public class UserClass : IUserInterface
    {
        private readonly IAppPermissions _appPermissions;
        private static IHttpContextAccessor _ctx;

        static readonly string jwtkey = CommonLib.Utility.AppConfiguration().GetSection("Keys").GetSection("jwt").GetSection("key").Value;
        static readonly string jwtissuer = CommonLib.Utility.AppConfiguration().GetSection("Keys").GetSection("jwt").GetSection("issuer").Value;
        static readonly string AgeApp = CommonLib.Utility.AppConfiguration().GetSection("Keys").GetSection("jwt").GetSection("TokenAge").GetSection("App").Value;
        static readonly string AgeUser = CommonLib.Utility.AppConfiguration().GetSection("Keys").GetSection("jwt").GetSection("TokenAge").GetSection("User").Value;

        static readonly string appUsers = CommonLib.Utility.AppConfiguration().GetSection("Resources").GetSection("appUsers").Value;

        private readonly string actor, appID;

        public UserClass(IAppPermissions appPermissions, IHttpContextAccessor ctx)
        {
            // skip https certificate validation
            //_client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication()
            //{
            //    CertificateValidationMode = X509CertificateValidationMode.None,
            //    RevocationMode = X509RevocationMode.NoCheck
            //};
            _ctx = ctx;
            actor = _ctx.HttpContext.User.Identity.Name;

            var clam = _ctx.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "appID");

            appID = (clam is null) ? "" : clam.Value;

            _appPermissions = appPermissions;
        }


        public async Task<LoginInfo> LoginAsync(UserLogin loginmain, string appID, IHttpContextAccessor ctx)
        {
            //return await GetTokenAsync(new LoginInfo { UserName = loginmain.UserName }, appID);

            await _appPermissions.CheckAuthorizationAsync(ctx, appID);
            LoggerClassLib.ActivityLog<string>.Logger("Login Request", Newtonsoft.Json.JsonConvert.SerializeObject(loginmain.UserName), appID);
            
            if (loginInfo == null)
                throw new BadRequestException(new GeneralResponse { code = "400", message = "User does not have access to this app" });

            loginInfo = await GetTokenAsync(loginInfo, appID);

            LoggerClassLib.ActivityLog<string>.Logger("Login Response", Newtonsoft.Json.JsonConvert.SerializeObject(loginInfo), appID);
            return loginInfo;
        }

        public async Task<TokenRefresh> RefreshTokenAsync(RefreshToken refreshtoken)
        {
            LoginInfo loginInfo = new LoginInfo();

            loginInfo.UserName = _ctx.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;
            string rolestr = _ctx.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;
            string oldtoken = _ctx.HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7);

            LoggerClassLib.ActivityLog<string>.Logger("RefreshToken Request", JsonConvert.SerializeObject(refreshtoken));
            LoggerClassLib.ActivityLog<string>.Logger("oldtoken", oldtoken);


            loginInfo.RoleMap = JsonConvert.DeserializeObject<List<RoleMap>>(rolestr);
            var resp = await _appPermissions.CheckValidRefreshTokenAsync(refreshtoken.token, oldtoken);
            if (!resp)
                throw new BadRequestException(new GeneralResponse { code = "400", message = "Invalid RefreshToken/Token" });

            loginInfo = await GetTokenAsync(loginInfo, appID);

            await _appPermissions.DeleteRefreshTokenAsync(refreshtoken.token, oldtoken);


            TokenRefresh tokenRefresh = new TokenRefresh { Token = loginInfo.Token, RefreshToken = loginInfo.RefreshToken };

            LoggerClassLib.ActivityLog<string>.Logger("RefreshToken Response", JsonConvert.SerializeObject(tokenRefresh), appID);
            return tokenRefresh;
        }

        private async Task<LoginInfo> GetTokenAsync(LoginInfo loginInfo, string AppID)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expirationtime = appUsers.Contains(loginInfo.UserName) ? DateTime.UtcNow.AddYears(Convert.ToInt32(AgeApp)) : DateTime.UtcNow.AddMinutes(Convert.ToInt32(AgeUser));
            var claims = new[] {
                new Claim(ClaimTypes.Name, loginInfo.UserName),
                new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(loginInfo.RoleMap)),
                new Claim("appID", AppID)
            };

            var token = new JwtSecurityToken(issuer: jwtissuer,
              audience: jwtissuer,
              claims: claims,
              notBefore: DateTime.UtcNow,
              expires: expirationtime,
              signingCredentials: credentials);

            loginInfo.Token = new JwtSecurityTokenHandler().WriteToken(token);
            loginInfo.RefreshToken = GenerateRefreshToken();
            await _appPermissions.SaveRefreshTokenAsync(loginInfo.RefreshToken, loginInfo.Token);
            return loginInfo;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
