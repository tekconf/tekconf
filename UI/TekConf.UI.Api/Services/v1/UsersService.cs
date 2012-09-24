using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services.Requests.v1;
using TekConf.UI.Api.UrlResolvers.v1;
using FluentMongo.Linq;
using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;

namespace TekConf.UI.Api.Services.v1
{
    public class UsersService : MongoRestServiceBase<UsersRequest>
    {
        public ICacheClient CacheClient { get; set; }

        public override object OnPost(UsersRequest request)
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