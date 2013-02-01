using System.Web;

namespace TekConf.UI.Web.Controllers
{
	using TekConf.Azure;

	public class FileSystemImageSaver : IImageSaver
		{
				public string SaveImage(string imageName, HttpPostedFileBase image)
				{
						var url = "img/conferences/" + imageName;
						var filename = HttpContext.Current.Server.MapPath("~/" + url);
						var root = FullyQualifiedApplicationPath;

						image.SaveAs(filename);

						return root + url;
				}

				public string FullyQualifiedApplicationPath
				{
						get
						{
								//Return variable declaration
								var appPath = string.Empty;

								//Getting the current context of HTTP request

								//Checking the current context content
								//Formatting the fully qualified website url/name
								appPath = string.Format("{0}://{1}{2}{3}",
																				HttpContext.Current.Request.Url.Scheme,
																				HttpContext.Current.Request.Url.Host,
																				HttpContext.Current.Request.Url.Port == 80
																						? string.Empty
																						: ":" + HttpContext.Current.Request.Url.Port,
																				HttpContext.Current.Request.ApplicationPath);

								if (!appPath.EndsWith("/"))
										appPath += "/";

								return appPath;
						}
				}
		}
}