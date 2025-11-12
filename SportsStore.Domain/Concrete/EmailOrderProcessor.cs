using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Net;
using System.Net.Mail;
using System.Text;
namespace SportsStore.Domain.Concrete
{
	public class EmailSettings
	{
		public string MailToAddress { get; set; }
		public string MailFromAddress { get; set; }
		public bool UseSsl { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string ServerName { get; set; }
		public int ServerPort { get; set; }
		public bool WriteAsFile { get; set; }
		public string FileLocation { get; set; }
	}

	public class EmailOrderProcessor : IOrderProcessor
	{
		private EmailSettings emailSettings;

		public EmailOrderProcessor(IOptions<EmailSettings> settings)
		{
			emailSettings = settings.Value;
		}

		public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
		{
			try
			{
				StringBuilder body = new StringBuilder()
				.AppendLine("A new order has been submitted")
				.AppendLine("---")
				.AppendLine("Items:");

				foreach (var line in cart.Lines)
				{
					var subtotal = line.Product.Price * line.Quantity;
					body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity, line.Product.Name, subtotal);
				}

				body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
						.AppendLine("---")
						.AppendLine("Ship to:")
						.AppendLine(shippingInfo.Name)
						.AppendLine(shippingInfo.Line1)
						.AppendLine(shippingInfo.Line2 ?? "")
						.AppendLine(shippingInfo.Line3 ?? "")
						.AppendLine(shippingInfo.City)
						.AppendLine(shippingInfo.State ?? "")
						.AppendLine(shippingInfo.Country)
						.AppendLine(shippingInfo.Zip)
						.AppendLine("---")
						.AppendFormat("Gift wrap: {0}", shippingInfo.GiftWrap ? "Yes" : "No");

				var client = new SmtpClient("localhost", 25);

				var message = new MailMessage();
				message.From = new MailAddress("SportsStore@example.com");
				message.To.Add("user@example.com");
				message.Subject = "SportsStore Notification";
				message.Body = body.ToString();

				if (emailSettings.WriteAsFile)
				{
					client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
					client.PickupDirectoryLocation = emailSettings.FileLocation;
					client.EnableSsl = false;
				}

				client.Send(message);
			}
			catch (Exception ex)
			{
				//Sorry - we couldn't send the email to confirm your RSVP.</b>
			}

		}
	}
}