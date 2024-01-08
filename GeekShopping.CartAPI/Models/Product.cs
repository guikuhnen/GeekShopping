using GeekShopping.CartAPI.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartAPI.Models
{
	public class Product
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long Id { get; set; }

		[Required, StringLength(150)]
		public string Name { get; set; }

		[Required, Range(1, 10000), Precision(7, 2)]
		public decimal Price { get; set; }

		[StringLength(500)]
		public string? Description { get; set; }

		[StringLength(50)]
		public string? CategoryName { get; set; }

		[StringLength(300)]
		public string? ImageURL { get; set; }

		public Product() { }
	}
}
