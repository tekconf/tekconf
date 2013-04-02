﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using TekConf.UI.Api;

namespace TekConf.Common.Entities
{
	public class GeoLocationEntity : IEntity
	{
		[BsonId(IdGenerator = typeof(CombGuidGenerator))]
		public Guid _id { get; private set; }

		public GeoLocationEntity()
		{
			_id = Guid.NewGuid();
		}

		public string name { get; set; }
		public string asciiname { get; set; }
		public string alternatenames { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public string countrycode { get; set; }
		public string countryname { get; set; }
		public string timezone { get; set; }
		public string fipscode { get; set; }

	}
}
