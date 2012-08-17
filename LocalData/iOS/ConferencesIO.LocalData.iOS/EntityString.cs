using System;
using System.Collections.Generic;
using Catnap;

namespace ConferencesIO.LocalData.iOS
{

	public class EntityString : Entity<string>
	{
		public override bool IsTransient
		{
			get { return Id == string.Empty; }
		}
	}

}
