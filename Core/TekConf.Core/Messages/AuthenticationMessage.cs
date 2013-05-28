using Cirrious.MvvmCross.Plugins.Messenger;

namespace TekConf.Core.ViewModels
{
	public class AuthenticationMessage : MvxMessage
	{
		public AuthenticationMessage(object sender, string userName) : base(sender)
		{
			UserName = userName;
		}

		public string UserName { get; private set; }
	}
}