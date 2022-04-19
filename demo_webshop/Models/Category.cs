using System.ComponentModel.DataAnnotations;

namespace demo_webshop.Models
{
    public class Category
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

    }
}
