using AmadoApp.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AmadoApp.Core.Entities.Account;

namespace AmadoApp.Business.Services.Implementations
{
	public static class SendMessageService
	{
		public static void SendEmailMessage(string toUser, string webUser, string pincode)
		{
			switch (pincode)
			{
				case "1000":
					using (var client = new SmtpClient("smtp.gmail.com", 587))
					{
						client.UseDefaultCredentials = false;
						client.DeliveryMethod = SmtpDeliveryMethod.Network;
						client.Credentials = new NetworkCredential("karamlma@code.edu.az", "ocbx juim drnh fqpl");
						client.EnableSsl = true;

						var mailMessage = new MailMessage()
						{
							From = new MailAddress("karamlma@code.edu.az"),
							Subject = "Welcome to Furni. Website",
							Body = $"Hello I am from {webUser}" +
							$"<p>Welcome to Furni. , your ultimate destination for style inspiration and exclusive updates! As a valued subscriber, you're in for a treat. Get ready to elevate your inbox with a curated selection of fashion insights, lifestyle trends, and insider news tailored specifically for you. Delight in early access to our latest collections, limited-time offers, and exclusive promotions, all delivered directly to your inbox. Your preferences matter, and we're committed to providing a personalized experience that matches your unique taste. Join our vibrant community, share your thoughts, and engage with fellow subscribers. Your privacy is important to us, and we ensure the security of your information. Expect surprises – from exciting giveaways to special treats, we love to spoil our subscribers. Get set for a subscription journey filled with elegance, sophistication, and endless style possibilities. Embrace your individuality with Diana, where every newsletter is a celebration of your distinct persona.<p>",
							IsBodyHtml = true
						};

						mailMessage.To.Add(toUser);
						client.Send(mailMessage);
					}
					break;
				case "2000":
					using (var client = new SmtpClient("smtp.gmail.com", 587))
					{
						client.UseDefaultCredentials = false;
						client.DeliveryMethod = SmtpDeliveryMethod.Network;
						client.Credentials = new NetworkCredential("karamlma@code.edu.az", "ocbx juim drnh fqpl");
						client.EnableSsl = true;

						var mailMessage = new MailMessage()
						{
							From = new MailAddress("karamlma@code.edu.az"),
							Subject = "Welcome to Furni. Website",
							Body = $"Hello I am from {webUser} :D" +
							$"<p>" +
							$"Hello and have a good time\r\n" +
							$"I hope you are having a good day. As the Furni. group, " +
							$"we would like to inform you about a new product added to our website. " +
							$"If you want to browse that product, you can visit the website.\r\n" +
							$"Furni. wishes you a nice day" +
							$"<p>",
							IsBodyHtml = true
						};

						mailMessage.To.Add(toUser);
						client.Send(mailMessage);
					}
					break;
				default:
					using (var client = new SmtpClient("smtp.gmail.com", 587))
					{
						client.UseDefaultCredentials = false;
						client.DeliveryMethod = SmtpDeliveryMethod.Network;
						client.Credentials = new NetworkCredential("karamlma@code.edu.az", "ocbx juim drnh fqpl");
						client.EnableSsl = true;

						var mailMessage = new MailMessage()
						{
							From = new MailAddress("karamlma@code.edu.az"),
							Subject = "Welcome to Diana Website",
							Body = $"Hello {webUser}, " +
							$"Thank you for visiting our website. " +
							$"Please write this code to confirmation section\n" +
							$"\n" +
							$"Pincode: {pincode}",
							IsBodyHtml = true
						};

						mailMessage.To.Add(toUser);
						client.Send(mailMessage);
					}
					break;
			}
		}

		public static void SendUrlMessage(string toUser, string webUser, string url)
		{
			using (var client = new SmtpClient("smtp.gmail.com", 587))
			{
				client.UseDefaultCredentials = false;
				client.DeliveryMethod = SmtpDeliveryMethod.Network;
				client.Credentials = new NetworkCredential("karamlma@code.edu.az", "ocbx juim drnh fqpl");
				client.EnableSsl = true;

				var mailMessage = new MailMessage
				{
					From = new MailAddress("karamlma@code.edu.az"),
					Subject = "Welcome to Furni. ",
					Body = $"<!DOCTYPE html>" +
					   $"<html>" +
					   $"<head>" +
					   $"<style>" +
					   $"  body {{" +
					   $"    font-family: 'Arial', sans-serif;" +
					   $"    background-color: #f4f4f4;" +
					   $"    margin: 0;" +
					   $"    padding: 0;" +
					   $"  }}" +
					   $"  .container {{" +
					   $"    max-width: 600px;" +
					   $"    margin: auto;" +
					   $"    padding: 20px;" +
					   $"    background-color: #ffffff;" +
					   $"    border-radius: 5px;" +
					   $"    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);" +
					   $"  }}" +
					   $"  h1 {{" +
					   $"    color: #333333;" +
					   $"  }}" +
					   $"  p {{" +
					   $"    color: #666666;" +
					   $"  }}" +
					   $"</style>" +
					   $"</head>" +
					   $"<body>" +
					   $"  <div class='container'>" +
					   $"    <h1>Welcome to Furni., {webUser}!</h1>" +
					   $"    <p>This is your confirmation url for change password verification:</p>" +
					   $"    <h2 style='color: #007BFF;'><a href=\"{url}\">Click Me!</a></h2>" +
					   $"  </div>" +
					   $"</body>" +
					   $"</html>",
					IsBodyHtml = true
				};


				mailMessage.To.Add(toUser);
				client.Send(mailMessage);
			}
		}

		public static async Task SendUrlMessageAsync(AppUser user, string url)
		{
			SendUrlMessage(
				toUser: user.Email,
				webUser: $"{user.Name} {user.Surname}",
				url: url
				);
		}
	}
}