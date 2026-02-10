using ECommerce.Domain;
using ECommerce.Domain.Interfaces;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace ECommerce.Tests
{
  
    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }

 
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; }
    }

   
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(int id);
    }

    
    public class OrderService
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(string customerName, decimal amount)
        {
            var order = new Order
            {
                CustomerName = customerName,
                TotalAmount = amount,
                ProductId = 1,
                Quantity = 1,
                Status = OrderStatus.Pending
            };
            return await _orderRepository.AddAsync(order);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }
    }

    // MOQ's tests
    public class OrderServiceTests
    {
        [Fact]
        public async Task CreateOrder_ShouldCallRepository()
        {
            // arrange
            var mockRepository = new Mock<IRepository<Order>>();

            mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order order) =>
                {
                    order.Id = 1;
                    return order;
                });

            var service = new OrderService(mockRepository.Object);

            // act
            var result = await service.CreateOrderAsync("fatmanur", 600);

            // assert
            Assert.NotNull(result);
            Assert.Equal("fatmanur", result.CustomerName);
            Assert.Equal(600, result.TotalAmount);
            Assert.Equal(1, result.Id);
            Assert.Equal(OrderStatus.Pending, result.Status);

            // verify
            mockRepository.Verify(
                repo => repo.AddAsync(It.IsAny<Order>()),
                Times.Once
            );
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnOrder_WhenExists()
        {
            // arrange
            var mockRepository = new Mock<IRepository<Order>>();
            var fakeOrder = new Order
            {
                Id = 1,
                CustomerName = "Test",
                TotalAmount = 100,
                ProductId = 1,
                Quantity = 1,
                Status = OrderStatus.Pending
            };

            mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(fakeOrder);

            var service = new OrderService(mockRepository.Object);

            // act
            var result = await service.GetOrderByIdAsync(1);

            // assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.CustomerName);
            Assert.Equal(100, result.TotalAmount);
            Assert.Equal(OrderStatus.Pending, result.Status);

            // verify
            mockRepository.Verify(
                repo => repo.GetByIdAsync(1),
                Times.Once
            );
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnNull_WhenNotExists()
        {
            // arrange
            var mockRepository = new Mock<IRepository<Order>>();

            mockRepository
                .Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Order)null);

            var service = new OrderService(mockRepository.Object);

            // act
            var result = await service.GetOrderByIdAsync(999);

            // assert
            Assert.Null(result);

            // verify 
            mockRepository.Verify(
                repo => repo.GetByIdAsync(999),
                Times.Once
            );
        }
    }
}