namespace Tekconf.DTO
{
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

		public string AddressShortDisplay()
		{
			if (!string.IsNullOrWhiteSpace (City) && !string.IsNullOrWhiteSpace (StateOrProvince)) {
				return string.Format ("{0}, {1}", City, StateOrProvince);
			}

			if (!string.IsNullOrWhiteSpace (City)) {
				return City;
			}

			if (!string.IsNullOrWhiteSpace (StateOrProvince)) {
				return StateOrProvince;
			}

			return "No Location Set";
		}

		public string AddressLongDisplay()
		{
			var addressLine1 = string.IsNullOrEmpty (AddressLine1) ? "" : AddressLine1 + "\n";
			var addressLine2 = string.IsNullOrEmpty (AddressLine2) ? "" : AddressLine2 + "\n";
			var addressLine3 = string.IsNullOrEmpty (AddressLine3) ? "" : AddressLine3 + "\n";
			var city = string.IsNullOrEmpty (City) ? "" : City + " ";
			var stateOrProvince = string.IsNullOrEmpty (StateOrProvince) ? "" : StateOrProvince + " ";
			var postalCode = string.IsNullOrEmpty (PostalCode) ? "" : PostalCode + " ";
			var country = string.IsNullOrEmpty (Country) ? "" : Country + " ";

			var address = string.Format ("{0}{1}{2}{3}{4}{5}{6}", addressLine1, addressLine2, addressLine3, city, stateOrProvince, postalCode, country);
			return address;
		}
    }
}