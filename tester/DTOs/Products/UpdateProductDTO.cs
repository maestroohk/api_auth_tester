using System.ComponentModel.DataAnnotations;

namespace tester.DTOs.Products
{
    public class UpdateProductDTO
    {
        [Required]
        [MaxLength(100)]
        public required string ProductName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string ProductType { get; set; }

        [Required]
        [MaxLength(100)]
        public required string ProductCategory { get; set; }

        [Required]
        [MaxLength(255)]
        public required string ProductDescription { get; set; }
    }
}
