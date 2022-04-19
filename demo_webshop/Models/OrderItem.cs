using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo_webshop.Models
{
    public class OrderItem
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public int OrderId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Total { get; set; }
    }
}
