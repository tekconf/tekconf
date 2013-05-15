using System;
using Microsoft.AspNet.SignalR;
using PushSharp;
using PushSharp.WindowsPhone;
using TekConf.Common.Entities.Messages;
using TinyMessenger;

namespace TekConf.UI.Api
{
	using System.Linq;

	using TekConf.Common.Entities;

	public class HubSubscriptions
	{
		private readonly ITinyMessengerHub _hub;
		private readonly IRepository<SessionRoomChangedMessage> _sessionRoomChangedRepository;
		private readonly IRepository<ConferenceLocationChangedMessage> _conferenceLocationChangedRepository;
		private readonly IRepository<ConferenceEndDateChangedMessage> _conferenceEndDateChangedRepository;
		private readonly IRepository<ConferencePublishedMessage> _conferencePublishedRepository;
		private readonly IRepository<ConferenceUpdatedMessage> _conferenceUpdatedRepository;
		private readonly IRepository<ConferenceStartDateChangedMessage> _conferenceStartDateChangedRepository;
		private readonly IRepository<SessionAddedMessage> _sessionAddedRepository;
		private readonly IRepository<SessionRemovedMessage> _sessionRemovedRepository;
		private readonly IRepository<SpeakerAddedMessage> _speakerAddedRepository;
		private readonly IRepository<SpeakerRemovedMessage> _speakerRemovedRepository;
		private readonly IRepository<ConferenceCreatedMessage> _conferenceCreatedRepository;
		private readonly IRepository<ScheduleCreatedMessage> _scheduleCreatedRepository;
		private readonly IRepository<SessionAddedToScheduleMessage> _sessionAddedToScheduleRepository;
		private readonly IRepository<SessionStartDateChangedMessage> _sessionStartDateChangedRepository;
		private readonly IRepository<SessionEndDateChangedMessage> _sessionEndDateChangedRepository;

		private readonly IRepository<SubscriptionEntity> _subscriptionRepository;
		private readonly IRepository<UserEntity> _userRepository;
		private readonly IRepository<ScheduleEntity> _scheduleRepository;

		private readonly IEmailSender _emailSender;
		private readonly IEntityConfiguration _entityConfiguration;

		public HubSubscriptions(ITinyMessengerHub hub,
																IRepository<SessionRoomChangedMessage> sessionRoomChangedRepository,
																IRepository<ConferenceLocationChangedMessage> conferenceLocationChangedRepository,
																IRepository<ConferenceEndDateChangedMessage> conferenceEndDateChangedRepository,
																IRepository<ConferencePublishedMessage> conferencePublishedRepository,
																IRepository<ConferenceUpdatedMessage> conferenceUpdatedRepository,
																IRepository<ConferenceStartDateChangedMessage> conferenceStartDateChangedRepository,
																IRepository<SessionAddedMessage> sessionAddedRepository,
																IRepository<SessionRemovedMessage> sessionRemovedRepository,
																IRepository<SpeakerAddedMessage> speakerAddedRepository,
																IRepository<SpeakerRemovedMessage> speakerRemovedRepository,
																IRepository<ConferenceCreatedMessage> conferenceCreatedRepository,
																IRepository<ScheduleCreatedMessage> scheduleCreatedRepository,
																IRepository<SessionAddedToScheduleMessage> sessionAddedToScheduleRepository,
																IRepository<SessionStartDateChangedMessage> sessionStartDateChangedRepository,
																IRepository<SessionEndDateChangedMessage> sessionEndDateChangedRepository,
																IRepository<SubscriptionEntity> subscriptionRepository,
																IRepository<UserEntity> userRepository,
																IRepository<ScheduleEntity> scheduleRepository,
																IEmailSender emailSender,
																IEntityConfiguration entityConfiguration
														)
		{
			_hub = hub;

			_sessionRoomChangedRepository = sessionRoomChangedRepository;
			_sessionAddedRepository = sessionAddedRepository;
			_conferencePublishedRepository = conferencePublishedRepository;

			_conferenceLocationChangedRepository = conferenceLocationChangedRepository;
			_conferenceEndDateChangedRepository = conferenceEndDateChangedRepository;
			_conferenceUpdatedRepository = conferenceUpdatedRepository;
			_conferenceStartDateChangedRepository = conferenceStartDateChangedRepository;
			_sessionRemovedRepository = sessionRemovedRepository;
			_speakerAddedRepository = speakerAddedRepository;
			_speakerRemovedRepository = speakerRemovedRepository;
			_conferenceCreatedRepository = conferenceCreatedRepository;
			_scheduleCreatedRepository = scheduleCreatedRepository;
			_sessionAddedToScheduleRepository = sessionAddedToScheduleRepository;
			_sessionStartDateChangedRepository = sessionStartDateChangedRepository;
			_sessionEndDateChangedRepository = sessionEndDateChangedRepository;
			_subscriptionRepository = subscriptionRepository;
			_userRepository = userRepository;
			_scheduleRepository = scheduleRepository;
			_emailSender = emailSender;
			this._entityConfiguration = entityConfiguration;

			Subscribe();
		}

