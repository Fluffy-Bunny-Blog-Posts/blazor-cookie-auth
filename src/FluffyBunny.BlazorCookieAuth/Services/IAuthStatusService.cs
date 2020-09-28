using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services
{
    public interface IAuthStatusService
    {
        Task CheckAsync();
    }
}
