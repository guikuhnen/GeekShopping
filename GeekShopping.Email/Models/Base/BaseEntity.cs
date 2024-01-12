using System.ComponentModel.DataAnnotations;

namespace GeekShopping.Email.Models.Base
{
	public class BaseEntity
	{
		[Key]
        public long Id { get; set; }
    }
}
