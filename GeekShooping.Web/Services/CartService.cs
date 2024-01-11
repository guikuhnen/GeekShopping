using GeekShopping.Web.Models;
using GeekShopping.Web.Utils;
using System.Net;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services
{
    public class CartService : ICartService
    {
        public readonly HttpClient _client;
        public const string BasePath = "api/v1/cart";

        public CartService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<object> Checkout(CartHeaderViewModel cartHeader, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.PostAsJson($"{BasePath}/checkout", cartHeader);

            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CartHeaderViewModel>();
            else if (response.StatusCode == HttpStatusCode.PreconditionFailed)
                return "Coupon Price has changed. Please confirm!";
            else
                throw new Exception($"Something went wrong calling the API: " + $"{response.ReasonPhrase}");
		}

        public async Task<CartViewModel> AddItemToCart(CartViewModel cart, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PostAsJson($"{BasePath}/add-cart", cart);

            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CartViewModel>();
            else
                throw new Exception($"Something went wrong calling the API: " + $"{response.ReasonPhrase}");
        }

        public async Task<CartViewModel> FindCartByUserId(string userId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"{BasePath}/find-cart/{userId}");

            return await response.ReadContentAs<CartViewModel>();
        }

        public async Task<CartViewModel> UpdateCart(CartViewModel cart, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.PutAsJson($"{BasePath}/update-cart", cart);

            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CartViewModel>();
            else
                throw new Exception($"Something went wrong calling the API: " + $"{response.ReasonPhrase}");
        }

        public async Task<bool> ApplyCoupon(CartViewModel cart, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.PostAsJson($"{BasePath}/apply-coupon", cart);

			if (response.IsSuccessStatusCode)
				return await response.ReadContentAs<bool>();
			else
				throw new Exception($"Something went wrong calling the API: " + $"{response.ReasonPhrase}");
		}

        public async Task<bool> RemoveCoupon(string userId, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.DeleteAsync($"{BasePath}/remove-coupon/{userId}");

			if (response.IsSuccessStatusCode)
				return await response.ReadContentAs<bool>();
			else
				throw new Exception($"Something went wrong calling the API: " + $"{response.ReasonPhrase}");
		}

        public async Task<bool> RemoveFromCart(long cartDetailsId, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync($"{BasePath}/remove-cart/{cartDetailsId}");

            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<bool>();
            else
                throw new Exception($"Something went wrong calling the API: " + $"{response.ReasonPhrase}");
        }

        public async Task<bool> ClearCart(string userId, string token)
		{
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _client.DeleteAsync($"{BasePath}/clear-cart/{userId}");

			if (response.IsSuccessStatusCode)
				return await response.ReadContentAs<bool>();
			else
				throw new Exception($"Something went wrong calling the API: " + $"{response.ReasonPhrase}");
		}
    }
}