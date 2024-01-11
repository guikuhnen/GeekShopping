using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GeekShopping.CartAPI.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private ICartRepository _cartRepository;
		private ICouponRepository _couponRepository;
		private IRabbitMQMessageSender _messageSender;

		public CartController(ICartRepository cartRepository, ICouponRepository couponRepository, IRabbitMQMessageSender messageSender)
		{
			_cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
			_couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
			_messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
		}

		[HttpPost("add-cart")]
		public async Task<ActionResult<CartVO>> AddCart(CartVO cartVO)
		{
			var cart = await _cartRepository.SaveOrUpdateCart(cartVO);

			if (cart == null)
				return NotFound();

			return Ok(cart);
		}

		[HttpPost("checkout")]
		public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO checkoutHeaderVO)
		{
			if (checkoutHeaderVO?.UserId == null)
				return BadRequest();

			var cart = await _cartRepository.FindCartByUserId(checkoutHeaderVO.UserId);

			if (cart == null)
				return NotFound();

			if (!string.IsNullOrEmpty(checkoutHeaderVO.CouponCode))
			{
				var token = await HttpContext.GetTokenAsync("access_token");

				CouponVO coupon = await _couponRepository.GetCouponByCouponCode(checkoutHeaderVO.CouponCode, token);

				if (checkoutHeaderVO.DiscountAmount != coupon.DiscountAmount)
					return StatusCode((int)HttpStatusCode.PreconditionFailed);
			}

			checkoutHeaderVO.CartDetails = cart.CartDetails;
			checkoutHeaderVO.DateTime = DateTime.Now.ToUniversalTime();

			_messageSender.SendMessage(checkoutHeaderVO, "checkoutqueue");

			return Ok(checkoutHeaderVO);
		}

		[HttpGet("find-cart/{id}")]
		public async Task<ActionResult<CartVO>> FindCartByUserId(string id)
		{
			var cart = await _cartRepository.FindCartByUserId(id);

			if (cart == null)
				return NotFound();

			return Ok(cart);
		}

		[HttpPut("update-cart")]
		public async Task<ActionResult<CartVO>> UpdateCart(CartVO cartVO)
		{
			var cart = await _cartRepository.SaveOrUpdateCart(cartVO);

			if (cart == null)
				return NotFound();

			return Ok(cart);
		}

		[HttpPost("apply-coupon")]
		public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO cartVO)
		{
			var status = await _cartRepository.ApplyCoupon(cartVO.CartHeader.UserId, cartVO.CartHeader.CouponCode);

			if (!status)
				return NotFound();

			return Ok(status);
		}

		[HttpDelete("remove-coupon/{userId}")]
		public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
		{
			var status = await _cartRepository.RemoveCoupon(userId);

			if (!status)
				return NotFound();

			return Ok(status);
		}

		[HttpDelete("remove-cart/{id}")]
		public async Task<ActionResult<CartVO>> RemoveCart(int id)
		{
			var status = await _cartRepository.RemoveFromCart(id);

			if (!status)
				return BadRequest();

			return Ok(status);
		}

		[HttpDelete("clear-cart/{id}")]
		public async Task<ActionResult<CartVO>> ClearCart(string id)
		{
			var status = await _cartRepository.ClearCart(id);

			if (!status)
				return BadRequest();

			return Ok(status);
		}
	}
}
