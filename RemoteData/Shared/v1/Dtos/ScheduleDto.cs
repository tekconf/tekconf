using System.Collections.Generic;

namespace TekConf.RemoteData.Dtos.v1
{
    public class ScheduleDto
    {
        public string userSlug { get; set; }
        public string conferenceSlug { get; set; }
        public string url { get; set; }
        public List<FullSessionDto> sessions { get; set; }
    }
}