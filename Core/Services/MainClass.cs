using CommonLib;
using CoreLib.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLib.Services
{
    public class MainClass : IMainInterface
    {
        //private readonly IAppPermissions _appPermissions;
        private static IHttpContextAccessor _ctx;

        //readonly Data.Repositories.Finacle _finacle = new Data.Repositories.Finacle();

        private readonly string actor, appID;

        public MainClass(IHttpContextAccessor ctx)
        {
            // skip https certificate validation
            //_client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication()
            //{
            //    CertificateValidationMode = X509CertificateValidationMode.None,
            //    RevocationMode = X509RevocationMode.NoCheck
            //};
            _ctx = ctx;
            actor = _ctx.HttpContext.User.Identity.Name;
            appID = _ctx.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "appID").Value;

           // _appPermissions = appPermissions;
        }

    }
}
