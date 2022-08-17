using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{

    public interface IProductRepository
    {
        public Task<ProductDto> Create(ProductDto objDto);
        public Task<ProductDto> Update(ProductDto objDto);
        public Task<int> Delete(int id);

        public Task<ProductDto> Get(int id);
        public Task<IEnumerable<ProductDto>> GetAll();

    }

}
