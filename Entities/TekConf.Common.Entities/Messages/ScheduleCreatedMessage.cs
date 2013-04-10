namespace TekConf.Common.Entities.Messages
{
	public class ScheduleCreatedMessage : TinyMessageBase
	{
		public string UserName { get; set; }
		public string ConferenceSlug { get; set; }
	}
}