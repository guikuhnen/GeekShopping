using GeekShopping.OrderAPI.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Models
{
	public class OrderDetail : BaseEntity
	{
		public long OrderHeaderId { get; set; }
		[ForeignKey("OrderHeaderId")]
		public virtual OrderHeader OrderHeader { get; set; }
		
		public long ProductId { get; set; }
		[StringLength(150)]
		public string ProductName { get; set; }

		public int Count { get; set; }

		[Required, Range(1, 10000), Precision(7, 2)]
		public decimal Price { get; set; }

		public OrderDetail() { }
	}
}
