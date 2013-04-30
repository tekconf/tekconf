using System.Collections.Generic;

namespace TekConf.Common.Entities
{
	public class HistoryEntity : IEntity
	{
		public virtual string ConferenceName { get; set; }
		public virtual string Notes { get; set; }
		public virtual List<string> Links { get; set; }
	}
}