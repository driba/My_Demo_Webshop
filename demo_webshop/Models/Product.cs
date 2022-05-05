using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demo_webshop.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? ImageName { get; set; }

        [NotMapped]
        public ICollection<OrderItem>? Items { get; set; }

    }
}
