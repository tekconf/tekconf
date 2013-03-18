using System;
using System.Collections.Generic;

namespace TekConf.RemoteData.Dtos.v1
{
	public class PresentationDto
	{
		public string Slug { get; set; }
		public SpeakerDto Speaker { get; set; }
		public DateTime UploadedOn { get; set; }
		public int NumberOfViews { get; set; }
		public List<string> GivenAtConferences { get; set; }
		public List<string> Tags { get; set; }
		public List<string> Subjects { get; set; }
		public string Description { get; set; }
		public string Title { get; set; }
		public string Level { get; set; }
		public string Comments { get; set; }
		public string ImagePath { get; set; }
		public List<string> DownloadPaths { get; set; }
		public int ExpectedNumberOfMinutes { get; set; }
		public List<HistoryDto> History { get; set; } 
	}
}