using System.Threading.Tasks;

namespace Moneta.Frontend.API.Bus
{
    public interface IBus
    {
        Task SendAsync<T>(string queue, string token, T message);
    }
}
