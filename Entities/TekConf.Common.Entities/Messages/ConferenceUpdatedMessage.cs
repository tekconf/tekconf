namespace TekConf.UI.Api
{
	public class ConferenceUpdatedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
		public string ConferenceName { get; set; }
	}
}