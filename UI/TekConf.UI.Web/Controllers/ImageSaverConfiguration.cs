using System.Configuration;
using TekConf.Azure;

namespace TekConf.UI.Web.Controllers
{
	public class ImageSaverConfiguration : IImageSaverConfiguration
	{
		public string ImageUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["ImageUrl"];
			}
		}

		public string ConnectionString
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString;
			}
		}
	}
}