using System.Collections.Generic;

namespace TekConf.RemoteData.Dtos.v1
{
	public class HistoryDto
	{
		public virtual string ConferenceName { get; set; }
		public virtual string Notes { get; set; }
		public virtual List<string> Links { get; set; }
	}
}