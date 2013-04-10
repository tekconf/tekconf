namespace TekConf.Common.Entities
{
	public class SpeakerRemovedMessage : TinyMessageBase
	{
		public string SessionSlug { get; set; }
		public string SessionTitle { get; set; }
		public string SpeakerSlug { get; set; }
		public string SpeakerName { get; set; }
	}
}