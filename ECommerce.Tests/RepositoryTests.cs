// RepositoryTests.cs - GÜNCEL VERSİYON
using ECommerce.Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.Tests
{
    public class RepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddProduct_ShouldWork()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            var product = new Product
            {
                Name = "Test Ürün",
                Price = 100,
                Stock = 50
            };

            // Act
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Assert
            Assert.True(product.Id > 0);
            var savedProduct = await context.Products.FindAsync(product.Id);
            Assert.NotNull(savedProduct);
            Assert.Equal("Test Ürün", savedProduct.Name);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            var product = new Product
            {
                Name = "Laptop",
                Price = 5000,
                Stock = 10
            };
            
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            var result = await context.Products.FindAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Laptop", result.Name);
            Assert.Equal(5000, result.Price);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnAll()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            context.Products.Add(new Product { Name = "Ürün 1", Price = 100, Stock = 10 });
            context.Products.Add(new Product { Name = "Ürün 2", Price = 200, Stock = 20 });
            await context.SaveChangesAsync();

            // Act
            var result = await context.Products.ToListAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateProduct_ShouldWork()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            var product = new Product
            {
                Name = "Eski İsim",
                Price = 100,
                Stock = 10
            };
            
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            product.Name = "Yeni İsim";
            product.Price = 150;
            context.Products.Update(product);
            await context.SaveChangesAsync();

            // Assert
            var updated = await context.Products.FindAsync(product.Id);
            Assert.Equal("Yeni İsim", updated.Name);
            Assert.Equal(150, updated.Price);
        }

        [Fact]
        public async Task DeleteProduct_ShouldWork()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            var product = new Product
            {
                Name = "Silinecek",
                Price = 100,
                Stock = 10
            };
            
            context.Products.Add(product);
            await context.SaveChangesAsync();

            // Act
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            // Assert
            var deleted = await context.Products.FindAsync(product.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task ProductCRUD_IntegrationTest()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            
            // CREATE
            var newProduct = new Product
            {
                Name = "Integration Test Ürün",
                Price = 250,
                Stock = 30
            };
            
            context.Products.Add(newProduct);
            await context.SaveChangesAsync();
            
            var productId = newProduct.Id;
            Assert.True(productId > 0);

            // READ
            var retrieved = await context.Products.FindAsync(productId);
            Assert.NotNull(retrieved);
            Assert.Equal("Integration Test Ürün", retrieved.Name);

            // UPDATE
            retrieved.Price = 300;
            retrieved.Stock = 25;
            await context.SaveChangesAsync();
            
            var updated = await context.Products.FindAsync(productId);
            Assert.Equal(300, updated.Price);
            Assert.Equal(25, updated.Stock);

            // DELETE
            context.Products.Remove(updated);
            await context.SaveChangesAsync();
            
            var deleted = await context.Products.FindAsync(productId);
            Assert.Null(deleted);
        }
    }
}