using System.ComponentModel.DataAnnotations;

namespace GeekShopping.CartAPI.Models.Base
{
	public class BaseEntity
	{
		[Key]
        public long Id { get; set; }
    }
}
