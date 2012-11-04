using System;
using TekConf.RemoteData.v1;

namespace TekConf.RemoteData.Dtos.v1
{
    public class ConferencesDto
    {
        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string location { get; set; }
        public AddressDto address { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string imageUrl { get; set; }
        public DateTime registrationOpens { get; set; }
        public DateTime registrationCloses { get; set; }
        public string slug
        {
            get { return name.GenerateSlug(); }
        }
        public bool IsOnSale()
        {
            bool isOnSale = this.registrationOpens <= DateTime.Now && this.registrationCloses >= DateTime.Now;

            return isOnSale;
        }
    }
}