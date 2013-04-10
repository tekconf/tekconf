namespace TekConf.Common.Entities
{
	public class AddressEntity : IEntity
	{
		public int StreetNumber { get; set; }
		public string BuildingName { get; set; }
		public string StreetNumberSuffix { get; set; }
		public string StreetName { get; set; }
		public string StreetType { get; set; }
		public string StreetDirection { get; set; }
		public string AddressType { get; set; }
		public string AddressTypeId { get; set; }
		public string LocalMunicipality { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string GoverningDistrict { get; set; }
		public string PostalArea { get; set; }
		public string Country { get; set; }
	}
}