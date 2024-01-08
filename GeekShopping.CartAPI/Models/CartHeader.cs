using GeekShopping.CartAPI.Models.Base;

namespace GeekShopping.CartAPI.Models
{
	public class CartHeader : BaseEntity
	{
		public string? UserId { get; set; }

		public string? CouponCode { get; set;}

		public CartHeader() { }
	}
}
