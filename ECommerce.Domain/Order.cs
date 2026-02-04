using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerce.Domain
{
    public class Order
    {
        public int Id { get; set; }

        public int ProductId { get; set; }   
        public int Quantity { get; set; }    

        public decimal TotalAmount { get; set; } 
        public OrderStatus Status { get; set; }

        public string CustomerName { get; set; } = string.Empty;

    }
}
