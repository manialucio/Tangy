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
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductDto> Create(ProductDto objDto)
        {

            var Product = _mapper.Map<Product>(objDto);

            var dbobj = await _db.Products.AddAsync(Product);
            await _db.SaveChangesAsync();
            return _mapper.Map<ProductDto>(dbobj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
                _db.Products.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductDto> Get(int id)
        {
            var obj = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
                return _mapper.Map<ProductDto>(obj);
            }
            return null;
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return _mapper.Map<List<ProductDto>>(await _db.Products.ToArrayAsync());
        }



        public async Task<ProductDto> Update(ProductDto objDto)
        {
            var obj = _db.Products.FirstOrDefault(c => c.Id == objDto.Id);
            if (obj != null)
            {
                _mapper.Map<ProductDto, Product>(objDto, obj);
                _db.Products.Update(obj);
                int i = await _db.SaveChangesAsync();
                if (i > 0)
                {
                    return _mapper.Map<ProductDto>(obj);
                }
            }
            throw new Exception("errore durante il salvataggio");
        }
    }
}
