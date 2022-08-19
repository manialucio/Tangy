using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models
{
    public class ProductPriceDto
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }

        public ProductDto? Product { get; set; }
        
        [Required]
        public string Size { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Price > 0!!")]
        public double Price { get; set; }

    }
}
