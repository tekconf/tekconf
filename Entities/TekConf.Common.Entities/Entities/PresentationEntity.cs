using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TekConf.Common.Entities
{
	public class PresentationEntity : IEntity
	{
		public PresentationEntity()
		{
			this.Tags = new List<string>();
			this.Subjects = new List<string>();
			this.DownloadPaths = new List<string>();
			this.History = new List<HistoryEntity>();
		}

		[BsonId(IdGenerator = typeof(CombGuidGenerator))]
		public Guid _id { get; set; }
		public string slug { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public List<string> Tags { get; set; }
		public List<string> Videos { get; set; }
		public List<string> Subjects { get; set; }
		public string Difficulty { get; set; }
		public int Length { get; set; }

		public int NumberOfViews { get; set; }
		public string Comments { get; set; }
		public string ImageUrl { get; set; }
		public List<string> DownloadPaths { get; set; }
		public string SpeakerSlug { get; set; }
		public DateTime UploadedOn { get; set; }
		public List<HistoryEntity> History { get; set; } 

	}
}