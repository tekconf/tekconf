using System;
namespace TekConf.Mobile.Core
{
	public class ConferenceModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string HighlightColor { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string ImageUrl { get; set; }
		public string City { get; set; }
		public string State { get; set; }
	}
}

