using System;

namespace TekConf.UI.Api
{
	public class ConferenceStartDateChangedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
		public DateTime? OldValue { get; set; }
		public DateTime? NewValue { get; set; }

		public string ConferenceName { get; set; }
	}
}