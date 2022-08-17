using Tangy_Models;

namespace TangyWeb_Client.Services.IServices
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetAll();
        public Task<ProductDto> Get(int id);
    }
}
