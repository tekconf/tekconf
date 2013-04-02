using System;
using Microsoft.AspNet.SignalR;
using TekConf.Common.Entities.Messages;
using TinyMessenger;

namespace TekConf.UI.Api
{
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
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;

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
																IEmailSender emailSender,
																IConfiguration configuration
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
			_emailSender = emailSender;
			_configuration = configuration;

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

				//_emailSender.Send(message);
			});
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
								var message = @event.ConferenceName + " has been published. ";
								context.Clients.All.broadcastMessage(message);

								//_emailSender.Send(message);
							});

		}
	}
}
