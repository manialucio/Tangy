using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Models;

namespace Tangy_Business.Repository.IRepository
{

    public interface IOrderRepository
    {
        public Task<OrderDto> Get(int id);
        public Task<IEnumerable<OrderDto>> GetAll(string? userId = null, string? status = null);

        public Task<OrderDto> Create(OrderDto objDto);
        public Task<int> Delete(int id);

        public Task<OrderHeaderDto> Update(OrderHeaderDto objDto);
        public Task<OrderHeaderDto> MarkPaymentSuccessful(int id);
        public Task<bool> UpdateOrderStatus(int orderId, string status);


    }

}
