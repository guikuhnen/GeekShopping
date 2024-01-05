using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
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
            var products = await _productService.FindAll();

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
                var response = await _productService.Create(model);

                if (response != null)
                    return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET
        public async Task<IActionResult> Edit(long id)
        {
            var model = await _productService.FindById(id);

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
                var response = await _productService.Update(model);

                if (response != null)
                    return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize]
        // GET
        public async Task<IActionResult> Delete(long id)
        {
            var model = await _productService.FindById(id);

            if (model != null)
                return View(model);
            else
                return NotFound();
        }

        [Authorize(Roles = Role.Admin), HttpPost]
        public async Task<IActionResult> Delete(ProductModel model)
        {
            var response = await _productService.Delete(model.Id);

            if (response)
                return RedirectToAction(nameof(Index));

            return View(model);
        }
    }
}
