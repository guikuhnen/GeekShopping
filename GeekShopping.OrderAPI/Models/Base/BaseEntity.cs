using System.ComponentModel.DataAnnotations;

namespace GeekShopping.OrderAPI.Models.Base
{
	public class BaseEntity
	{
		[Key]
        public long Id { get; set; }
    }
}
