namespace TekConf.Common.Entities
{
	public class SessionAddedMessage : TinyMessageBase
	{
		public string SessionSlug { get; set; }
		public string SessionTitle { get; set; }
	}
}