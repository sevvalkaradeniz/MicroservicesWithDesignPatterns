using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    //make it public
    public class StockReservedEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set;}

        //since paymentAPI service will subscribe to this event, we should send information about the payment
        public PaymentMessage Payment { get; set; }

        //need order item for failure

        public List<OrderItemMessage> OrderItem { get; set; } = new List<OrderItemMessage>();
    }
}
