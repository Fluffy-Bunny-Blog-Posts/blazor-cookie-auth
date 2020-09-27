using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services
{
    public interface IAuthPingService
    {
        Task PingAsync();
    }
}
