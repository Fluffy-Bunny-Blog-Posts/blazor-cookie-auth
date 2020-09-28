using System.Threading.Tasks;

namespace BlazorApp1.Services
{
    public interface ISimpleService
    {
        Task PingAsync();
    }
}
