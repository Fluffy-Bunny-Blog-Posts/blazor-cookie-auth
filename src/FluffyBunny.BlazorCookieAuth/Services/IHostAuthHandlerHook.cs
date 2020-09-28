using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services
{
    public interface IHostAuthHandlerHook  
    {
        Task OnAuthorizedExpiresInAsync(long seconds);
    }
}