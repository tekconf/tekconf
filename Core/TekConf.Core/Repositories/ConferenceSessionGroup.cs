using System.Collections.Generic;

namespace TekConf.Core.Repositories
{
	public class ConferenceSessionGroup : List<ConferenceSessionListDto>
	{
		public string Key { get; set; }

		public ConferenceSessionGroup(string key, IEnumerable<ConferenceSessionListDto> items)
			: base(items)
		{
			Key = key;
		}
	}
}