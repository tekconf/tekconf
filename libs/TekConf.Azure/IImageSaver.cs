using System.Web;

namespace TekConf.Azure
{
	public interface IImageSaver
	{
		string SaveImage(string imageName, HttpPostedFileBase image);
	}
}