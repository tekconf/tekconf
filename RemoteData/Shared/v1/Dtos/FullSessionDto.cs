using System;
using System.Collections.Generic;

namespace TekConf.RemoteData.Dtos.v1
{
		public class FullSessionDto
		{
				public string slug { get; set; }
				public string title { get; set; }
				public DateTime start { get; set; }
				public DateTime end { get; set; }
				public string room { get; set; }
				public string difficulty { get; set; }
				public string description { get; set; }
				public string twitterHashTag { get; set; }
				public string sessionType { get; set; }
				public List<string> links { get; set; }
				public List<string> tags { get; set; }
				public List<string> subjects { get; set; }
				public List<string> resources { get; set; }
				public List<string> prerequisites { get; set; }
				public List<FullSpeakerDto> speakers { get; set; }
				public bool? isAddedToSchedule { get; set; }
				public string startDescription
				{
						get
						{

								if (this.start == default(DateTime))
								{
										return "Not scheduled yet";
								}
								else
								{
										return this.start.ToString("dddd h:mm tt");
								}
						}
				}
		}
}