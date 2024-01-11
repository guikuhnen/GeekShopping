using GeekShopping.OrderAPI.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GeekShopping.OrderAPI.Models
{
	public class OrderHeader : BaseEntity
	{
		public string UserId { get; set; }

		public string CouponCode { get; set; }

		[Required, Range(1, 10000), Precision(7, 2)]
		public decimal PurchaseAmount { get; set; }

		// Checkout

		[Required, Precision(5, 2)]
		public decimal DiscountAmount { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTime PurchaseDate { get; set; }

		public DateTime OrderTime { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		public string CardNumber { get; set; }

		public string CVV { get; set; }

		public string ExpiryMonthYear { get; set; }

		public int OrderTotalItens { get; set; }

		public bool PaymentStatus { get; set; }

		public ICollection<OrderDetail> OrderDetails { get; set; }

		public OrderHeader() { }
	}
}
