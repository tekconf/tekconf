using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using SendGridMail;
using SendGridMail.Transport;

namespace TekConf.UI.Api
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;

		public EmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Send(string message)
		{
			var myMessage = SendGrid.GetInstance();

			myMessage.From = new MailAddress("robgibbens@arteksoftware.com");

			var recipients = new List<string>
				{
					@"Rob Gibbens <robgibbens@gmail.com.com>",
					@"Rob Gibbens <robgibbens@arteksoftware.com>",
				};

			myMessage.AddTo(recipients);

			myMessage.Subject = "TekConf updates";

			myMessage.Html = string.Format("<p>{0}</p>", message);
			myMessage.Text = string.Format("{0}", message);

		
			// Create credentials, specifying your user name and password.
			var credentials = new NetworkCredential("azure_4c325a45cc209c2f4b523188604da156@azure.com", "a3gdm7bn");

			// Create an SMTP transport for sending email.
			var transportSmtp = SMTP.GetInstance(credentials);

			// Send the email.
			transportSmtp.Deliver(myMessage);
		}
	}
}