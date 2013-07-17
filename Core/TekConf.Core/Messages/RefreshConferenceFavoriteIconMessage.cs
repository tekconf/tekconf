namespace TekConf.Core.Messages
{
	using Cirrious.MvvmCross.Plugins.Messenger;

	public class RefreshConferenceFavoriteIconMessage : MvxMessage
	{
		public RefreshConferenceFavoriteIconMessage(object sender)
			: base(sender)
		{

		}
	}
}