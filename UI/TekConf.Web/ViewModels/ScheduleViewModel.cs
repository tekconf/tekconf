namespace TekConf.Web.ViewModels
{
	using System.Collections.Generic;

	using TekConf.RemoteData.Dtos.v1;

	public class ScheduleViewModel
	{
		public IEnumerable<FullConferenceDto> Conferences { get; set; }
	}
}