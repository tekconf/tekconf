using System;

namespace TekConf.UI.Api
{
	public class RoomChanged : EventArgs
	{
		public RoomChanged(string sessionSlug, string oldValue, string newValue)
		{
			this.SessionSlug = sessionSlug;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		public string SessionSlug { get; private set; }
		public string OldValue { get; private set; }
		public string NewValue { get; private set; }
	}
}