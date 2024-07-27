using tester.DTOs.Products;
using tester.Models;

namespace tester.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(CreateProductDTO productDTO);
        Task<ProductDTO> GetProductById (int id);
        Task<IEnumerable<ProductDTO>> GetAllProducts();
        Task<bool> DeleteProduct(int id);
        Task<bool> UpdateProduct(int id, UpdateProductDTO productDTO);

        Task<Product> UpdateProductQuantity(int productId, UpdateProductQuantityDTO updateProductQuantityDTO);
        Task<Product> UpdateProductPrice(int productId, UpdateProductPriceDTO updateProductPriceDTO);
    }
}
