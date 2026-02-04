namespace ECommerce.Domain.Interfaces
{
    public interface IShippingService
    {
        Task<bool> PrepareShipmentAsync(int orderId);
        Task<bool> CancelShipmentAsync(int orderId);
    }
}