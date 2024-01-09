using GeekShopping.CouponAPI.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CouponAPI.Models
{
	public class Coupon : BaseEntity
	{
		[Required, StringLength(30)]
		public string CouponCode { get; set; }

		[Required, Precision(5, 2)]
		public decimal DiscountAmount { get; set; }

		public Coupon() { }
	}
}
