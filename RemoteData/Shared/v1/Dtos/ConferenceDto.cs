using System;

namespace TekConf.RemoteData.Dtos.v1
{
    public class ConferenceDto
    {
        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string location { get; set; }
        public AddressDto address { get; set; }
        public string url { get; set; }
        public string slug { get; set; }
        public string sessionsUrl { get; set; }
        public string speakersUrl { get; set; }
    }
}