using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Repositories
{
	public interface ICartRepository
	{
		Task<CartVO> SaveOrUpdateCart(CartVO cartVO);

		Task<CartVO> FindCartByUserId(string userId);

		Task<bool> ApplyCoupon(string userId, string couponCode);

		Task<bool> RemoveCoupon(string userId);

		Task<bool> RemoveFromCart(long cartDetailsId);

		Task<bool> ClearCart(string userId);
	}
}
