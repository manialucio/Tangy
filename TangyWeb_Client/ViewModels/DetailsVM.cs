using System.ComponentModel.DataAnnotations;
using Tangy_Models;

namespace TangyWeb_Client.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Count = 1;
            ProductPrice = new();

        }

        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Enter quantity")]
        public int Count { get; set; }
        [Required]
        public int SelectedProductPriceId { get;set; }
        public ProductPriceDto ProductPrice { get; set; }
    }
}
