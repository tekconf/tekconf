using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoNamesOrgImporter
{
	public class Configuration : IConfiguration
	{
		public string MongoServer
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString;
			}
		}
	}
}
