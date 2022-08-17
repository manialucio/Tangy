using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Tangy_Business.Repository.IRepository;
using Tangy_Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TangyWeb_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }


        // GET: api/<OrderController>
        [HttpGet]
        public async Task<IActionResult> Get(string? userId, string? status)
        {
            var result = await _orderRepository.GetAll(userId, status);
            return Ok(result);

        }

        // GET api/<OrderController>/5
        [HttpGet("{orderHeaderId}")]
        public async Task<IActionResult> Get(int? orderHeaderId)
        {
            if ((orderHeaderId ?? 0) > 0)
            {
                var result = await _orderRepository.Get(orderHeaderId.Value);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            return BadRequest(
                    new ErrorModelDto()
                    { Message = "Order product Id", StatusCode = StatusCodes.Status400BadRequest }
                    );
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderDto order)
        {
            if (order == null ||  order.OrderHeader == null || order.OrderHeader.Id != 0)
            {
                return BadRequest(
                        new ErrorModelDto()
                        { Message = "Invalid product ", StatusCode = StatusCodes.Status400BadRequest }
                        );
            }
            var productUpdated = await _orderRepository.Create(order);
            if (productUpdated == null)
            {
                return this.UnprocessableEntity(
                    new ErrorModelDto()
                    {
                        Message = " Errore durante creazione ",
                        StatusCode = StatusCodes.Status409Conflict
                    });
            }
            return Ok(new SuccessModelDto()
            {
                Message = " Product created",
                Data = productUpdated
            });
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrderDto order)
        {
            if (order == null || order.OrderHeader == null || order.OrderHeader.Id == 0 || id != order.OrderHeader.Id)
            {
                return BadRequest(
                        new ErrorModelDto()
                        { Message = "Invalid order Key ", StatusCode = StatusCodes.Status400BadRequest }
                        );
            }

            var orderHeaderUpdated = await _orderRepository.Update(order.OrderHeader);
            if (orderHeaderUpdated == null)
            {
                return this.UnprocessableEntity(
                    new ErrorModelDto()
                    {
                        Message = " Errore durante aggiornamento ",
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    });
            }
            order.OrderHeader = orderHeaderUpdated;
            return Ok(new SuccessModelDto()
            {
                Message = " Product created",
                Data = order
            });
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest(
                        new ErrorModelDto()
                        { Message = "Invalid product Key ", StatusCode = StatusCodes.Status400BadRequest }
                        );
            }

            int deletedItems = await _orderRepository.Delete(id);
            if (deletedItems == 0)
            {
                return NotFound(
                  new ErrorModelDto()
                  {
                      Message = " Id non valido ordine non cancellato ",
                      StatusCode = StatusCodes.Status404NotFound
                  });
            }
            return Ok(new SuccessModelDto()
            {
                Message = " Order deleted",
            });

        }


    }
}
