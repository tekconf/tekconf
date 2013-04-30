namespace TekConf.Common.Entities
{
	public class SessionAddedMessage : TinyMessageBase
	{
		public string SessionSlug { get; set; }
		public string SessionTitle { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var p = obj as SessionAddedMessage;
			if (p == null)
				return false;

			// Return true if the fields match:
			return (this.SessionSlug == p.SessionSlug)
							&& (this.SessionTitle == p.SessionTitle);
		}

		public override int GetHashCode()
		{
			return this.SessionSlug.GetHashCode()
							+ this.SessionTitle.GetHashCode();
		}
	}
}