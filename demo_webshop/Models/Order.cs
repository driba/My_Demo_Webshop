using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using demo_webshop.Data;

namespace demo_webshop.Models
{
    public class Order
    {

        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateCreated { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Total { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string BillingFirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string BillingLastName { get; set; }

        [Required]
        [StringLength(100)]
        public string BillingEmail { get; set; }

        [Required]
        [StringLength(100)]
        public string BillingPhone { get; set; }

        [Required]
        [StringLength(150)]
        public string BillingAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string BillingCity { get; set; }

        [Required]
        [StringLength(50)]
        public string BillingCountry { get; set; }

        [Required]
        [StringLength(10)]
        public string BillingZip { get; set; }

        [StringLength(3000)]
        public string Message { get; set; }

    }
}
