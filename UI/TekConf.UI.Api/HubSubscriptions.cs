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
		private readonly IRepository<ConferenceSavedMessage> _conferenceSavedRepository;
		private readonly IRepository<ConferenceStartDateChangedMessage> _conferenceStartDateChangedRepository;
		private readonly IRepository<SessionAddedMessage> _sessionAddedRepository;
		private readonly IRepository<SessionRemovedMessage> _sessionRemovedRepository;
		private readonly IRepository<SpeakerAddedMessage> _speakerAddedRepository;
		private readonly IRepository<SpeakerRemovedMessage> _speakerRemovedRepository;

		public HubSubscriptions(ITinyMessengerHub hub, 
																IRepository<SessionRoomChangedMessage> sessionRoomChangedRepository,
																IRepository<ConferenceLocationChangedMessage> conferenceLocationChangedRepository,
																IRepository<ConferenceEndDateChangedMessage> conferenceEndDateChangedRepository,
																IRepository<ConferencePublishedMessage> conferencePublishedRepository,
																IRepository<ConferenceSavedMessage> conferenceSavedRepository,
																IRepository<ConferenceStartDateChangedMessage> conferenceStartDateChangedRepository,
																IRepository<SessionAddedMessage> sessionAddedRepository,
																IRepository<SessionRemovedMessage> sessionRemovedRepository,
																IRepository<SpeakerAddedMessage> speakerAddedRepository,
																IRepository<SpeakerRemovedMessage> speakerRemovedRepository
														)
		{
			_hub = hub;

			_sessionRoomChangedRepository = sessionRoomChangedRepository;
			_sessionAddedRepository = sessionAddedRepository;
			_conferencePublishedRepository = conferencePublishedRepository;

			_conferenceLocationChangedRepository = conferenceLocationChangedRepository;
			_conferenceEndDateChangedRepository = conferenceEndDateChangedRepository;
			_conferenceSavedRepository = conferenceSavedRepository;
			_conferenceStartDateChangedRepository = conferenceStartDateChangedRepository;
			_sessionRemovedRepository = sessionRemovedRepository;
			_speakerAddedRepository = speakerAddedRepository;
			_speakerRemovedRepository = speakerRemovedRepository;

			Subscribe();
		}

		private void Subscribe()
		{
			SubscribeToSessionRoomChangedEvent();
			SubscribeToSessionAdded();
			SubscribeToConferencePublished();
			SubscribeToConferenceLocationChanged();
			SubscribeToConferenceEndDateChanged();
			SubscribeToConferenceSaved();
			SubscribeToConferenceStartDateChanged();
			SubscribeToSessionRemoved();
			SubscribeToSpeakerAdded();
			SubscribeToSpeakerRemoved();
		}

		private void SubscribeToSpeakerRemoved()
		{
			_hub.Subscribe<SpeakerRemovedMessage>((@event) =>
			{
				_speakerRemovedRepository.Save(@event);
			});
		}

		private void SubscribeToSpeakerAdded()
		{
			_hub.Subscribe<SpeakerAddedMessage>((@event) =>
			{
				_speakerAddedRepository.Save(@event);
			});
		}

		private void SubscribeToSessionRemoved()
		{
			_hub.Subscribe<SessionRemovedMessage>((@event) =>
			{
				_sessionRemovedRepository.Save(@event);
			});
		}

		private void SubscribeToConferenceStartDateChanged()
		{
			_hub.Subscribe<ConferenceStartDateChangedMessage>((@event) =>
			{
				_conferenceStartDateChangedRepository.Save(@event);
			});
		}

		private void SubscribeToConferenceSaved()
		{
			_hub.Subscribe<ConferenceSavedMessage>((@event) =>
			{
				_conferenceSavedRepository.Save(@event);
			});
		}

		private void SubscribeToConferenceEndDateChanged()
		{
			_hub.Subscribe<ConferenceEndDateChangedMessage>((@event) =>
			{
				_conferenceEndDateChangedRepository.Save(@event);
			});
		}

		private void SubscribeToConferenceLocationChanged()
		{
			_hub.Subscribe<ConferenceLocationChangedMessage>((@event) =>
			{
				_conferenceLocationChangedRepository.Save(@event);
			});
		}

		private void SubscribeToSessionRoomChangedEvent()
		{
			_hub.Subscribe<SessionRoomChangedMessage>((@event) =>
							{
								_sessionRoomChangedRepository.Save(@event);
							});
		}

		private void SubscribeToSessionAdded()
		{
			_hub.Subscribe<SessionAddedMessage>((@event) =>
																			 {
																				 _sessionAddedRepository.Save(@event);
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
															});

		}
	}
}