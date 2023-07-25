using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    //for queue names, make class public
    public class RabbitMQSettings
    {
        public const string StockReservedEventQueueName = "stock-reserved-queue";
        public const string StockOrderCreatedEvenetQueueName = "stock-order-created-queue";
        public const string PaymentStockReservedEventQueueName = "payment-stock-reserved-queue";
    }
}
