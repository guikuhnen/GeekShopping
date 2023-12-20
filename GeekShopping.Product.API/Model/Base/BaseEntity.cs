using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.Product.API.Model.Base
{
	public class BaseEntity
	{
		[Key]
        public long Id { get; set; }
    }
}
