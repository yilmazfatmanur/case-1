using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain
{
    public enum OrderStatus
    {
        OrderCreated,
        StockReserved,
        PaymentCompleted,
        ShipmentPrepared,
        OrderCompleted,
        Cancelled
    }
}
