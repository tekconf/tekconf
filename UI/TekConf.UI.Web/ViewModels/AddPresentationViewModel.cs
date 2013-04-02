using System.Collections.Generic;

namespace TekConf.UI.Web.ViewModels
{
	public class AddPresentationViewModel
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public List<string> Tags { get; set; }
	}
}