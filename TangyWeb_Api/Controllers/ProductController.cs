using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Tangy_Business.Repository.IRepository;
using Tangy_Common;
using Tangy_Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TangyWeb_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }


        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _productRepository.GetAll();
            return Ok(result);

        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? Id)
        {
            if ((Id ?? 0) > 0)
            {
                var result = await _productRepository.Get(Id.Value);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            return BadRequest(
                    new ErrorModelDto()
                    { Message = "Invalid product Id", StatusCode = StatusCodes.Status400BadRequest }
                    );
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDto product)
        {
            if (product == null || product.Id != 0)
            {
                return BadRequest(
                        new ErrorModelDto()
                        { Message = "Invalid product ", StatusCode = StatusCodes.Status400BadRequest }
                        );
            }
            var productUpdated = await _productRepository.Create(product);
            if (productUpdated == null)
            {
                return  this.UnprocessableEntity( 
                    new ErrorModelDto()
                    {
                        Message = " Errore durante creazione ", StatusCode = StatusCodes.Status409Conflict
                    });
            }
            return Ok(new SuccessModelDto()
            {
                Message =" Product created", Data = productUpdated
            });
        }

    // PUT api/<ProductController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ProductDto product)
    {
            if (product == null || id == 0 || id != product.Id)
            {
                return BadRequest(
                        new ErrorModelDto()
                        { Message = "Invalid product Key ", StatusCode = StatusCodes.Status400BadRequest }
                        );
            }

            var productUpdated = await _productRepository.Update(product);
            if (productUpdated == null)
            {
                return   this.UnprocessableEntity(
                    new ErrorModelDto()
                    {
                        Message = " Errore durante aggiornamento ",
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    });
            }
            return Ok( new SuccessModelDto()
            {
                Message = " Product created",
                Data = productUpdated
            });
        }

    // DELETE api/<ProductController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
            if (id == 0 )
            {
                return BadRequest(
                        new ErrorModelDto()
                        { Message = "Invalid product Key ", StatusCode = StatusCodes.Status400BadRequest }
                        );
            }

            int deletedItems = await _productRepository.Delete(id);
            if (deletedItems == 0)
            {
                  return NotFound(
                    new ErrorModelDto()
                    {
                        Message = " Id non valido prodotto non cancellato ",
                        StatusCode = StatusCodes.Status404NotFound
                    });
            }
            return Ok(new SuccessModelDto()
            {
                Message = " Product deleted",
            });

        }


}
}
