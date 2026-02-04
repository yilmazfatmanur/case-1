using ECommerce.Domain.Interfaces;

namespace ECommerce.Repository.Services
{
    public class InventoryService : IInventoryService
    {
                private static readonly Dictionary<int, int> _stock = new()
        {
            { 1, 100 }, // ProductId 1'in stoğu 100
            { 2, 50 },  // ProductId 2'nin stoğu 50
            { 3, 0 }    // ProductId 3'ün stoğu 0 (stok yok)
        };

        public Task<bool> ReserveStockAsync(int orderId, int productId, int quantity)
        {
            Console.WriteLine($"[Inventory] Stok rezerve ediliyor: OrderId={orderId}, ProductId={productId}, Quantity={quantity}");

            if (!_stock.ContainsKey(productId))
            {
                Console.WriteLine($"[Inventory] Ürün bulunamadı: ProductId={productId}");
                return Task.FromResult(false);
            }

            if (_stock[productId] >= quantity)
            {
                _stock[productId] -= quantity;
                Console.WriteLine($"[Inventory] Stok rezerve edildi. Kalan stok: {_stock[productId]}");
                return Task.FromResult(true);
            }

            Console.WriteLine($"[Inventory] Yetersiz stok! Mevcut: {_stock[productId]}, İstenen: {quantity}");
            return Task.FromResult(false);
        }

        public Task<bool> ReleaseStockAsync(int orderId, int productId, int quantity)
        {
            Console.WriteLine($"[Inventory] Stok iade ediliyor: OrderId={orderId}, ProductId={productId}, Quantity={quantity}");

            if (_stock.ContainsKey(productId))
            {
                _stock[productId] += quantity;
                Console.WriteLine($"[Inventory] Stok iade edildi. Yeni stok: {_stock[productId]}");
            }

            return Task.FromResult(true);
        }
    }
}