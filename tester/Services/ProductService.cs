using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tester.Data;
using tester.DTOs.Products;
using tester.Models;
using tester.Services.Interfaces;

namespace tester.Services
{
    public class ProductService : IProductService
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> CreateProduct(CreateProductDTO productDTO) 
        {        
            var product = _mapper.Map<Product>(productDTO);        
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;
            return _mapper.Map<ProductDTO>(product);   
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProducts() 
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProduct(int id, UpdateProductDTO productDTO)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            _mapper.Map(productDTO, product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
