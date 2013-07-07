using Cirrious.MvvmCross.Plugins.Messenger;

namespace TekConf.Core.Messages
{
	public class DownloadConferenceImageMessage : MvxMessage
	{
		public DownloadConferenceImageMessage(object sender, string imageUrl) : base(sender)
		{
			ImageUrl = imageUrl;
		}

		public string ImageUrl { get; private set; }
	}
}