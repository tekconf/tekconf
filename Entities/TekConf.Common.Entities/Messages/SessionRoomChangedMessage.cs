namespace TekConf.Common.Entities
{
	public class SessionRoomChangedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
		public string SessionSlug { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }
		public string SessionTitle { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var p = obj as SessionRoomChangedMessage;
			if (p == null)
				return false;

			// Return true if the fields match:
			return (this.SessionTitle == p.SessionTitle)
							&& (this.ConferenceSlug == p.ConferenceSlug)
							&& (this.OldValue == p.OldValue)
							&& (this.NewValue == p.NewValue)
							&& (this.SessionSlug == p.SessionSlug);
		}

		public override int GetHashCode()
		{
			return this.SessionTitle.GetHashCode()
							+ this.ConferenceSlug.GetHashCode()
							+ this.OldValue.GetHashCode()
							+ this.NewValue.GetHashCode()
							+ this.SessionSlug.GetHashCode();
		}
	}
}