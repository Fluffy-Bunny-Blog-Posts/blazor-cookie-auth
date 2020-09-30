using System.Threading.Tasks;

namespace BlazorApp1.Services
{
    public interface IFakeTokenFetchService
    {
        Task<TokenResponse> GetFakeBearerTokenAsync();
    }
}
