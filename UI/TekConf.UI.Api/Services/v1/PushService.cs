using System;
using System.Collections.Generic;
using System.Net;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using TekConf.Common.Entities;
using TekConf.UI.Api.Services.Requests.v1;
using System.Linq;

namespace TekConf.UI.Api.Services.v1
{
	public class PushService : MongoServiceBase
	{
		private readonly IRepository<UserEntity> _userRepository;
		public ICacheClient CacheClient { get; set; }

		public PushService(IRepository<UserEntity> userRepository)
		{
			_userRepository = userRepository;
		}

		public object Post(WindowsPhonePushRequest request)
		{
			var user = _userRepository.AsQueryable().FirstOrDefault(u => u.userName.ToLower() == request.userName.ToLower()) ??
				new UserEntity()
					{
						_id = Guid.NewGuid(),
						userName =  request.userName
					};

			if (user.WindowsPhoneEndpointUris == null)
				user.WindowsPhoneEndpointUris = new List<string>();

			if (user.WindowsPhoneEndpointUris.Contains(request.endpointUri))
			{
				return new HttpResult()
				{
					StatusCode = HttpStatusCode.NotModified
				};
			}

			user.WindowsPhoneEndpointUris.Add(request.endpointUri);
			_userRepository.Save(user);
			this.CacheClient.FlushAll();

			return new HttpResult()
			{
				StatusCode = HttpStatusCode.OK
			};
		}
	}
}