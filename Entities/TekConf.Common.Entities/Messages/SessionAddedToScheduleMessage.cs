namespace TekConf.Common.Entities.Messages
{
	public class SessionAddedToScheduleMessage : TinyMessageBase
	{
		public string UserName { get; set; }
		public string ConferenceSlug { get; set; }
		public string SessionSlug { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var p = obj as SessionAddedToScheduleMessage;
			if (p == null)
				return false;

			// Return true if the fields match:
			return (this.UserName == p.UserName)
							&& (this.ConferenceSlug == p.ConferenceSlug)
							&& (this.SessionSlug == p.SessionSlug);
		}

		public override int GetHashCode()
		{
			return this.UserName.GetHashCode()
							+ this.ConferenceSlug.GetHashCode()
							+ this.SessionSlug.GetHashCode();
		}
	}
}