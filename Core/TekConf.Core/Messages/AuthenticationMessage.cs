using Cirrious.MvvmCross.Plugins.Messenger;

namespace TekConf.Core.Messages
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