using System.Configuration;

namespace TekConf.Common.Entities
{
	public class EntityConfiguration : IEntityConfiguration
	{
		public int cacheTimeout
		{
			get
			{
				int timeout = 120;
				if (ConfigurationManager.AppSettings["cacheTimeout"] != null)
					int.TryParse(ConfigurationManager.AppSettings["cacheTimeout"], out timeout);
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

		public string TwitterConsumerKey
		{
			get
			{
				return ConfigurationManager.AppSettings["TwitterConsumerKey"];
			}
		}
		public string TwitterConsumerSecret
		{
			get
			{
				return ConfigurationManager.AppSettings["TwitterConsumerSecret"];
			}
		}
		public string TwitterAccessToken
		{
			get
			{
				return ConfigurationManager.AppSettings["TwitterAccessToken"];
			}
		}
		public string TwitterAccessTokenSecret
		{
			get
			{
				return ConfigurationManager.AppSettings["TwitterAccessTokenSecret"];
			}
		}

	}
}