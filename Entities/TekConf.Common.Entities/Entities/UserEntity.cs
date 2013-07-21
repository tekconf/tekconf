using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using TekConf.Common.Entities;
using TinyMessenger;

namespace TekConf.Common.Entities
{
	public class UserEntity : IEntity
	{
		public UserEntity()
		{
			WindowsPhoneEndpointUris = new List<string>();
		}

		[BsonIgnore]
		public ITinyMessengerHub Hub { get; set; }

		public void Save(MongoCollection<UserEntity> collection)
		{
			if (!this.isSaved)
			{
				if (_id == default(Guid))
				{
					_id = Guid.NewGuid();
				}
				slug = (firstName + "-" + lastName).ToLower().GenerateSlug();
				isSaved = true;
			}
			collection.Save(this);
		}

		[BsonId(IdGenerator = typeof(CombGuidGenerator))]
		public Guid _id { get; set; }
		public string slug { get; private set; }
		public bool isSaved { get; private set; }

		public string userName { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string description { get; set; }
		public string blogUrl { get; set; }
		public string twitterName { get; set; }
		public string facebookUrl { get; set; }
		public string linkedInUrl { get; set; }
		public string emailAddress { get; set; }
		public string phoneNumber { get; set; }
		public bool isFeatured { get; set; }
		public string profileImageUrl { get; set; }
		public string company { get; set; }

		public List<string> WindowsPhoneEndpointUris { get; set; } 
		public string googlePlusUrl { get; set; }
		public string vimeoUrl { get; set; }
		public string youtubeUrl { get; set; }
		public string githubUrl { get; set; }
		public string coderWallUrl { get; set; }
		public string stackoverflowUrl { get; set; }
		public string bitbucketUrl { get; set; }
		public string codeplexUrl { get; set; }

		public string fullName
		{
			get
			{
				string name = string.Empty;
				if (string.IsNullOrWhiteSpace(this.firstName))
				{
					if (!string.IsNullOrWhiteSpace(this.lastName))
					{
						name = this.lastName;
					}
				}
				else if (string.IsNullOrWhiteSpace(this.lastName))
				{
					if (!string.IsNullOrWhiteSpace(this.firstName))
					{
						name = this.firstName;
					}
				}
				else
				{
					name = this.firstName + " " + this.lastName;
				}
				return name;
			}
		}
	}
}