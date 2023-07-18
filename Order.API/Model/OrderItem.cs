using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Model
{
    public class OrderItem
    {
        
        public int Id { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName ="decimal(18,2)")] //18 digits
        public decimal Price { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; } //foreign key olarak otomatik algılıyor isimlendirmeden dolayı

        public int Count { get; set; }
    }
}
