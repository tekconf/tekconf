using System;

namespace TekConf.RemoteData.Dtos.v1
{
	public class ConferenceEndDateChangedDto
	{
		public string ConferenceSlug { get; set; }
		public string ConferenceName { get; set; }
		public DateTime? OldValue { get; set; }
		public DateTime? NewValue { get; set; }
	}
}