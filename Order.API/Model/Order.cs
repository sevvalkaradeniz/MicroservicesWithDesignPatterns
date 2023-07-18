using System.ComponentModel.DataAnnotations;

namespace Order.API.Model
{
    public class Order
    {
        
        public int Id { get; set; }

        public DateTime CreatedDate { get; set;}

        public string BuyerId { get; set; }

        public Address Address { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public OrderStatus Status { get; set; }

        public string Message { get; set; }
    }

    public enum OrderStatus
    {
        Suspend,
        Success,
        Fail,
    }
}
