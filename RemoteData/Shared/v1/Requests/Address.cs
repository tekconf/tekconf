//using System.ComponentModel.DataAnnotations;

namespace TekConf.UI.Api.Services.Requests.v1
{
    public class Address
    {
        //[Display(Name = "Street Number")]
        public int StreetNumber { get; set; }

        ////[Display(Name = "Building Name")]
        //public string BuildingName { get; set; }

        ////[Display(Name = "Street Number Suffix")]
        //public string StreetNumberSuffix { get; set; }

        //[Display(Name = "Street Name")]
        public string StreetName { get; set; }

        ////[Display(Name = "Street Type")]
        //public string StreetType { get; set; }

        ////[Display(Name = "Street Direction")]
        //public string StreetDirection { get; set; }

        ////[Display(Name = "Address Type")]
        //public string AddressType { get; set; }

        ////[Display(Name = "Address Type Id")]
        //public string AddressTypeId { get; set; }

        ////[Display(Name = "Local Municipality")]
        //public string LocalMunicipality { get; set; }

        //[Display(Name = "City")]
        public string City { get; set; }

        //[Display(Name = "State")]
        public string State { get; set; }

        ////[Display(Name = "Governing District")]
        //public string GoverningDistrict { get; set; }

        //[Display(Name = "Postal Area")]
        public string PostalArea { get; set; }

        //[Display(Name = "Country")]
        public string Country { get; set; }
    }
}