namespace TekConf.RemoteData.Dtos.v1
{
	public class SessionRoomChangedDto
	{
		public string ConferenceSlug { get; set; }
		public string SessionSlug { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }
		public string SessionTitle { get; set; }
	}
}