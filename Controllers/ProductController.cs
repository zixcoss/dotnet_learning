using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_learning.Data;
using dotnet_learning.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public DatabaseContext DatabaseContext { get; set;}
        public ProductController(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }

        

        [HttpGet]
        public IActionResult GetProducts()
        {
            var result = this.DatabaseContext.Products.ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var result = this.DatabaseContext.Products.Select(ProductResponse.FromProduct).FirstOrDefault(p => p.ProductId == id);
            return Ok(result);
        }
        
        
    }
}