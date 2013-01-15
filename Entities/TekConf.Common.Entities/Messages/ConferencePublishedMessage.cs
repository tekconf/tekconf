namespace TekConf.UI.Api
{
	public class ConferencePublishedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
		public string ConferenceName { get; set; }
	}
}