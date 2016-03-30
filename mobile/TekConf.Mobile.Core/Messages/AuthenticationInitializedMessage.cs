using MvvmCross.Plugins.Messenger;

namespace TekConf.Mobile.Core.Messages
{
    public class AuthenticationInitializedMessage : MvxMessage
    {
		public AuthenticationInitializedMessage(object sender) : base(sender)
		{

		}
    }
}