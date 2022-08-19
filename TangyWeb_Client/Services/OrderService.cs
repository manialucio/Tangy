using Newtonsoft.Json;
using Tangy_Models;
using TangyWeb_Client.Services.IServices;

namespace TangyWeb_Client.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private string BaseServerUrl;

        public OrderService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<OrderDto> Get(int orderHeaderId)
        {
            var response = await _httpClient.GetAsync($"/api/order/{orderHeaderId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var order = JsonConvert.DeserializeObject<OrderDto>(content);
                return order;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);
                throw new Exception(errorModel.Message);
            }
        }

        public async Task<IEnumerable<OrderDto>> GetAll(string? userId = null)
        {

            var response = await _httpClient.GetAsync($"/api/order/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(content);
                return orders;
            }
            return new List<OrderDto>();
        }

        public   async Task<OrderDto> Create(StripePaymentDto paymentDto)
        {
            var tempContent = JsonConvert.SerializeObject(paymentDto);
            var bodyContent = new StringContent(tempContent, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Order/Create", bodyContent);
            if (response.IsSuccessStatusCode)
            {
                var resultString =  await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OrderDto>(resultString);
                return result;
            }
            else
            {
                return new OrderDto();
            }
 
        }
    }
}
