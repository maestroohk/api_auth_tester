using System.ComponentModel.DataAnnotations;

namespace tester.DTOs.Products
{
    public class UpdateProductQuantityDTO
    {
        [Required]
        public int ProductQuantity { get; set; }   
    }
}
