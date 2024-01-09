namespace GeekShopping.Web.Models
{
	public class CartViewModel
	{
		public long Id { get; set; }

		public CartHeaderViewModel CartHeader { get; set; }

		public IEnumerable<CartDetailViewModel> CartDetails { get; set; }
	}
}
