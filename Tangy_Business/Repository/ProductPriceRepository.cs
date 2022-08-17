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
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductPriceRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProductPriceDto> Create(ProductPriceDto objDto)
        {

            var productPrice = _mapper.Map<ProductPrice>(objDto);

            var dbobj = await _db.ProductPrices.AddAsync(productPrice);
            await _db.SaveChangesAsync();
            return _mapper.Map<ProductPriceDto>(dbobj.Entity);
        }

        public async Task<int> Delete(int id)
        {
            var obj = await _db.ProductPrices.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
                _db.ProductPrices.Remove(obj);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<ProductPriceDto> Get(int id)
        {
            var obj = await _db.ProductPrices.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
                return _mapper.Map<ProductPriceDto>(obj);
            }
            return new ProductPriceDto();
        }

        public async Task<IEnumerable<ProductPriceDto>> GetAll(int? productId = null)
        {
            IEnumerable<ProductPrice> prices = await _db.ProductPrices.Where(p => !productId.HasValue || p.ProductId == productId.Value).ToArrayAsync();
            return _mapper.Map<List<ProductPriceDto>>(prices);
        }



        public async Task<ProductPriceDto> Update(ProductPriceDto objDto)
        {
            var obj = _db.ProductPrices.FirstOrDefault(c => c.Id == objDto.Id);
            if (obj != null)
            {
                obj.Price = objDto.Price;
                obj.ProductId = objDto.ProductId;
                obj.Size = objDto.Size;

                _db.ProductPrices.Update(obj);
                int i = await _db.SaveChangesAsync();
                if (i > 0)
                {
                    return _mapper.Map<ProductPriceDto>(obj);
                }
            }
            throw new Exception("errore durante il salvataggio");
        }
    }
}
