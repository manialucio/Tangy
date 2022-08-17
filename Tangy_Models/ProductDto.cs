using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public bool ShopFavorites { get; set; } = false;
        public bool CustomerFavorites { get; set; } = false;
        public string Color { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage ="Please select  a category")]
        public int CategoryId { get; set; }
        public CategoryDto? Category { get; set; }

        public List<ProductPriceDto>? Prices { get; set; }
        public bool IsDefaultImage => ImageUrl.ToLower().Contains("default");
    }
}
