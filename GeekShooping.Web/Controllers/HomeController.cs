using GeekShopping.Web.Models;
using GeekShopping.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GeekShopping.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IProductService _productService;
		private readonly ICartService _cartService;

		public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
		{
			_logger = logger;
			_productService = productService ?? throw new ArgumentNullException(nameof(productService));
			_cartService = cartService ?? throw new ArgumentNullException(nameof(productService));
		}

		public async Task<IActionResult> Index()
		{
			var products = await _productService.FindAll(string.Empty);

			return View(products);
		}

		[Authorize]
		public async Task<IActionResult> Details(long id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var model = await _productService.FindById(id, token);

			return View(model);
		}
		
		[Authorize, HttpPost]
		public async Task<IActionResult> AddItemToCart(ProductViewModel model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");

			CartViewModel cart = new()
			{
				CartHeader = new()
				{
					UserId = User.Claims.Where(c => c.Type == "sub")?.FirstOrDefault()?.Value
				}
			};

			CartDetailViewModel cartDetail = new()
			{
				Count = model.Count,
				ProductId = model.Id,
				Product = await _productService.FindById(model.Id, token)
			};

			List<CartDetailViewModel> listCartDetail = [cartDetail];

			var response = await _cartService.AddItemToCart(cart, token);
			if (response != null)
				return RedirectToAction(nameof(Index));

            return View(model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[Authorize]
		public async Task<IActionResult> Login()
		{
			//var accessToken = await HttpContext.GetTokenAsync("access_token");
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Logout()
		{
			return SignOut("Cookies", "oidc");
		}
	}
}
