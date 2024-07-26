using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tester.DTOs.Products;
using tester.Helpers;
using tester.Models;
using tester.Services.Interfaces;

namespace tester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [PermissionAuthorize("CreateProduct")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDTO productDTO)
        {
            if (productDTO == null) return BadRequest("Product data is null.");
            var product = await _productService.CreateProduct(productDTO);
            return CreatedAtAction(nameof(CreateProduct), new { id = product.ProductId}, product);
        }

        [HttpGet("{id}")]
        [PermissionAuthorize("GetProductById")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound("Product Not Found!");
            return Ok(product);
        }

        [HttpGet]
        [PermissionAuthorize("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts() 
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpPut("{id}")]
        [PermissionAuthorize("UpdateProduct")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDTO)
        {
            if (productDTO == null) return BadRequest("Product Not Found!");
            var result = await _productService.UpdateProduct(id, productDTO);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/quantity")]
        [PermissionAuthorize("UpdateProductQuantity")]
        public async Task<IActionResult> UpdateProductQuantity (int id, [FromBody] UpdateProductQuantityDTO updateProductQuantityDTO)
        {
            var updatedProduct = await _productService.UpdateProductQuantity(id, updateProductQuantityDTO);
            if (updatedProduct == null) return NotFound();
            return Ok(updatedProduct);
        }

        [HttpPut("{id}/price")]
        [PermissionAuthorize("UpdateProductPrice")]
        public async Task<IActionResult> UpdateProductPrice (int id, [FromBody] UpdateProductPriceDTO updateProductPriceDTO)
        {
            var updatedProduct = await _productService.UpdateProductPrice(id, updateProductPriceDTO);
            if (updatedProduct == null) return NotFound();
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize("DeleteProduct")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result) return NotFound();
            return NoContent();
        }
        
    }
}
