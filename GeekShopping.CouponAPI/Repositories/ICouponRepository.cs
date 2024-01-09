using GeekShopping.CouponAPI.Data.ValueObjects;

namespace GeekShopping.CouponAPI.Repositories
{
	public interface ICouponRepository
	{
		Task<CouponVO> GetCouponByCouponCode(string couponCode);
	}
}
