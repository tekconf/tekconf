namespace TekConf.Common.Entities
{
	public class SpeakerAddedMessage : TinyMessageBase
	{
		public string SessionSlug { get; set; }
		public string SessionTitle { get; set; }
		public string SpeakerSlug { get; set; }
		public string SpeakerName { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var p = obj as SpeakerAddedMessage;
			if (p == null)
				return false;

			// Return true if the fields match:
			return (this.SessionTitle == p.SessionTitle)
							&& (this.SessionSlug == p.SessionSlug)
							&& (this.SpeakerSlug == p.SpeakerSlug)
							&& (this.SpeakerName == p.SpeakerName);
		}

		public override int GetHashCode()
		{
			return this.SessionTitle.GetHashCode()
							+ this.SessionSlug.GetHashCode()
							+ this.SpeakerSlug.GetHashCode()
							+ this.SpeakerName.GetHashCode();
		}
	}
}