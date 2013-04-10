namespace TekConf.Common.Entities
{
	public class ConferenceUpdatedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
		public string ConferenceName { get; set; }
	}
}