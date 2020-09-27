using System.Net.Http;

namespace FluffyBunny.BlazorCookieAuth.Services
{
    public interface IHostHttpClient
    {
        HttpClient CreateHttpClient();
    }
}
