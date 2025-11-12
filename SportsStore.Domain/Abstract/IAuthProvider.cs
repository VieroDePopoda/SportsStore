using Microsoft.AspNetCore.Http;

namespace SportsStore.Domain.Abstract
{
	public interface IAuthProvider
	{
		Task<bool> ValidateCredentialsAsync(string username, string password);
		Task SignInAsync(HttpContext context, string username);
		Task SignOutAsync(HttpContext context);
	}
}
