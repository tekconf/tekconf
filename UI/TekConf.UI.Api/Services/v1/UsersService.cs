using System;
using System.Linq;
using System.Net;
using System.Web.Security;
using TekConf.UI.Api.Services.Requests.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;

namespace TekConf.UI.Api.Services.v1
{
    public class UserService : MongoServiceBase
    {
        public ICacheClient CacheClient { get; set; }

        public object Get(User request)
        {
            if (request == null || request.userName == default(string))
            {
                return new HttpError()
                           {StatusCode = HttpStatusCode.BadRequest, StatusDescription = "UserName is required."};

            }
            else
            {
                var user3 = Membership.FindUsersByName(request.userName);
                return null;
                //var collection = this.RemoteDatabase.GetCollection<UserEntity>("ASPNETDB");
                //var user = collection.AsQueryable().FirstOrDefault(u => u.userName == request.userName);
                //if (user == null)
                //{
                //    return new HttpError() {StatusCode = HttpStatusCode.BadRequest, StatusDescription = "User not found"};
                //}
                //else
                //{
                //    return user;
                //}
            }
        }

        public object Post(User request)
        {
            var collection = this.RemoteDatabase.GetCollection<UserEntity>("users");
            var userExists = collection
                .AsQueryable().Any(u => u.userName == request.userName);
            
            if (userExists)
            {
                return new HttpResult()
                {
                    StatusCode = HttpStatusCode.NotModified
                };
            }
            else
            {
                var user = new UserEntity()
                               {
                                   _id = Guid.NewGuid(),
                                   userName = request.userName,
                               };
                collection.Save(user);
                return new HttpResult() {StatusCode = HttpStatusCode.Created};
            }
        }
    }
}