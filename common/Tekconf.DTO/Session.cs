using System;
using System.Collections.Generic;
using System.Linq;

namespace Tekconf.DTO
{
    public class Session
	{
		public string Slug { get; set; }

		public string Title { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public string Description { get; set; }

		public string Room { get; set; }

        public List<Speaker> Speakers { get; set; }

		public string SpeakerName()
		{
			if (Speakers == null || !Speakers.Any ()) {
				return "N/A";
			}

			if (Speakers.Count () == 1) {
				var speaker = Speakers.First ();
				return string.Format ("{0} {1}", speaker.FirstName, speaker.LastName);
			}

			return string.Format ("{0} speakers", Speakers.Count ());
		}
    }
}