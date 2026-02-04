using Xunit;
using ECommerce.Domain;

namespace ECommerce.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }
        
        [Fact]
        public void Product_ShouldInitializeCorrectly()
        {
            var product = new Product();
            Assert.NotNull(product);
        }
    }
}