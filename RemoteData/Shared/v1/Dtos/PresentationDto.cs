using System;
using System.Collections.Generic;

namespace TekConf.RemoteData.Dtos.v1
{
	public class PresentationDto
	{
		public string Slug { get; set; }

		public string Title { get; set; }
		public string Description { get; set; }
		public List<string> Tags { get; set; }
		public List<string> Videos { get; set; } 
		public List<string> Subjects { get; set; }
		public string Difficulty { get; set; }
		public string Comments { get; set; }
		public string ImageUrl { get; set; }
		public List<string> DownloadPaths { get; set; }
		public int Length { get; set; }


		public int NumberOfViews { get; set; }
		public SpeakerDto Speaker { get; set; }
		public DateTime UploadedOn { get; set; }

		public List<HistoryDto> History { get; set; }
		public List<string> GivenAtConferences { get; set; }
	}
}