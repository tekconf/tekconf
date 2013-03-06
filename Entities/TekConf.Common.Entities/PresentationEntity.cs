using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TekConf.UI.Api
{
	public class PresentationEntity
	{
		[BsonId(IdGenerator = typeof(CombGuidGenerator))]
		public Guid _id { get; set; }
		public string slug { get; set; }
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

		public string SpeakerSlug { get; set; }
	}
}