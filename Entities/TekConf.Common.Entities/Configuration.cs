using System.Configuration;

namespace TekConf.UI.Api
{
	public class Configuration : IConfiguration
	{
		public string MongoServer { 
			get
			{
				return ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString;
			}
		}
	}
}