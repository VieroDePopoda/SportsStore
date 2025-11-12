using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private readonly IAuthProvider authProvider;

		public AccountController(IAuthProvider _authProvider)
		{
			authProvider = _authProvider;
		}

		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				if (await authProvider.ValidateCredentialsAsync(model.UserName, model.Password))
				{
					await authProvider.SignInAsync(HttpContext, model.UserName);
					return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
				}

				ModelState.AddModelError("", "Invalid login");
				return View();
			}
			else
			{
				return View();
			}
		}

		public async Task<IActionResult> Logout()
		{
			await authProvider.SignOutAsync(HttpContext);
			return RedirectToAction("Login");
		}
	}
}
