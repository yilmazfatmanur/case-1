using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    public interface IInventoryService
    {
        Task<bool> ReserveStockAsync(int orderId, int productId, int quantity);
        Task<bool> ReleaseStockAsync(int orderId, int productId, int quantity);
    }
}
