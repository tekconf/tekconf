using System;
using System.Linq;
using TekConf.Common.Entities;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api.Services;
using TekConf.UI.Api.Services.Requests.v1;
using TinyMessenger;

namespace TekConf.UI.Api.v1
{
	public class SubscriptionService : MongoServiceBase
	{
		private readonly ITinyMessengerHub _hub;
		private readonly IRepository<SubscriptionEntity> _subscriptionRepository;

		public SubscriptionService(ITinyMessengerHub hub, IRepository<SubscriptionEntity> subscriptionRepository)
		{
			_hub = hub;
			_subscriptionRepository = subscriptionRepository;
		}

		public object Get(Subscription request)
		{
			var dto = new SubscriptionDto();

			if (!string.IsNullOrWhiteSpace(request.emailAddress))
			{
				var subscriptionExists = _subscriptionRepository.AsQueryable().Any(x => x.EmailAddress == request.emailAddress.Trim());

				if (subscriptionExists)
					dto.EmailAddress = request.emailAddress;
			}

			return dto;
		}

		public object Post(CreateSubscription request)
		{
			var dto = new SubscriptionDto();

			if (!string.IsNullOrWhiteSpace(request.EmailAddress))
			{
				if (!_subscriptionRepository.AsQueryable().Any(x => x.EmailAddress == request.EmailAddress.Trim()))
				{
					var subscriptionEntity = new SubscriptionEntity() {_id = Guid.NewGuid(), EmailAddress = request.EmailAddress};
					_subscriptionRepository.Save(subscriptionEntity);
				}

				dto.EmailAddress = request.EmailAddress;
			}

			return dto;
		}

		public object Put(CreateSubscription request)
		{
			var dto = new SubscriptionDto();

			if (!string.IsNullOrWhiteSpace(request.EmailAddress))
			{
				if (!_subscriptionRepository.AsQueryable().Any(x => x.EmailAddress == request.EmailAddress.Trim()))
				{
					var subscriptionEntity = new SubscriptionEntity() { _id = Guid.NewGuid(), EmailAddress = request.EmailAddress };
					_subscriptionRepository.Save(subscriptionEntity);
				}

				dto.EmailAddress = request.EmailAddress;
			}

			return dto;
		}
	}
}