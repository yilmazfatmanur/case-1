using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(int orderId, decimal amount);
        Task<bool> RefundPaymentAsync(int orderId, decimal amount);
    }
}
