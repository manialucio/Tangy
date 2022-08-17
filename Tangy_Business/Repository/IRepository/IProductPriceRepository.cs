using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{

    public interface IProductPriceRepository
    {
        public Task<ProductPriceDto> Create(ProductPriceDto objDto);
        public Task<ProductPriceDto> Update(ProductPriceDto objDto);
        public Task<int> Delete(int id);

        public Task<ProductPriceDto> Get(int id);
        public Task<IEnumerable<ProductPriceDto>> GetAll(int? productID=null);

    }

}
