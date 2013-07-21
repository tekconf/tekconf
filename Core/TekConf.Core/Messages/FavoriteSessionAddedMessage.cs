using Cirrious.MvvmCross.Plugins.Messenger;

namespace TekConf.Core.Messages
{
	using TekConf.RemoteData.Dtos.v1;

	public class FavoriteSessionAddedMessage : MvxMessage
	{
		public ScheduleDto Schedule { get; set; }

		public FavoriteSessionAddedMessage(object sender, ScheduleDto schedule)
			: base(sender)
		{
			Schedule = schedule;
		}
	}
}