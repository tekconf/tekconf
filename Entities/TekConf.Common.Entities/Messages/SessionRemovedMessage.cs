namespace TekConf.UI.Api
{
	public class SessionRemovedMessage : TinyMessageBase
	{
		public string SessionSlug { get; set; }
		public string SessionTitle { get; set; }
		public string ConferenceName { get; set; }
	}
}