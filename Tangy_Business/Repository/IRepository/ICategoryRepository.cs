using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public Task<CategoryDto> Create(CategoryDto objDto);
        public Task<CategoryDto> Update(CategoryDto objDto);
        public Task<int> Delete(int id);

        public Task<CategoryDto> Get(int id);
        public Task<IEnumerable<CategoryDto>> GetAll();

    }
}
