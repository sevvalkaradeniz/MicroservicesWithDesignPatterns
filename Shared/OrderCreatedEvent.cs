using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class OrderCreatedEvent //class should be public
    {
        //OrderCreatedEvent should send credit card information to the Payment microservice so we need to create another class for credit card information
        //We need order item message -> id of the product and amount
        public int OrderId { get; set; } //to detect the order

        public string BuyerId { get; set; } //id of the user in the website

        public PaymentMessage Payment { get; set; } //class that stores credit card information of the user

        public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>(); //since user can buy multiple product
    }
}
