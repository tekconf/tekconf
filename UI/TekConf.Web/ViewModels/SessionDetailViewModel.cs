namespace TekConf.Web.ViewModels
{
	using TekConf.RemoteData.Dtos.v1;

	public class SessionDetailViewModel
	{
		public SessionDto Session { get; set; }
		public FullConferenceDto Conference { get; set; }
	}
}