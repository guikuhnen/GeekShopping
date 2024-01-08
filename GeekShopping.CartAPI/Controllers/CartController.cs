using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private ICartRepository _repository;

		public CartController(ICartRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		[HttpPost("add-cart")]
		public async Task<ActionResult<CartVO>> AddCart(CartVO cartVO)
		{
			var cart = await _repository.SaveOrUpdateCart(cartVO);

			if (cart == null)
				return NotFound();

			return Ok(cart);
		}

		[HttpGet("find-cart/{id}")]
		public async Task<ActionResult<CartVO>> FindCartByUserId(string id)
		{
			var cart = await _repository.FindCartByUserId(id);

			if (cart == null)
				return NotFound();

			return Ok(cart);
		}

		[HttpPut("update-cart")]
		public async Task<ActionResult<CartVO>> UpdateCart(CartVO cartVO)
		{
			var cart = await _repository.SaveOrUpdateCart(cartVO);

			if (cart == null)
				return NotFound();

			return Ok(cart);
		}

		[HttpDelete("remove-cart/{id}")]
		public async Task<ActionResult<CartVO>> RemoveCart(int id)
		{
			var status = await _repository.RemoveFromCart(id);

			if (!status)
				return BadRequest();

			return Ok(status);
		}
	}
}
