using System.ComponentModel.DataAnnotations;

namespace dotnet_learning.DTOs.Product
{
    public class ProductRequest
    {
        public int? ProductId {get; set;}
        [Required]
        [MaxLength(100, ErrorMessage = "Name, maximum length 100")]
        public string Name {set; get;} = null!;
        [Range(0,10000, ErrorMessage = "Stock, Range must between 0 and 10000")]
        public int Stock { get; set; }
        [Range(0,10000, ErrorMessage = "Price, Range must between 0 and 10000")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile>? FormFiles { get; set; }
    }
}