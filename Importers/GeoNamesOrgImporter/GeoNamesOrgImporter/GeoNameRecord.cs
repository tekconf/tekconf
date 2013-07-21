using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace GeoNamesOrgImporter
{
	[DelimitedRecord("\t")]
	public class GeoNameRecord
	{
		public string geonameid { get; set; }
		public string name { get; set; }
		public string asciiname { get; set; }
		public string alternatenames { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string featureclass { get; set; }
		public string featurecode { get; set; }
		public string countrycode { get; set; }
		public string cc2 { get; set; }
		public string admin1code { get; set; }
		public string admin2code { get; set; }
		public string admin3code { get; set; }
		public string admin4code { get; set; }
		public string population { get; set; }
		public string elevation { get; set; }
		public string dem { get; set; }
		public string timezone { get; set; }
		public string modificationdate { get; set; }


//geonameid         : integer id of record in geonames database
//name              : name of geographical point (utf8) varchar(200)
//asciiname         : name of geographical point in plain ascii characters, varchar(200)
//alternatenames    : alternatenames, comma separated varchar(5000)
//latitude          : latitude in decimal degrees (wgs84)
//longitude         : longitude in decimal degrees (wgs84)
//feature class     : see http://www.geonames.org/export/codes.html, char(1)
//feature code      : see http://www.geonames.org/export/codes.html, varchar(10)
//country code      : ISO-3166 2-letter country code, 2 characters
//cc2               : alternate country codes, comma separated, ISO-3166 2-letter country code, 60 characters
//admin1 code       : fipscode (subject to change to iso code), see exceptions below, see file admin1Codes.txt for display names of this code; varchar(20)
//admin2 code       : code for the second administrative division, a county in the US, see file admin2Codes.txt; varchar(80) 
//admin3 code       : code for third level administrative division, varchar(20)
//admin4 code       : code for fourth level administrative division, varchar(20)
//population        : bigint (8 byte int) 
//elevation         : in meters, integer
//dem               : digital elevation model, srtm3 or gtopo30, average elevation of 3''x3'' (ca 90mx90m) or 30''x30'' (ca 900mx900m) area in meters, integer. srtm processed by cgiar/ciat.
//timezone          : the timezone id (see file timeZone.txt) varchar(40)
//modification date : date of last modification in yyyy-MM-dd format
	}
}
