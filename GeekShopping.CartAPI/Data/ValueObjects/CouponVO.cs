namespace GeekShopping.CartAPI.Data.ValueObjects
{
	public class CouponVO
	{
		public long Id { get; set; }

		public required string CouponCode { get; set; }

		public decimal DiscountAmount { get; set; }
	}
}
