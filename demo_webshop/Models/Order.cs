using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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
        public ApplicationUser? User { get; set; }
        public string? UserId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2)]
        public string BillingFirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2)]
        public string BillingLastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100)]
        public string BillingEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(100)]
        public string BillingPhone { get; set; }

        [Required(ErrorMessage = "Adress is required")]
        [StringLength(150)]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50)]
        public string BillingCity { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50)]
        public string BillingCountry { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(10)]
        public string BillingZip { get; set; }

        [StringLength(3000)]
        public string? Message { get; set; }

    }
}
