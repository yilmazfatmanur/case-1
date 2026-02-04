using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain;
using Xunit;


namespace ECommerce.Tests
{
    public class ProductTests
    {
        [Fact]
        public void Product_ShouldHaveDefaultName()
        {
            var p = new Product();

            Assert.NotNull(p.Name);
            Assert.Equal(string.Empty ,p.Name);
        }
    }
}
