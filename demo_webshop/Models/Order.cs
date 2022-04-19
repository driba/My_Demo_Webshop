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


    }
}
