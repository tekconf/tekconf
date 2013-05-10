using System.Linq;
using System.Net;
using System.Web.Security;
using AutoMapper;
using TekConf.Common.Entities;
using TekConf.UI.Api.Services.Requests.v1;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;

namespace TekConf.UI.Api.Services.v1
{
	public class UserService : MongoServiceBase
	{
		private readonly IRepository<UserEntity> _userRepository;

		public ICacheClient CacheClient { get; set; }

		public UserService(IRepository<UserEntity> userRepository)
		{
			_userRepository = userRepository;
		}

		public object Get(User request)
		{
			if (request.IsNull() || request.userName == default(string))
			{
				return new HttpError() { StatusCode = HttpStatusCode.BadRequest, StatusDescription = "UserName is required." };

			}
			else
			{
				var user3 = Membership.FindUsersByName(request.userName);
				return null;
			}
		}

		public object Post(User request)
		{
			var userExists = _userRepository
				.AsQueryable().Any(u => u.userName.ToLower() == request.userName.ToLower());

			if (userExists)
			{
				return new HttpResult()
				{
					StatusCode = HttpStatusCode.NotModified
				};
			}
			else
			{
				var user = Mapper.Map<UserEntity>(request);

				_userRepository.Save(user);
				return new HttpResult() { StatusCode = HttpStatusCode.Created };
			}
			this.CacheClient.FlushAll();
		}
	}
}