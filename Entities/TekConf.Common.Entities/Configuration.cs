using System.Configuration;

namespace TekConf.UI.Api
{
	public class Configuration : IConfiguration
	{
		public int cacheTimeout
		{
			get
			{
				int timeout = 120;
				if (ConfigurationManager.AppSettings["cacheTimeout"] != null)
					int.TryParse(ConfigurationManager.AppSettings["cacheTimeout"].ToString(), out timeout);
				return timeout;
			}
		}

		public string webUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["webUrl"];
			}
		}

		public string MongoServer { 
			get
			{
				return ConfigurationManager.ConnectionStrings["MongoServer"].ConnectionString;
			}
		}
	}
}