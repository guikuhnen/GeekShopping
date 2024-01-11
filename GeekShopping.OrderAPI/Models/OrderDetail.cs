using GeekShopping.OrderAPI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Models
{
	public class OrderDetail : BaseEntity
	{
		public long OrderHeaderId { get; set; }
		[ForeignKey("OrderHeaderId")]
		public virtual OrderHeader OrderHeader { get; set; }

		public int Count { get; set; }

		public OrderDetail() { }
	}
}
