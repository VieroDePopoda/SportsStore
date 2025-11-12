using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SportsStore.Domain.Abstract;
using System.Security.Claims;

namespace SportsStore.Domain.Concrete
{
	public class CookieAuthProvider : IAuthProvider
	{
		private readonly IConfiguration config;

		public CookieAuthProvider(IConfiguration _config)
		{
			config = _config;
		}

		public Task<bool> ValidateCredentialsAsync(string username, string password)
		{
			var admin = config.GetSection("AdminCredentials");
			return Task.FromResult(username == admin["Username"] && password == admin["Password"]);
		}

		public async Task SignInAsync(HttpContext context, string username)
		{
			var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);
			await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
		}

		public async Task SignOutAsync(HttpContext context)
		{
			await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		}
	}
}
