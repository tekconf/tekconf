using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoNamesOrgImporter
{
	public interface IConfiguration
	{
		string MongoServer { get; }
	}
}
