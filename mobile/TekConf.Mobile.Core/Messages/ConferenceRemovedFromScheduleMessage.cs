using MvvmCross.Plugins.Messenger;

namespace TekConf.Mobile.Core.Messages
{
    public class ConferenceRemovedFromScheduleMessage : MvxMessage
    {
		public ConferenceRemovedFromScheduleMessage(object sender, string slug) : base(sender)
		{
			this.Slug = slug;
		}
		public string Slug { get; private set; }
    }
}