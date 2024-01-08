using GeekShopping.CartAPI.Models.Base;

namespace GeekShopping.CartAPI.Models
{
	public class CartDetail : BaseEntity
	{
		public long CartHeaderId { get; set; }
		public CartHeader CartHeader { get; set; }

		public long ProductId { get; set; }
		public Product Product { get; set; }

		public int Count { get; set; }

		public CartDetail() { }
	}
}
