using GeekShopping.Web.Models;
using GeekShopping.Web.Services;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace GeekShopping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [Authorize]
        public async Task<IActionResult> Index()
		{
			var token = await HttpContext.GetTokenAsync("access_token");
			var products = await _productService.FindAll(token);

            return View(products);
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (ModelState.IsValid)
			{
				var token = await HttpContext.GetTokenAsync("access_token");
				var response = await _productService.Create(model, token);

                if (response != null)
                    return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET
        public async Task<IActionResult> Edit(long id)
		{
			var token = await HttpContext.GetTokenAsync("access_token");
			var model = await _productService.FindById(id, token);

            if (model != null)
                return View(model);
            else
                return NotFound();
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> Edit(ProductModel model)
        {
            if (ModelState.IsValid)
			{
				var token = await HttpContext.GetTokenAsync("access_token");
				var response = await _productService.Update(model, token);

                if (response != null)
                    return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize]
        // GET
        public async Task<IActionResult> Delete(long id)
		{
			var token = await HttpContext.GetTokenAsync("access_token");
			var model = await _productService.FindById(id, token);

            if (model != null)
                return View(model);
            else
                return NotFound();
        }

        [Authorize(Roles = Role.Admin), HttpPost]
        public async Task<IActionResult> Delete(ProductModel model)
		{
			var token = await HttpContext.GetTokenAsync("access_token");
			var response = await _productService.Delete(model.Id, token);

            if (response)
                return RedirectToAction(nameof(Index));

            return View(model);
        }
    }
}
