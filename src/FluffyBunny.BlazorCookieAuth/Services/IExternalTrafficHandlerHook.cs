using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services
{
    public interface IExternalTrafficHandlerHook 
    {
        Task OnTrafficAsync();
    }
}