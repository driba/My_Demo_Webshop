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
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must have minimum length of 2 and a maximum length of 50")]
        public string BillingFirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must have minimum length of 2 and a maximum length of 50")]
        public string BillingLastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email has maximum length of 100 characters")]
        public string BillingEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(100, ErrorMessage = "Phone number has maximum length of 100 numbers")]
        public string BillingPhone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(150, ErrorMessage = "Address has maximum length of 150 characters")]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City name has a maximum length of 50 letters")]
        public string BillingCity { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country name has a maximum length of 50 letters")]
        public string BillingCountry { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(10, ErrorMessage = "Postal code has a maximum length of 50 numbers")]
        public string BillingZip { get; set; }

        [StringLength(3000, ErrorMessage = "Exceeded maximum message length (3000)")]
        public string? Message { get; set; }

        // TODO> OrderStatus (npr.: Cancel, Confirm, On Hold, Processing)

        [NotMapped]
        public ICollection<OrderItem>? Items { get; set; }
    }
}
