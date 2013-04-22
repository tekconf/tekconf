namespace TekConf.UI.Api
{
	using System.Collections.Generic;

	public interface IEmailSender
	{
		void Send(string message);
		void Send(string message, List<string> recipients);
	}
}