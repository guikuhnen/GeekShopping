using GeekShopping.Web.Models;
using GeekShopping.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
	public class CartController : Controller
	{
		private readonly IProductService _productService;
		private readonly ICartService _cartService;

		public CartController(IProductService productService, ICartService cartService)
		{
			_productService = productService ?? throw new ArgumentNullException(nameof(productService));
			_cartService = cartService ?? throw new ArgumentNullException(nameof(productService));
		}

		[Authorize]
		public async Task<IActionResult> Index()
		{
			return View(await FindUserCart());
		}

		private async Task<CartViewModel> FindUserCart()
		{
			var token = await HttpContext.GetTokenAsync("access_token");
			var userId = User.Claims.Where(c => c.Type == "sub")?.FirstOrDefault()?.Value;

			var response = await _cartService.FindCartByUserId(userId, token);

			if (response?.CartHeader != null)
				foreach (var detail in response.CartDetails)
					response.CartHeader.PurchaseAmount += (detail.Product.Price * detail.Count);

			return response;
		}

		public async Task<IActionResult> Remove(int id)
		{
			var token = await HttpContext.GetTokenAsync("access_token");

			var response = await _cartService.RemoveFromCart(id, token);

			if (response)
				return RedirectToAction(nameof(Index));

			return BadRequest();
		}
	}
}
