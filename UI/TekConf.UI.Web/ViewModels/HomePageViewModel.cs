using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Web.Controllers
{
    public class HomePageViewModel
    {
        public List<SpeakersDto> FeaturedSpeakers { get; set; }
        public List<ConferencesDto> FeaturedConferences { get; set; } 
    }
}