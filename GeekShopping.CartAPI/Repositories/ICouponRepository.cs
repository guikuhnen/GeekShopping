using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Repositories
{
	public interface ICouponRepository
	{
		Task<CouponVO> GetCouponByCouponCode(string couponCode, string token);
	}
}
