using System;
using System.Collections.Generic;
using Catnap;

namespace TekConf.LocalData.iOS
{
	
	public class EntityInt : Entity<int>
	{
		public override bool IsTransient
		{
			get { return Id == 0; }
		}
	}

}
