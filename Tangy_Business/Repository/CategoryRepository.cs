using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CategoryDto> Create(CategoryDto objDto)
        {

            var  category = _mapper.Map<Category>(objDto);
            category.CreatedDate = DateTime.Now;

           var dbobj = await  _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return _mapper.Map<CategoryDto>(dbobj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
                _db.Categories.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<CategoryDto> Get(int id)
        {
            var obj = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
                return _mapper.Map<CategoryDto>(obj);
            }
            return new CategoryDto();
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            return _mapper.Map<List<CategoryDto>>( await _db.Categories.ToArrayAsync());
        }

  

        public async Task<CategoryDto> Update(CategoryDto objDto)
        {
            var obj = _db.Categories.FirstOrDefault(c => c.Id == objDto.Id);
            if (obj != null)
            {
                obj.Name = objDto.Name;
                _db.Categories.Update(obj);
                int i = await _db.SaveChangesAsync();
                if (i> 0)
                {
                    return _mapper.Map<CategoryDto>(obj);
                }
            }
            throw new Exception("errore durante il salvataggio");
        }
    }
}
