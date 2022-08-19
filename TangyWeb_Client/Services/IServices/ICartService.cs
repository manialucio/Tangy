using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Services.IServices
{
    public interface ICartService
    {
        Task DecrementCart(ShoppingCart shoppingCart);
        Task IncrementCart(ShoppingCart shoppingCart);
        Task RemoveCart(ShoppingCart cartItem);
        Task<List<ShoppingCart>> GetCart();
        Task ClearCart();

        event Action OnChange;

    }
}
