using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Web.Controllers
{
		public class HomePageViewModel
		{
				public List<FullSpeakerDto> FeaturedSpeakers { get; set; }
				public List<FullConferenceDto> FeaturedConferences { get; set; }
				public int TotalCount { get; set; }
		}
}