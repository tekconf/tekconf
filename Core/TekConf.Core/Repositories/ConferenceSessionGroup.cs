using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

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

	public class FullSessionGroup : List<FullSessionDto>
	{
		public string Key { get; set; }

		public FullSessionGroup(string key, IEnumerable<FullSessionDto> items)
			: base(items)
		{
			Key = key;
		}
	}
}