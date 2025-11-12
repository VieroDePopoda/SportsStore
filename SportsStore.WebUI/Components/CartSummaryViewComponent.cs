using Microsoft.AspNetCore.Mvc;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Extensions;

namespace SportsStore.WebUI.Components
{
	public class CartSummaryViewComponent : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			var cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();
			return View(cart);
		}
	}
}
