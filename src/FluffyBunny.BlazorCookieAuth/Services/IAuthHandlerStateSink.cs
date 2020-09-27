using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services
{
    public interface IAuthHandlerStateSink
    {
        Task OnAuthenticatedAsync(bool authenticated);
    }
}