		private void Subscribe()
		{
			SubscribeToSessionRoomChangedEvent();
			SubscribeToSessionAdded();
			SubscribeToConferencePublished();
			SubscribeToConferenceLocationChanged();
			SubscribeToConferenceEndDateChanged();
			SubscribeToConferenceUpdated();
			SubscribeToConferenceStartDateChanged();
			SubscribeToSessionRemoved();
			SubscribeToSessionStartDateChanged();
			SubscribeToSessionEndDateChanged();
			SubscribeToSpeakerAdded();
			SubscribeToSpeakerRemoved();
			SubscribeToConferenceCreated();
			SubscribeToScheduleCreated();
			SubscribeToSessionAddedToSchedule();
		}

		public void SubscribeToScheduleCreated()
		{
			_hub.Subscribe<ScheduleCreatedMessage>((@event) =>
			{
				_scheduleCreatedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.UserName + " created a schedule for " + @event.ConferenceSlug;
				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		public void SubscribeToSessionAddedToSchedule()
		{
			_hub.Subscribe<SessionAddedToScheduleMessage>((@event) =>
			{
				_sessionAddedToScheduleRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.UserName + " added " + @event.SessionSlug + " to their schedule for " + @event.ConferenceSlug;
				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		public void SubscribeToSessionStartDateChanged()
		{
			_hub.Subscribe<SessionStartDateChangedMessage>((@event) =>
			{
				_sessionStartDateChangedRepository.Save(@event);

				var message = @event.SessionTitle + " start date has changed to " + @event.NewValue.ToShortTimeString();

				SendWindowsPhonePushNotifications(@event, message);

				_emailSender.Send(message);
			});
		}

		public void SubscribeToSessionEndDateChanged()
		{
			_hub.Subscribe<SessionEndDateChangedMessage>((@event) =>
			{
				_sessionEndDateChangedRepository.Save(@event);
				var message = @event.SessionTitle + " end date has changed to " + @event.NewValue.ToShortTimeString();
				SendWindowsPhonePushNotifications(@event, message);
				_emailSender.Send(message);
			});
		}

		private void SubscribeToSpeakerRemoved()
		{
			_hub.Subscribe<SpeakerRemovedMessage>((@event) =>
			{
				_speakerRemovedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.SpeakerName + " has been removed from " + @event.SessionTitle;
				context.Clients.All.broadcastMessage(message);

				//_emailSender.Send(message);
			});
		}

		private void SubscribeToSpeakerAdded()
		{
			_hub.Subscribe<SpeakerAddedMessage>((@event) =>
			{
				_speakerAddedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.SpeakerName + " has been added to " + @event.SessionTitle;
				context.Clients.All.broadcastMessage(message);

				//_emailSender.Send(message);
			});
		}

		private void SubscribeToSessionRemoved()
		{
			_hub.Subscribe<SessionRemovedMessage>((@event) =>
			{
				_sessionRemovedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.SessionTitle + " has been removed from " + @event.ConferenceName;
				context.Clients.All.broadcastMessage(message);

				//_emailSender.Send(message);
			});
		}

		private void SubscribeToConferenceStartDateChanged()
		{
			_hub.Subscribe<ConferenceStartDateChangedMessage>((@event) =>
			{
				_conferenceStartDateChangedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.ConferenceName + " start date has changed from "
																+ (@event.OldValue.HasValue ? @event.OldValue.Value.ToString() : "(not set)")
																+ " to "
																+ (@event.NewValue.HasValue ? @event.NewValue.Value.ToString() : "(not set)");
				context.Clients.All.broadcastMessage(message);

				//_emailSender.Send(message);
			});
		}

		private void SubscribeToConferenceUpdated()
		{
			_hub.Subscribe<ConferenceUpdatedMessage>((@event) =>
			{
				_conferenceUpdatedRepository.Save(@event);

				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.ConferenceName + " has been updated.";
				context.Clients.All.broadcastMessage(message);


				SendWindowsPhonePushNotifications(@event, message);

				_emailSender.Send(message);
			});
		}

		private void SendWindowsPhonePushNotifications(ConferenceUpdatedMessage @event, string message)
		{
			var push = new PushBroker();
			push.RegisterWindowsPhoneService();

			var windowsPhoneUsers = _userRepository.AsQueryable().ToList().Where(x => x.WindowsPhoneEndpointUris.Any()).ToList();
			var schedules = _scheduleRepository.AsQueryable().Where(x => x.ConferenceSlug == @event.ConferenceSlug).ToList();

			var notifications = windowsPhoneUsers.Where(wpu => schedules.Select(s => s.UserName).Contains(wpu.userName)).ToList();

			foreach (var user in notifications)
			{
				foreach (var endpoint in user.WindowsPhoneEndpointUris)
				{
					push.QueueNotification(new WindowsPhoneToastNotification()
						.ForEndpointUri(new Uri(endpoint))
						.ForOSVersion(WindowsPhoneDeviceOSVersion.Eight)
						.WithBatchingInterval(BatchingInterval.Immediate)
						.WithNavigatePath("~/Views/ConferenceDetailView.xaml")
						.WithParameter("slug", @event.ConferenceSlug)
						.WithText1("TekConf")
						.WithText2(message));
				}
			}
		}

		private void SendWindowsPhonePushNotifications(SessionStartDateChangedMessage @event, string message)
		{
			var push = new PushBroker();
			push.RegisterWindowsPhoneService();

			var windowsPhoneUsers = _userRepository.AsQueryable().ToList().Where(x => x.WindowsPhoneEndpointUris.Any()).ToList();
			var schedules = _scheduleRepository.AsQueryable()
				.Where(x => x.ConferenceSlug == @event.ConferenceSlug)
				.Where(x => x.SessionSlugs.Contains(@event.SessionSlug))
				.ToList();

			var notifications = windowsPhoneUsers.Where(wpu => schedules.Select(s => s.UserName).Contains(wpu.userName)).ToList();

			foreach (var user in notifications)
			{
				foreach (var endpoint in user.WindowsPhoneEndpointUris)
				{
					push.QueueNotification(new WindowsPhoneToastNotification()
						.ForEndpointUri(new Uri(endpoint))
						.ForOSVersion(WindowsPhoneDeviceOSVersion.Eight)
						.WithBatchingInterval(BatchingInterval.Immediate)
						.WithNavigatePath("~/Views/SessionDetailView.xaml")
						.WithParameter("ConferenceSlug", @event.ConferenceSlug)
						.WithParameter("SessionSlug", @event.SessionSlug)
						.WithText1("TekConf")
						.WithText2(message));

					//push.QueueNotification(new WindowsPhoneTileNotification()
					//	.ForEndpointUri(new Uri(endpoint))
					//	.ForOSVersion(WindowsPhoneDeviceOSVersion.Eight)
					//	.WithBatchingInterval(BatchingInterval.Immediate)
					//	.WithTitle("HI THERE"));
				}
			}
		}

		private void SendWindowsPhonePushNotifications(SessionEndDateChangedMessage @event, string message)
		{
			var push = new PushBroker();
			push.RegisterWindowsPhoneService();

			var windowsPhoneUsers = _userRepository.AsQueryable().ToList().Where(x => x.WindowsPhoneEndpointUris.Any()).ToList();
			var schedules = _scheduleRepository.AsQueryable()
				.Where(x => x.ConferenceSlug == @event.ConferenceSlug)
				.Where(x => x.SessionSlugs.Contains(@event.SessionSlug))
				.ToList();

			var notifications = windowsPhoneUsers.Where(wpu => schedules.Select(s => s.UserName).Contains(wpu.userName)).ToList();

			foreach (var user in notifications)
			{
				foreach (var endpoint in user.WindowsPhoneEndpointUris)
				{
					push.QueueNotification(new WindowsPhoneToastNotification()
						.ForEndpointUri(new Uri(endpoint))
						.ForOSVersion(WindowsPhoneDeviceOSVersion.Eight)
						.WithBatchingInterval(BatchingInterval.Immediate)
						.WithNavigatePath("~/Views/SessionDetailView.xaml")
						.WithParameter("ConferenceSlug", @event.ConferenceSlug)
						.WithParameter("SessionSlug", @event.SessionSlug)
						.WithText1("TekConf")
						.WithText2(message));
				}
			}
		}

		private void SubscribeToConferenceCreated()
		{
			_hub.Subscribe<ConferenceCreatedMessage>((@event) =>
			{
				_conferenceCreatedRepository.Save(@event);

				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.ConferenceName + " has been created.";
				context.Clients.All.broadcastMessage(message);

				_emailSender.Send(message);
			});
		}

		private void SubscribeToConferenceEndDateChanged()
		{
			_hub.Subscribe<ConferenceEndDateChangedMessage>((@event) =>
			{
				_conferenceEndDateChangedRepository.Save(@event);
				//TODO: Only send to people that have this conference saved to their schedule
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.ConferenceName + " end date has changed from "
																+ (@event.OldValue.HasValue ? @event.OldValue.Value.ToString() : "(not set)")
																+ " to "
																+ (@event.NewValue.HasValue ? @event.NewValue.Value.ToString() : "(not set)");

				context.Clients.All.broadcastMessage(message);

				//_emailSender.Send(message);
			});
		}

		private void SubscribeToConferenceLocationChanged()
		{
			_hub.Subscribe<ConferenceLocationChangedMessage>((@event) =>
			{
				_conferenceLocationChangedRepository.Save(@event);
				var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
				var message = @event.ConferenceName + " location has changed from "
																+ (!string.IsNullOrWhiteSpace(@event.OldValue) ? @event.OldValue : "(not set)")
																+ " to "
																+ (!string.IsNullOrWhiteSpace(@event.NewValue) ? @event.NewValue : "(not set)");
				context.Clients.All.broadcastMessage(message);

				//_emailSender.Send(message);
			});
		}

		private void SubscribeToSessionRoomChangedEvent()
		{
			_hub.Subscribe<SessionRoomChangedMessage>((@event) =>
							{
								_sessionRoomChangedRepository.Save(@event);
								var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
								var message = @event.SessionTitle + " room has changed from "
																				+ (!string.IsNullOrWhiteSpace(@event.OldValue) ? @event.OldValue : "(not set)")
																				+ " to "
																				+ (!string.IsNullOrWhiteSpace(@event.NewValue) ? @event.NewValue : "(not set)");
								context.Clients.All.broadcastMessage(message);

								//_emailSender.Send(message);
							});
		}

		private void SubscribeToSessionAdded()
		{
			_hub.Subscribe<SessionAddedMessage>((@event) =>
								{
									_sessionAddedRepository.Save(@event);

									var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
									var message = @event.SessionTitle + " has been added ";
									context.Clients.All.broadcastMessage(message);

									//_emailSender.Send(message);
								});
		}

		private void SubscribeToConferencePublished()
		{
			_hub.Subscribe<ConferencePublishedMessage>((@event) =>
							{
								// Add to community megaphone
								// Add to rss feed
								// Tweet
								_conferencePublishedRepository.Save(@event);
								var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
								var message = @event.ConferenceName + " has been published to TekConf.";
								context.Clients.All.broadcastMessage(message);

								//TODO : Format this better
								var subscriptions = _subscriptionRepository.AsQueryable().Select(x => x.EmailAddress).ToList();
								_emailSender.Send(message, subscriptions);
							});

		}
	}
}