using System;
using System.Collections.Generic;
using Catnap;

namespace ConferencesIO.LocalData.iOS
{
	
	public class EntityInt : Entity<int>
	{
		public override bool IsTransient
		{
			get { return Id == 0; }
		}
	}

}
