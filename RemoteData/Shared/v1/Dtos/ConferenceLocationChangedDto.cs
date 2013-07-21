namespace TekConf.RemoteData.Dtos.v1
{
	public class ConferenceLocationChangedDto
	{
		public string ConferenceSlug { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }
		public string ConferenceName { get; set; }
	}
}