using Blazored.LocalStorage;
using TangyWeb_Client.Services.IServices;
using TangyWeb_Client.ViewModels;
using Tangy_Common;

namespace TangyWeb_Client.Services
{
    public class CartService : ICartService
    {
        public readonly ILocalStorageService _localStorageService;
        public readonly IProductService _productService;

        public event Action OnChange;

        public CartService(ILocalStorageService localStorageService, IProductService productService)
        {
            _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
            _productService = productService;
        }

        public async Task DecrementCart(ShoppingCart cartItem)
        {
            var cart = await _localStorageService.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            if (cart == null)
            {
                cart = new List<ShoppingCart>();

            }
            var foundedInCart = cart.Where(c => c.ProductId == cartItem.ProductId && cartItem.ProductPriceId == c.ProductPriceId).FirstOrDefault();
            if (foundedInCart != null)
            {
                foundedInCart.Count -= cartItem.Count;
                if (foundedInCart.Count <= 0)
                {
                    cart.Remove(foundedInCart);
                }
            }

            await _localStorageService.SetItemAsync(SD.ShoppingCart, cart);
            OnChange.Invoke();
        }


        public async Task IncrementCart(ShoppingCart cartItem)
        {
            var cart = await _localStorageService.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            if (cart == null)
            {
                cart = new List<ShoppingCart>();

            }
            var foundedInCart = cart.Where(c => c.ProductId == cartItem.ProductId && cartItem.ProductPriceId == c.ProductPriceId).FirstOrDefault();
            if (foundedInCart == null)
            {
                cart.Add(new ShoppingCart()
                {
                    Count = cartItem.Count,
                    ProductId = cartItem.ProductId,
                    ProductPriceId = cartItem.ProductPriceId,
                });
            }
            else
            {
                foundedInCart.Count += cartItem.Count;

            }
            await _localStorageService.SetItemAsync(SD.ShoppingCart, cart);
            OnChange.Invoke();

        }

        public async Task RemoveCart(ShoppingCart cartItem)
        {
            var cart = await _localStorageService.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            if (cart == null)
            {
                cart = new List<ShoppingCart>();

            }
            var foundedInCart = cart.Where(c => c.ProductId == cartItem.ProductId && cartItem.ProductPriceId == c.ProductPriceId).FirstOrDefault();
            if (foundedInCart != null)
            {
                cart.Remove(foundedInCart);
            }
            await _localStorageService.SetItemAsync(SD.ShoppingCart, cart);
            OnChange.Invoke();

        }


        public async Task<List<ShoppingCart>> GetCart()
        {
            var cart = await _localStorageService.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            if (cart == null)
            {
                return new List<ShoppingCart>();

            }

            foreach (var cartItem in cart)
            {
                var product = await _productService.Get(cartItem.ProductId);
                cartItem.Product = product;
                cartItem.ProductPrice = product.Prices.First(p => p.Id == cartItem.ProductPriceId);

            }
            return cart;
        }
    }
}
