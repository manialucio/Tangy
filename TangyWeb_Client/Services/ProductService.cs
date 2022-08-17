using Newtonsoft.Json;
using Tangy_Models;
using TangyWeb_Client.Services.IServices;

namespace TangyWeb_Client.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private string BaseServerUrl;

        public ProductService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<ProductDto> Get(int productId)
        {
            var response = await _httpClient.GetAsync($"/api/product/{productId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var product = JsonConvert.DeserializeObject<ProductDto>(content);
                if (String.IsNullOrWhiteSpace(product.ImageUrl))
                {
                    product.ImageUrl = @"/images/default.png";
                }
                product.ImageUrl = BaseServerUrl + product.ImageUrl;
                return product;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content);
                throw new Exception(errorModel.Message);
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {

            var response = await _httpClient.GetAsync("/api/product");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(content);
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        if (String.IsNullOrWhiteSpace(product.ImageUrl))
                        {
                            product.ImageUrl = @"/images/default.png";
                        }
                        product.ImageUrl = BaseServerUrl + product.ImageUrl;

                    }
                }
                return products;
            }
            return new List<ProductDto>();
        }
    }
}
