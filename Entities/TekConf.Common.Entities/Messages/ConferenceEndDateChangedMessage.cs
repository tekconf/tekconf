using System;

namespace TekConf.Common.Entities
{
	public class ConferenceEndDateChangedMessage : TinyMessageBase
	{
		public string ConferenceSlug { get; set; }
		public string ConferenceName { get; set; }
		public DateTime? OldValue { get; set; }
		public DateTime? NewValue { get; set; }
	}
}