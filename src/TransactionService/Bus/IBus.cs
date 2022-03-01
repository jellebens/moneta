using System.Threading.Tasks;

namespace TransactionService.Bus
{
    public interface IBus
    {
        Task SendAsync<T>(T message);
        bool IsConnected();
    }
}
