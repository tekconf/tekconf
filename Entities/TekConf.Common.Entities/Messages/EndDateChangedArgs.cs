using System;

namespace TekConf.Common.Entities
{
	public class EndDateChangedArgs : EventArgs
	{
		public EndDateChangedArgs(string sessionSlug, DateTime oldValue, DateTime newValue)
		{
			this.SessionSlug = sessionSlug;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		public string SessionSlug { get; private set; }
		public DateTime OldValue { get; private set; }
		public DateTime NewValue { get; private set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var p = obj as EndDateChangedArgs;
			if (p == null)
				return false;

			// Return true if the fields match:
			return (this.SessionSlug == p.SessionSlug)
				&& (this.OldValue == p.OldValue)
				&& (this.NewValue == p.NewValue);
		}

		public override int GetHashCode()
		{
			return this.SessionSlug.GetHashCode()
				+ this.OldValue.GetHashCode()
				+ this.NewValue.GetHashCode();
		}
	}
}