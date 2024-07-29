using System.ComponentModel.DataAnnotations;

namespace tester.DTOs.Products
{
    public class UpdateProductPriceDTO
    {
        [Required]
        public int ProductPrice { get; set; }
    }
}
