using MvvmCross.Plugins.Messenger;

namespace TekConf.Mobile.Core.Messages
{
    public class ConferenceAddedToScheduleMessage : MvxMessage
    {
		public ConferenceAddedToScheduleMessage(object sender, string slug) : base(sender)
		{
			this.Slug = slug;
		}
		public string Slug { get; private set; }
    }
}