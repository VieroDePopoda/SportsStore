using Microsoft.AspNetCore.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Components
{
	public class NavViewComponent : ViewComponent
	{
		private IProductRepository repository;

		public NavViewComponent(IProductRepository repo)
		{
			repository = repo;
		}
		public IViewComponentResult Invoke(string category = null)
		{
			ViewBag.SelectedCategory = category;

			IEnumerable<string> categories = repository.Products
			.Select(x => x.Category)
			.Distinct()
			.OrderBy(x => x);

			return View(categories);
		}
	}
}
