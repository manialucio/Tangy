using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Business.Repository.IRepository;
using Tangy_Common;
using Tangy_DataAccess;
using Tangy_DataAccess.Data;
using Tangy_DataAccess.ViewModel;
using Tangy_Models;

namespace Tangy_Business.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OrderDto> Create(OrderDto objDto)
        {
            var order = _mapper.Map<Order>(objDto);

            try
            {

            await _db.OrderHeaders.AddAsync(order.OrderHeader);
            await _db.SaveChangesAsync();
            foreach (var detail in order.OrderDetails)
            {
                detail.OrderHeaderId = order.OrderHeader.Id;

            }
            _db.OrderDetails.AddRange(order.OrderDetails);
            await _db.SaveChangesAsync();

            }
            catch( Exception ex)
            {

            }
            return new OrderDto()
            {
                OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDto>(order.OrderHeader),
                OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDto>>(order.OrderDetails).ToList()
            };

        }

        public async Task<int> Delete(int id)
        {
            var objHeader = await _db.OrderHeaders.FirstOrDefaultAsync(c => c.Id == id);
            if (objHeader != null)
            {
                IEnumerable<OrderDetail> objDetails = _db.OrderDetails.Where(o => o.OrderHeaderId == objHeader.Id);
                _db.OrderDetails.RemoveRange(objDetails);
                _db.OrderHeaders.Remove(objHeader);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<OrderDto> Get(int id)
        {
            Order order = new()
            {
                OrderHeader = await _db.OrderHeaders.FirstOrDefaultAsync(o => o.Id == id),
                OrderDetails = await _db.OrderDetails.Where(d => d.OrderHeaderId == id).ToListAsync(),
            };
            return new OrderDto()
            {
                OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDto>(order.OrderHeader),
                OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDto>>(order.OrderDetails).ToList()
            };
        }


        public async Task<IEnumerable<OrderDto>> GetAll(string? userId = null, string? status = null)
        {
            var ordersDto = new List<OrderDto>();
            IEnumerable<OrderHeader> orderHeaders = _db.OrderHeaders.Where(o => (userId == null || o.UserId == userId) && (status == null || o.Status == status));
            IEnumerable<OrderDetail> orderDetails = _db.OrderDetails;
            foreach (OrderHeader orderHeader in orderHeaders)
            {
                ordersDto.Add(new OrderDto()
                {
                    OrderHeader = _mapper.Map<OrderHeader, OrderHeaderDto>(orderHeader),
                    OrderDetails = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailDto>>(orderDetails.Where(d => d.OrderHeaderId == orderHeader.Id).ToList()).ToList()
                });
            };
            return ordersDto;
        }

        public async Task<OrderHeaderDto> MarkPaymentSuccessful(int id)
        {
            var data = await _db.OrderHeaders.FindAsync(id);
            if (data == null || data.Status != SD.OrderStatus.Pending)
            {
                return new OrderHeaderDto();
            }
            data.Status = SD.OrderStatus.Confirmed;
            await _db.SaveChangesAsync();
            return _mapper.Map<OrderHeader, OrderHeaderDto>(data);
        }

        public async Task<OrderHeaderDto> Update(OrderHeaderDto objDto)
        {
            if (objDto != null)
            {
                var orderHeader = _mapper.Map<OrderHeaderDto, OrderHeader>(objDto);
                _db.OrderHeaders.Update(orderHeader);
                await _db.SaveChangesAsync();
                return _mapper.Map<OrderHeader, OrderHeaderDto>(orderHeader);
            }
            else
            {
                return new OrderHeaderDto();
            }
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            var data = await _db.OrderHeaders.FindAsync(orderId);
            if (data == null)
            {
                return false;
            }
            data.Status = status;
            if (status == SD.OrderStatus.Shipped)
            {
                data.ShippingDate = DateTime.Now;
            }
            int updated = await _db.SaveChangesAsync();
            return updated > 0;
        }
    }
}
