using System.Collections.Generic;

namespace TekConf.Web.ViewModels
{
	public class AddPresentationViewModel
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public List<string> Tags { get; set; }
		public List<string> Videos { get; set; }
		public List<string> Subjects { get; set; }
		public List<string> DownloadPaths { get; set; }
		public string Difficulty { get; set; }
		public int Length { get; set; }
		public string ImageUrl { get; set; }
	}
}