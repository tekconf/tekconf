namespace TekConf.UI.Api
{
	public class ConferenceCreatedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
		public string ConferenceName { get; set; }

	}
}