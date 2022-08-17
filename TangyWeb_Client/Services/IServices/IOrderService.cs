using Tangy_Models;

namespace TangyWeb_Client.Services.IServices
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDto>> GetAll(string? userId);
        public Task<OrderDto> Get(int id);
    }
}
