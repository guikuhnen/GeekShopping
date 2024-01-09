using System.ComponentModel.DataAnnotations;

namespace GeekShopping.CouponAPI.Models.Base
{
	public class BaseEntity
	{
		[Key]
        public long Id { get; set; }
    }
}
