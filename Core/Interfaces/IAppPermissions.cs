using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface IAppPermissions
    {
        Task CheckAuthorizationAsync(IHttpContextAccessor ctx, string appID);
        Task CheckAuthorizationAsync(HttpContext ctx, string appID);
        Task<bool> CheckValidRefreshTokenAsync(string refreshtoken, string token);
        Task<bool> SaveRefreshTokenAsync(string refreshtoken, string token);
        Task<bool> DeleteRefreshTokenAsync(string refreshtoken, string token);

    }
}
