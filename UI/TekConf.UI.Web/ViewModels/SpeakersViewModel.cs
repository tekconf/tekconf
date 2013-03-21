using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Web.ViewModels
{
	public class ScheduleViewModel
	{
		public IEnumerable<FullConferenceDto> Conferences { get; set; }
	}

	public class SpeakersViewModel
	{
		public List<ConferencesDto> OpenConferences { get; set; }
		public List<PresentationDto> Presentations { get; set; }

		public List<FullConferenceDto> MyConferences { get; set; }
	}
}