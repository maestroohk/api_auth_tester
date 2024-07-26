using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tester.Models
{
    public class Product
    {
        [Key]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("product_name")]
        public required string ProductName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("product_type")]
        public required string ProductType { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("product_category")]
        public required string ProductCategory { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("product_description")]
        public required string ProductDescription { get; set; }

        [Required]
        [Column("product_quantity")]
        public int ProductQuantity { get; set; }

        [Required]
        [Column("product_price")]
        public int ProductPrice { get; set; }
    }
}
