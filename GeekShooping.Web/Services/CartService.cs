using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services
{
    public class CartService : ICartService
    {
        public readonly HttpClient _client;
        public const string BasePath = "api/v1/product";

        public CartService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<CartViewModel> Checkout(CartHeaderViewModel cartHeader, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<CartViewModel> AddItemToCart(CartViewModel cart, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<CartViewModel> FindCartByUserId(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<CartViewModel> UpdateCart(CartViewModel cart, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ApplyCoupon(CartViewModel cart, string couponCode, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveCoupon(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveFromCart(long cartId, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ClearCart(string userId, string token)
        {
            throw new NotImplementedException();
        }
    }
}