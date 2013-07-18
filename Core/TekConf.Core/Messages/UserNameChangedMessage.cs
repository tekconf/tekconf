namespace TekConf.Core.Messages
{
	using Cirrious.MvvmCross.Plugins.Messenger;

	public class UserNameChangedMessage : MvxMessage
	{
		public string UserName { get; set; }

		public UserNameChangedMessage(object sender) : base(sender)
		{
			
		}
	}
}