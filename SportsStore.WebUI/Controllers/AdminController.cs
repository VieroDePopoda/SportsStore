using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private IProductRepository repository;

		public AdminController(IProductRepository repo)
		{
			repository = repo;
		}

		public ViewResult Index()
		{
			return View(repository.Products);
		}

		public ViewResult Edit(int productId)
		{
			var product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

			return View(product);
		}

		[HttpPost]
		public ActionResult Edit(Product product, IFormFile image)
		{
			if (ModelState.IsValid)
			{
				if (image != null)
				{
					product.ImageMimeType = image.ContentType;

					using (var memoryStream = new MemoryStream())
					{
						image.CopyTo(memoryStream);
						product.ImageData = memoryStream.ToArray();
					}
				}

				repository.Save(product);

				TempData["message"] = string.Format("{0} has been saved", product.Name);
				return RedirectToAction("Index");
			}
			else
			{
				return View(product);
			}
		}

		public ViewResult Create()
		{
			return View("Edit", new Product());
		}

		[HttpPost]
		public ActionResult Delete(int productId)
		{
			Product deletedProduct = repository.Delete(productId);
			if (deletedProduct != null)
			{
				TempData["message"] = string.Format("{0} was deleted", deletedProduct.Name);
			}
			return RedirectToAction("Index");
		}
	}
}
