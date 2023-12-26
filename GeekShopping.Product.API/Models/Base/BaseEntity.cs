using System.ComponentModel.DataAnnotations;

namespace GeekShopping.Product.API.Models.Base
{
	public class BaseEntity
	{
		[Key]
        public long Id { get; set; }
    }
}
