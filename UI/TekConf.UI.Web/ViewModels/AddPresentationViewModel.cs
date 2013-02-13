using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TekConf.UI.Web.ViewModels
{
	public class AddPresentationViewModel
	{
		public string ConferenceSlug { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public List<string> Tags { get; set; }
		public List<PresentationHistory> History { get; set; } 
	}

	public class PresentationHistory
	{
		public DateTime DatePresented { get; set; }
		public string Notes { get; set; }
		public string ConferenceName { get; set; }
		public List<string> Links { get; set; } 
	}
}