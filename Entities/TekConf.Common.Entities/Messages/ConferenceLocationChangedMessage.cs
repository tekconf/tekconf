namespace TekConf.Common.Entities
{
	public class ConferenceLocationChangedMessage : TinyMessageBase
	{
		public string ConferenceName { get; set; }
		public string ConferenceSlug { get; set; }
		public string OldValue { get; set; }
		public string NewValue { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var p = obj as ConferenceLocationChangedMessage;
			if (p == null)
				return false;

			// Return true if the fields match:
			return (this.ConferenceName == p.ConferenceName)
							&& (this.ConferenceSlug == p.ConferenceSlug)
							&& (this.OldValue == p.OldValue)
							&& (this.NewValue == p.NewValue);
		}

		public override int GetHashCode()
		{
			return this.ConferenceName.GetHashCode()
							+ this.ConferenceSlug.GetHashCode()
							+ this.OldValue.GetHashCode()
							+ this.NewValue.GetHashCode();
		}
	}
}