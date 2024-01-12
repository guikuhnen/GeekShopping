using GeekShopping.Email.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace GeekShopping.Email.Models
{
	public class EmailLog : BaseEntity
	{
		[Required, StringLength(150)]
		public string Email { get; set; }

		[Required]
		public string Log { get; set; }

		[Required]
		public DateTime SendDate { get; set; }

		public EmailLog() { }
	}
}
