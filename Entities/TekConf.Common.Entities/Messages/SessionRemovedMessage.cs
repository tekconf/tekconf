namespace TekConf.UI.Api
{
	public class SessionRemovedMessage : TinyMessageBase
	{
		public string SessionSlug { get; set; }
	}
}