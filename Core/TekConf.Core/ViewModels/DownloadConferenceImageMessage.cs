using Cirrious.MvvmCross.Plugins.Messenger;

namespace TekConf.Core.ViewModels
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