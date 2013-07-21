using System;

namespace TekConf.Common.Entities
{
	public class ConferenceStartDateChangedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
		public DateTime? OldValue { get; set; }
		public DateTime? NewValue { get; set; }
		public string ConferenceName { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			var p = obj as ConferenceStartDateChangedMessage;
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