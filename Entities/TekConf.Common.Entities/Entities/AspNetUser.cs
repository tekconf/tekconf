using System;
using System.Collections.Generic;
using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TekConf.Common.Entities
{
	public class AspNetUser : ISupportInitialize, IEntity
	{
		private bool _isInitializingFromBson;

		[BsonId(IdGenerator = typeof(CombGuidGenerator))]
		public object _id { get; set; }

		public string UserName { get; set; }
		public string PasswordHash { get; set; }
		public object SecurityStamp { get; set; }

		public List<object> Roles { get; set; }
 		public List<object> Claims { get; set; } 
		public List<AspNetLogin> Logins { get; set; } 
		public void BeginInit()
		{
			_isInitializingFromBson = true;
		}

		public void EndInit()
		{
			_isInitializingFromBson = false;
		}
	}

	public class AspNetLogin
	{
		public string LoginProvider { get; set; }
		public string ProviderKey { get; set; }
	}
}