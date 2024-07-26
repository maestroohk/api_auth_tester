namespace tester.DTOs.Products
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public required string ProductType { get; set; }
        public required string ProductCategory { get; set; }
        public required string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductPrice { get; set; }
    }
}
