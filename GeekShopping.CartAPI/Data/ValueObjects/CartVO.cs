namespace GeekShopping.CartAPI.Data.ValueObjects
{
	public class CartVO
	{
		public long Id { get; set; }

		public CartHeaderVO CartHeader { get; set; }

		public IEnumerable<CartDetailVO> CartDetails { get; set; }
	}
}
