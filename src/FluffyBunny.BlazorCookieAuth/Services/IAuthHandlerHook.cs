using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services
{
    public interface IAuthHandlerHook
    {
        Task OnAuthorizedCallAsync(long seconds);
    }
}