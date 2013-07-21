using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace GeoNamesOrgImporter
{
	[DelimitedRecord("\t")]
	public class CountryRecord
	{
		//#ISO	ISO3	ISO-Numeric	fips	
		//Country	Capital	Area(in sq km)	Population	Continent	tld	CurrencyCode	
		//CurrencyName	Phone	Postal Code Format	Postal Code Regex	Languages	
		//geonameid	neighbours	EquivalentFipsCode

		public string ISO { get; set; }
		public string ISO3 { get; set; }
		public string ISONumeric { get; set; }
		public string FIPS { get; set; }
		public string Country { get; set; }
		public string Capital { get; set; }
			public string Area { get; set; }
		public string Population { get; set; }
		public string Continent { get; set; }
		public string TLD { get; set; }
		public string CurrencyCode { get; set; }
		public string CurrencyName { get; set; }
		public string Phone { get; set; }
		public string PostalCodeFormat { get; set; }
		public string PostalCodeRegex { get; set; }
		public string Languages { get; set; }
		public string geonameid { get; set; }
		public string neighbours { get; set; }
		public string EquivalentFipsCode { get; set; }
	}
}
