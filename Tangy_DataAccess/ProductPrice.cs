using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_DataAccess
{
    public class ProductPrice
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get;set; }

        public string Size { get; set; }
        public double Price { get; set; }


    }
}
