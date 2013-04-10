namespace TekConf.Common.Entities.Messages
{
	public class SessionAddedToScheduleMessage : TinyMessageBase
	{
		public string UserName { get; set; }
		public string ConferenceSlug { get; set; }
		public string SessionSlug { get; set; }
	}
}