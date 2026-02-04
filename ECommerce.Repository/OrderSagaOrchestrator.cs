using ECommerce.Domain;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace ECommerce.Repository
{
    public class OrderSagaOrchestrator
    {
        private readonly AppDbContext _context;
        private readonly IInventoryService _inventoryService;
        private readonly IPaymentService _paymentService;
        private readonly IShippingService _shippingService;

        public OrderSagaOrchestrator(
            AppDbContext context,
            IInventoryService inventoryService,
            IPaymentService paymentService,
            IShippingService shippingService)
        {
            _context = context;
            _inventoryService = inventoryService;
            _paymentService = paymentService;
            _shippingService = shippingService;
        }

        public async Task<OrderSaga> ProcessOrderAsync(int orderId)
        {
            // Saga oluştur
            var saga = new OrderSaga
            {
                OrderId = orderId,
                CurrentState = SagaState.OrderCreated,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.OrderSagas.Add(saga);
            await _context.SaveChangesAsync();

            // Saga'yı çalıştır
            await ExecuteSagaAsync(saga);

            return saga;
        }

        private async Task ExecuteSagaAsync(OrderSaga saga)
        {
            var order = await _context.Orders.FindAsync(saga.OrderId);

            if (order == null)
            {
                saga.CurrentState = SagaState.Cancelled;
                saga.ErrorMessage = "Sipariş bulunamadı";
                saga.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return;
            }

            // Adım 1: Stok Rezervasyonu
            saga.CurrentState = SagaState.InventoryReserving;
            saga.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            var inventoryResult = await _inventoryService.ReserveStockAsync(
                saga.OrderId, order.ProductId, order.Quantity);

            if (!inventoryResult)
            {
                saga.CurrentState = SagaState.InventoryReservationFailed;
                saga.ErrorMessage = "Stok yetersiz";
                saga.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync(); // önce Failed kaydet

                await CompensateAsync(saga, SagaState.InventoryReserving);
                return;
            }

            saga.CurrentState = SagaState.InventoryReserved;
            saga.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            // Adım 2: Ödeme İşlemi
            saga.CurrentState = SagaState.PaymentProcessing;
            saga.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            var paymentResult = await _paymentService.ProcessPaymentAsync(
                saga.OrderId, order.TotalAmount);

            if (!paymentResult)
            {
                saga.CurrentState = SagaState.PaymentFailed;
                saga.ErrorMessage = "Ödeme başarısız";
                saga.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                await CompensateAsync(saga, SagaState.PaymentProcessing);
                return;
            }

            saga.CurrentState = SagaState.PaymentProcessed;
            saga.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            // Adım 3: Kargo Hazırlığı
            saga.CurrentState = SagaState.ShippingPreparing;
            saga.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            var shippingResult = await _shippingService.PrepareShipmentAsync(saga.OrderId);

            if (!shippingResult)
            {
                saga.CurrentState = SagaState.ShippingFailed;
                saga.ErrorMessage = "Kargo hazırlığı başarısız";
                saga.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                await CompensateAsync(saga, SagaState.ShippingPreparing);
                return;
            }

            // Başarılı!
            saga.CurrentState = SagaState.Completed;
            saga.ErrorMessage = null;
            saga.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        private async Task CompensateAsync(OrderSaga saga, SagaState failedState)
        {
            var order = await _context.Orders.FindAsync(saga.OrderId);

            // ✅ ORDER NULL KONTROLÜ (EKLENEN KISIM)
            if (order == null)
            {
                saga.CurrentState = SagaState.Cancelled;
                saga.ErrorMessage = "Compensation sırasında sipariş bulunamadı";
                saga.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return;
            }

            // Geri alma işlemleri
            switch (failedState)
            {
                case SagaState.ShippingPreparing:
                    // Kargo başarısız -> Ödeme iade et + stok iade et
                    await _paymentService.RefundPaymentAsync(saga.OrderId, order.TotalAmount);
                    await _inventoryService.ReleaseStockAsync(saga.OrderId, order.ProductId, order.Quantity);
                    break;

                case SagaState.PaymentProcessing:
                    // Ödeme başarısız -> Stok iade et
                    await _inventoryService.ReleaseStockAsync(saga.OrderId, order.ProductId, order.Quantity);
                    break;

                case SagaState.InventoryReserving:
                    // Stok rezervasyonu başarısız -> Yapılacak bir şey yok
                    break;
            }

            saga.CurrentState = SagaState.Cancelled;
            saga.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
