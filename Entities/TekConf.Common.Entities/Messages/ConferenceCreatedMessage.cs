namespace TekConf.Common.Entities
{
	public class ConferenceCreatedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
		public string ConferenceName { get; set; }

	}
}