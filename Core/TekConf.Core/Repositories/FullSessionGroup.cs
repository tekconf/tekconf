namespace TekConf.Core.Repositories
{
	using System.Collections.Generic;

	using TekConf.RemoteData.Dtos.v1;

	public class FullSessionGroup : List<FullSessionDto>
	{
		public string Key { get; set; }
		public IEnumerable<FullSessionDto> Items { get; set; }
		public FullSessionGroup(string key, IEnumerable<FullSessionDto> items)
			: base(items)
		{
			Key = key;
			Items = items;
		}
	}
}