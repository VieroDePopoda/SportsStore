using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
	[TestClass]
	public class AdminSecutiryTests
	{
		[TestMethod]
		public void Can_Login_With_Valid_Credentials()
		{
			// Arrange - create a mock authentication provider
			Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
			mock.Setup(m => m.ValidateCredentialsAsync("admin", "123").Result).Returns(true);
			// Arrange - create the view model

			LoginViewModel model = new LoginViewModel
			{
				UserName = "admin",
				Password = "123"
			};
			// Arrange - create the controller
			AccountController target = new AccountController(mock.Object);
			// Act - authenticate using valid credentials
			var result = target.Login(model, "/MyURL").Result;
			// Assert
			Assert.IsInstanceOfType(result, typeof(RedirectResult));
			Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
		}

		[TestMethod]
		public void Cannot_Login_With_Invalid_Credentials()
		{
			// Arrange - create a mock authentication provider
			Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
			mock.Setup(m => m.ValidateCredentialsAsync("badUser", "badPass").Result).Returns(false);
			// Arrange - create the view model
			LoginViewModel model = new LoginViewModel
			{
				UserName = "badUser",
				Password = "badPass"
			};
			// Arrange - create the controller
			AccountController target = new AccountController(mock.Object);
			// Act - authenticate using valid credentials
			var result = target.Login(model, "/MyURL").Result;
			// Assert
			Assert.IsInstanceOfType(result, typeof(ViewResult));
			Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
		}
	}
}
