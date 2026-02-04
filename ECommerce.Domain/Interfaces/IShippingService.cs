using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    public interface IShippingService
    {
        Task<bool> PrepareShipmentAsync(int orderId);
    }
}
