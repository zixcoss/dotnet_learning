namespace dotnet_learning.DTOs.Product
{
    using dotnet_learning.Entities;
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public string? Image { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }

        public static ProductResponse FromProduct(Product product)
        {
            return new ProductResponse
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Image = product.Image,
                Stock = product.Stock,
                Price = product.Price,
            };
        }
    }
}