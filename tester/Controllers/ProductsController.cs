using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tester.DTOs.Products;
using tester.Models;
using tester.Services.Interfaces;

namespace tester.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDTO productDTO)
        {
            if (productDTO == null) return BadRequest("Product data is null.");
            var product = await _productService.CreateProduct(productDTO);
            return CreatedAtAction(nameof(CreateProduct), new { id = product.ProductId}, product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound("Product Not Found!");
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts() 
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDTO)
        {
            if (productDTO == null) return BadRequest("Product Not Found!");
            var result = await _productService.UpdateProduct(id, productDTO);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
