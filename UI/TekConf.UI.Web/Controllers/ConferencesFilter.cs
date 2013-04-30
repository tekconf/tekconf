namespace TekConf.UI.Web.Controllers
{
	public class ConferencesFilter
	{
		public string sortBy { get; set; }
		public bool showPastConferences { get; set; }
		public bool showOnlyOpenCalls { get; set; }
		public bool showOnlyOnSale { get; set; }
		public string viewAs { get; set; }
		public string search { get; set; }

		public string city { get; set; }
		public string state { get; set; }
		public string country { get; set; }
		public double? latitude { get; set; }
		public double? longitude { get; set; }
		public double? distance { get; set; }

	}
}