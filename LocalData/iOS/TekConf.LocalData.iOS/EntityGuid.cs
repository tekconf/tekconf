using System;
using System.Collections.Generic;
using Catnap;

namespace TekConf.LocalData.iOS
{
	public class EntityGuid : Entity<Guid>
	{
		public override bool IsTransient
		{
			get { return Id == Guid.Empty; }
		}
	}

}
