using System;

namespace TekConf.Common.Entities
{
	public class RoomChangedArgs : EventArgs
	{
		public RoomChangedArgs(string sessionSlug, string oldValue, string newValue)
		{
			this.SessionSlug = sessionSlug;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		public string SessionSlug { get; private set; }
		public string OldValue { get; private set; }
		public string NewValue { get; private set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var p = obj as RoomChangedArgs;
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