namespace TekConf.Common.Entities
{
	public class SessionRemovedMessage : TinyMessageBase
	{
		public string SessionSlug { get; set; }
		public string SessionTitle { get; set; }
		public string ConferenceName { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var p = obj as SessionRemovedMessage;
			if (p == null)
				return false;

			// Return true if the fields match:
			return (this.SessionTitle == p.SessionTitle)
							&& (this.ConferenceName == p.ConferenceName)
							&& (this.SessionSlug == p.SessionSlug);
		}

		public override int GetHashCode()
		{
			return this.SessionTitle.GetHashCode()
							+ this.ConferenceName.GetHashCode()
							+ this.SessionSlug.GetHashCode();
		}
	}
}