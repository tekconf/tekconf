using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekConf.Core.Data.Entities
{
	public interface IEntity
	{
		int Id { get; set; }
	}
	public class Entity : IEntity
	{
		public int Id { get; set; }
	}

	public class Conference : Entity
	{
		public string Name { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public virtual ICollection<Session> Sessions { get; set; }
	}

	public class Session : Entity
	{
		public string Title { get; set; }
		public int ConferenceId { get; set; }
		public virtual Conference Conference { get; set; }
	}
	public class TekConfDbContext : DbContext
	{
		public IDbSet<Conference> Conferences { get; set; }
		public IDbSet<Session> Sessions { get; set; }
	}
}
