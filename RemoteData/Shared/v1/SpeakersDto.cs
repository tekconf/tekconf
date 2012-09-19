namespace TekConf.RemoteData.Dtos.v1
{
    public class SpeakersDto
    {
        public string slug { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string url { get; set; }
        public string fullName { get { return this.firstName + " " + this.lastName; } }
        public string description { get; set; }

    }
}