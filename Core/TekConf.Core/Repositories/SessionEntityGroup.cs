namespace TekConf.Core.Repositories
{
	using System.Collections.Generic;

	using TekConf.Core.Entities;

	public class SessionEntityGroup : List<SessionEntity>
	{
		public string Key { get; set; }
		public IEnumerable<SessionEntity> Items { get; set; }
		public SessionEntityGroup(string key, IEnumerable<SessionEntity> items)
			: base(items)
		{
			Key = key;
			Items = items;
		}
	}
}