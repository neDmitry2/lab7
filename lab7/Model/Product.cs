using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab7.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool InStock { get; set; } = true;
    }
}