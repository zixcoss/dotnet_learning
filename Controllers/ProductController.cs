using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnet_learning.Data;
using dotnet_learning.DTOs.Product;
using dotnet_learning.Entities;
using dotnet_learning.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace dotnet_learning.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public IProductService ProductService { get; }

        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsAsync()
        {
            return (await this.ProductService.FindAll()).Select(ProductResponse.FromProduct).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProductByIdAsync(int id)
        {
            var product = await this.ProductService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }
            return ProductResponse.FromProduct(product);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> Search([FromQuery] string name)
        {
            var result = (await this.ProductService.Search(name))
            .Select(ProductResponse.FromProduct)
            .ToList();

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromForm] ProductRequest productRequest)
        {
            string finalImageName = "";

            if (productRequest.FormFiles != null)
            {
                (string errorMessage, string imageName) = await ProductService.UploadImage(productRequest.FormFiles);
                if (!String.IsNullOrEmpty(errorMessage))
                {
                    return BadRequest();
                }
                finalImageName = imageName;
            }

            var product = productRequest.Adapt<Product>();
            product.Image = finalImageName;
            await ProductService.Create(product);
            return StatusCode((int)HttpStatusCode.Created, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var product = await ProductService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }
            await ProductService.Delete(product);
            return NoContent();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> UpdateProductAsync(int id, [FromForm] ProductRequest productRequest)
        {
            if (id != productRequest.ProductId)
            {
                return BadRequest();
            }
            var product = await ProductService.FindById(id);
            if (product == null)
            {
                return NotFound();
            }
            // upload image

            //---------------
            productRequest.Adapt(product);
            await ProductService.Update(product);
            return Ok(ProductResponse.FromProduct(product));
        }


    }
}