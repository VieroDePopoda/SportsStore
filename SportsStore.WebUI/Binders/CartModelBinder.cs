using Microsoft.AspNetCore.Mvc.ModelBinding;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Extensions;

namespace SportsStore.WebUI.Binders
{
	public class CartModelBinder : IModelBinder
	{
		private const string sessionKey = "Cart";

		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var session = bindingContext.HttpContext.Session;
			var cart = session.GetObject<Cart>(sessionKey);

			if (cart == null)
			{
				cart = new();
				session.SetObject(sessionKey, cart);
			}

			bindingContext.Result = ModelBindingResult.Success(cart);

			return Task.CompletedTask;
		}
	}

	public class CartModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder? GetBinder(ModelBinderProviderContext context)
		{
			if (context.Metadata.ModelType == typeof(Cart))
			{
				return new CartModelBinder();
			}

			return null;
		}
	}
}
