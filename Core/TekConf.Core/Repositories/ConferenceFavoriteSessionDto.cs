using System;

namespace TekConf.Core.Repositories
{
	public class ConferenceFavoriteSessionDto
	{
		public string title { get; set; }
		public string slug { get; set; }
		public string room { get; set; }
		public string startDescription { get; set; }
		public DateTime start { get; set; }
	}
}