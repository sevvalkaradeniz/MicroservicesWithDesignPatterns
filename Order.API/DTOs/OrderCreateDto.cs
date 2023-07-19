using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.DTOs
{
    
    public class OrderCreateDto 
    {
        //public int OrderId { get; set; } we do not need OrderId since OrderCreateDto related to creation of it

        public string BuyerId { get; set; } //id of the user in the website

        public List<OrderItemDto> OrderItems { get; set; }

        public PaymentDto Payment { get; set; }

        public AddressDto Address { get; set; }
    }


    //DTOs for shared library classes
    public class OrderItemDto
    {
        public int ProductId { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; } //we added this

    }
    public class PaymentDto
    {
        public string CardName { get; set; }

        public string CardNumber { get; set; }

        public string Expiration { get; set; }

        public string CVV { get; set; }

        public decimal TotalPrice { get; set; }
    }

    //from model
    public class AddressDto
    {
        public string Line { get; set; }

        public string Province { get; set; }

        public string District { get; set; }
    }
}

//A DTO is an object that defines how the data will be sent over the network
//A data transfer object (DTO) is an object that carries data between processes.
//You can use this technique to facilitate communication between two systems
//(like an API and your server) without potentially exposing sensitive information.