using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Web.ViewModels
{
	public class SpeakersViewModel
	{
		public List<FullConferenceDto> OpenConferences { get; set; }
		public List<PresentationDto> Presentations { get; set; }

		public List<FullConferenceDto> MyConferences { get; set; }
	}
}