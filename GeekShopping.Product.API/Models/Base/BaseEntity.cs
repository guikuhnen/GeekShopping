using System.ComponentModel.DataAnnotations;

namespace GeekShopping.ProductAPI.Models.Base
{
	public class BaseEntity
	{
		[Key]
        public long Id { get; set; }
    }
}
