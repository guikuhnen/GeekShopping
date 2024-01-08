using GeekShopping.CartAPI.Models.Base;

namespace GeekShopping.CartAPI.Models
{
	public class Cart : BaseEntity
	{
		public CartHeader CartHeader { get; set; }

		public IEnumerable<CartDetail> CartDetails { get; set; }

		public Cart() { }
	}
}
