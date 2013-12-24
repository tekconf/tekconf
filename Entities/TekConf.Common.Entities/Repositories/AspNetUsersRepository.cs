using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace TekConf.Common.Entities
{
	public class AspNetUsersRepository : IAspNetUsersRepository
	{
		private readonly IEntityConfiguration _entityConfiguration;

		public AspNetUsersRepository(IEntityConfiguration entityConfiguration)
		{
			this._entityConfiguration = entityConfiguration;
		}


		public void Save(AspNetUser entity)
		{
			throw new NotImplementedException();
		}

		public IQueryable<AspNetUser> AsQueryable()
		{
			var collection = MongoCollection();
			return collection.AsQueryable();
		}

		public void Remove(Guid id)
		{
			throw new NotImplementedException();
		}

		public string GetUserName(string providerName, string providerKey)
		{
			var allUsers = this.AsQueryable().ToList();
			var user = allUsers
				.SingleOrDefault(x => x.Logins
															.Where(l => String.Equals(l.ProviderKey, providerKey, StringComparison.CurrentCultureIgnoreCase))
															.Any(l => String.Equals(l.LoginProvider, providerName, StringComparison.CurrentCultureIgnoreCase))
												);

			if (user != null)
				return user.UserName;

			return "";
		}

		private MongoCollection<AspNetUser> MongoCollection()
		{
			var collection = this.LocalDatabase.GetCollection<AspNetUser>("AspNetUsers");
			return collection;
		}

		private MongoServer _localServer;
		private MongoDatabase _localDatabase;
		private MongoDatabase LocalDatabase
		{
			get
			{
				if (_localServer == null)
				{
					var mongoServer = this._entityConfiguration.MongoServer;
					_localServer = MongoServer.Create(mongoServer);
				}

				if (_localDatabase == null)
				{
					_localDatabase = _localServer.GetDatabase("tekconf");

				}
				return _localDatabase;
			}
		}
	}
}