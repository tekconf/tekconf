namespace TekConf.UI.Api
{
	public class SpeakerRemovedMessage : TinyMessageBase
	{
		public string SessionSlug { get; set; }
		public string SpeakerSlug { get; set; }
	}
}