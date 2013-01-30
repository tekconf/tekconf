using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.Web.Controllers
{
    public class SpeakerDetailViewModel
    {
        public ConferencesDto Conference { get; set; }
        public FullSpeakerDto Speaker { get; set; }
        public List<SessionsDto> Sessions { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}