using dotnet_project.Database;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_project.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    public DatabaseContext DbContext { get; set; }
    public ProductController(DatabaseContext dbContext)
    {
      this.DbContext = dbContext;
    }

    [HttpGet("")]
    public IActionResult GetProducts()
    {
      var result = this.DbContext.Products.ToList();
      return Ok(result);
    }


  }
}