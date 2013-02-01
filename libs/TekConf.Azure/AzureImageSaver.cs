using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TekConf.Azure
{
	using System.IO;


	public class AzureImageSaver : IImageSaver
	{
		private readonly IImageSaverConfiguration _configuration;

		public AzureImageSaver(IImageSaverConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string SaveImage(string imageName, HttpPostedFileBase image)
		{
			string uri = string.Empty;
			var storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);

			var blobStorage = storageAccount.CreateCloudBlobClient();
			CloudBlobContainer container = blobStorage.GetContainerReference("images");
			string uniqueBlobName = string.Format("conferences/{0}", imageName);
			CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);

			blob.Properties.ContentType = image.ContentType;
			blob.UploadFromStream(image.InputStream);
			uri = blob.Uri.ToString();

			return uri;
		}
	}
}