using System;
using System.Collections.Generic;

namespace TekConf.UI.Web.ViewModels
{
	public class AddPresentationHistoryViewModel
	{
		public string PresentationSlug { get; set; }
		public string ConferenceName { get; set; }
		public DateTime DatePresented { get; set; }
		public string Notes { get; set; }
		public List<string> Links { get; set; }
		public string SpeakerSlug { get; set; }
	}
}