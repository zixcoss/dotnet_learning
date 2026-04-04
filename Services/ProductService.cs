using dotnet_learning.Data;
using dotnet_learning.DTOs.Product;
using dotnet_learning.Entities;
using dotnet_learning.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet_learning.Services
{
    public class ProductService : IProductService
    {
        public DatabaseContext DatabaseContext { get; set;}

        public ProductService(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }
        public async Task<IEnumerable<Product>> FindAll()
        {
            return await this.DatabaseContext.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task Create(Product product)
        {
            DatabaseContext.Products.Add(product);
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task Update(Product product)
        {
            DatabaseContext.Products.Update(product);
            await DatabaseContext.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            DatabaseContext.Products.Remove(product);
            await DatabaseContext.SaveChangesAsync();
        }

        public Task<Product?> FindById(int id)
        {
            return this.DatabaseContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
            return await this.DatabaseContext.Products.Include(p => p.Category).Where(p => p.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}