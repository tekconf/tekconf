namespace TekConf.UI.Api
{
	public class ConferenceLocationChangedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }
	}
}