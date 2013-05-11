using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using TekConf.Common.Entities;
using TinyMessenger;

namespace TekConf.Common.Entities
{
	public class ConferenceEntity : ISupportInitialize, IEntity
	{
		private readonly ITinyMessengerHub _hub;
		private readonly IConferenceRepository _repository;
		private bool _isInitializingFromBson;
		private readonly List<ConferenceCreatedMessage> _conferenceCreatedMessages;
		private readonly List<ConferenceStartDateChangedMessage> _conferenceStartDateChangedMessages;
		private readonly List<ConferenceEndDateChangedMessage> _conferenceEndDateChangedMessages;
		private readonly List<ConferenceLocationChangedMessage> _conferenceLocationChangedMessages;
		private readonly List<ConferencePublishedMessage> _conferencePublishedMessages;
		private readonly List<ConferenceUpdatedMessage> _conferenceUpdatedMessages;
		private readonly List<SessionAddedMessage> _sessionAddedMessages;
		private readonly List<SessionRemovedMessage> _sessionRemovedMessages;
		private readonly List<SpeakerAddedMessage> _speakerAddedMessages;
		private readonly List<SpeakerRemovedMessage> _speakerRemovedMessages;
		private readonly List<SessionRoomChangedMessage> _sessionRoomChangedMessages;
		private readonly List<SessionStartDateChangedMessage> _sessionStartDateChangedMessages;
		private readonly List<SessionEndDateChangedMessage> _sessionEndDateChangedMessages;

		public ConferenceEntity(ITinyMessengerHub hub, IConferenceRepository repository)
		{
			_hub = hub;
			_repository = repository;

			_conferenceStartDateChangedMessages = new List<ConferenceStartDateChangedMessage>();
			_conferenceCreatedMessages = new List<ConferenceCreatedMessage>();
			_conferenceEndDateChangedMessages = new List<ConferenceEndDateChangedMessage>();
			_conferenceLocationChangedMessages = new List<ConferenceLocationChangedMessage>();
			_conferencePublishedMessages = new List<ConferencePublishedMessage>();
			_conferenceUpdatedMessages = new List<ConferenceUpdatedMessage>();
			_sessionAddedMessages = new List<SessionAddedMessage>();
			_sessionRemovedMessages = new List<SessionRemovedMessage>();
			_sessionRoomChangedMessages = new List<SessionRoomChangedMessage>();
			_sessionStartDateChangedMessages = new List<SessionStartDateChangedMessage>();
			_sessionEndDateChangedMessages = new List<SessionEndDateChangedMessage>();
			_speakerAddedMessages = new List<SpeakerAddedMessage>();
			_speakerRemovedMessages = new List<SpeakerRemovedMessage>();
		}

		[BsonId(IdGenerator = typeof(CombGuidGenerator))]
		public Guid _id { get; set; }
		public bool isLive { get; private set; }
		public double[] position { get; set; }
		public string slug { get; set; }
		public DateTime datePublished { get; private set; }
		public bool isSaved { get; private set; }
		public int defaultTalkLength { get; set; }
		public List<string> rooms { get; set; }
		public List<string> sessionTypes { get; set; } 

		public string name
		{
			get { return _name; }
			set { _name = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		private DateTime? _start;
		public DateTime? start
		{
			get { return _start; }
			set
			{
				if (!_isInitializingFromBson && isSaved && _start != value)
				{
					_conferenceStartDateChangedMessages.Add(new ConferenceStartDateChangedMessage() { ConferenceSlug = this.slug, ConferenceName = this.name, OldValue = _start, NewValue = value });
				}

				_start = value;
			}
		}

		private DateTime? _end;
		public DateTime? end
		{
			get { return _end; }
			set
			{
				if (!_isInitializingFromBson && isSaved && _end != value)
					_conferenceEndDateChangedMessages.Add(new ConferenceEndDateChangedMessage() { ConferenceName = this.name, ConferenceSlug = this.slug, OldValue = _end, NewValue = value });

				_end = value;
			}
		}

		public DateTime callForSpeakersOpens { get; set; }
		public DateTime callForSpeakersCloses { get; set; }
		public DateTime registrationOpens { get; set; }
		public DateTime registrationCloses { get; set; }
		public DateTime dateAdded { get; set; }
		private string _location;
		public string location
		{
			get { return _location; }
			set
			{
				if (!_isInitializingFromBson && isSaved && _location != value)
					_conferenceLocationChangedMessages.Add(new ConferenceLocationChangedMessage() { ConferenceSlug = this.slug, ConferenceName = this.name, OldValue = _location, NewValue = value });

				_location = value.IsNullOrWhiteSpace() ? value : value.Trim();
			}
		}

		public bool? isOnline { get; set; }
		public AddressEntity address { get; set; }
		public string description
		{
			get { return _description; }
			set { _description = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string imageUrl
		{
			get { return _imageUrl; }
			set { _imageUrl = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string tagLine
		{
			get { return _tagLine; }
			set { _tagLine = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string facebookUrl
		{
			get { return _facebookUrl; }
			set { _facebookUrl = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string homepageUrl
		{
			get { return _homepageUrl; }
			set { _homepageUrl = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string lanyrdUrl
		{
			get { return _lanyrdUrl; }
			set { _lanyrdUrl = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string twitterHashTag
		{
			get { return _twitterHashTag; }
			set { _twitterHashTag = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string twitterName
		{
			get { return _twitterName; }
			set
			{
				if (!string.IsNullOrWhiteSpace(value) && !value.StartsWith("@"))
					value = "@" + value;

				_twitterName = value.IsNullOrWhiteSpace() ? value : value.Trim();
			}
		}

		public string meetupUrl
		{
			get { return _meetupUrl; }
			set { _meetupUrl = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string googlePlusUrl
		{
			get { return _googlePlusUrl; }
			set { _googlePlusUrl = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string vimeoUrl
		{
			get { return _vimeoUrl; }
			set { _vimeoUrl = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string youtubeUrl
		{
			get { return _youtubeUrl; }
			set { _youtubeUrl = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string githubUrl
		{
			get { return _githubUrl; }
			set { _githubUrl = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string linkedInUrl
		{
			get { return _linkedInUrl; }
			set { _linkedInUrl = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		[BsonIgnore]
		public double distance { get; set; }
		private IList<SessionEntity> _sessions = new List<SessionEntity>();
		public IEnumerable<SessionEntity> sessions
		{
			get
			{
				if (_sessions == null) _sessions = new List<SessionEntity>();
				return _sessions.AsEnumerable();
			}
			set
			{
				if (value == null)
					value = new List<SessionEntity>();

				_sessions = value.ToList();
				foreach (var session in _sessions)
				{
					session.RoomChanged += SessionRoomChangedHandler;
					session.StartDateChanged += SessionStartDateChangedHandler;
					session.EndDateChanged += SessionEndDateChangedHandler;
					
				}
			}
		}

		private IList<string> _tags = new List<string>();
		public IEnumerable<string> tags
		{
			get
			{
				return _tags.AsEnumerable();
			}
			set
			{
				if (value == null)
					value = new List<string>();

				_tags = value.ToList();
			}
		}

		private IList<string> _subjects = new List<string>();
		private string _twitterName;
		private string _name;
		private string _description;
		private string _imageUrl;
		private string _tagLine;
		private string _facebookUrl;
		private string _homepageUrl;
		private string _lanyrdUrl;
		private string _twitterHashTag;
		private string _meetupUrl;
		private string _googlePlusUrl;
		private string _vimeoUrl;
		private string _youtubeUrl;
		private string _githubUrl;
		private string _linkedInUrl;

		public IEnumerable<string> subjects
		{
			get { return _subjects.AsEnumerable(); }
			set
			{
				if (value == null)
					value = new List<string>();

				_subjects = value.ToList();
			}
		}

		private void SessionRoomChangedHandler(SessionEntity sessionEntity, RoomChangedArgs e)
		{
			if (!_isInitializingFromBson && isSaved)
				_sessionRoomChangedMessages.Add(new SessionRoomChangedMessage() { ConferenceSlug = this.slug, SessionSlug = e.SessionSlug, SessionTitle = sessionEntity.title, OldValue = e.OldValue, NewValue = e.NewValue });
		}

		private void SessionStartDateChangedHandler(SessionEntity sessionEntity, StartDateChangedArgs e)
		{
			if (!_isInitializingFromBson && isSaved)
				_sessionStartDateChangedMessages.Add(new SessionStartDateChangedMessage() { ConferenceSlug = this.slug, SessionSlug = e.SessionSlug, SessionTitle = sessionEntity.title, OldValue = e.OldValue, NewValue = e.NewValue });
		}

		private void SessionEndDateChangedHandler(SessionEntity sessionEntity, EndDateChangedArgs e)
		{
			if (!_isInitializingFromBson && isSaved)
				_sessionEndDateChangedMessages.Add(new SessionEndDateChangedMessage() { ConferenceSlug = this.slug, SessionSlug = e.SessionSlug, SessionTitle = sessionEntity.title, OldValue = e.OldValue, NewValue = e.NewValue });
		}


		public void Save()
		{
			bool isNew = false;
			if (!this.isSaved)
			{
				if (_id == default(Guid))
				{
					_id = Guid.NewGuid();
					isNew = true;
				}

				dateAdded = DateTime.Now;
				isSaved = true;
			}
			slug = name.GenerateSlug();
			_repository.Save(this);

			if (isNew)
			{
				_conferenceCreatedMessages.Add(new ConferenceCreatedMessage() { ConferenceSlug = this.slug, ConferenceName = this.name });
			}
			else
			{
				_conferenceUpdatedMessages.Add(new ConferenceUpdatedMessage() { ConferenceSlug = this.slug, ConferenceName = this.name });
			}

			PublishEvents();
		}

		private void PublishEvents()
		{
			foreach (var message in _conferenceStartDateChangedMessages)
			{
				_hub.Publish(message);
			}
			_conferenceStartDateChangedMessages.Clear();

			foreach (var message in _conferenceCreatedMessages)
			{
				_hub.Publish(message);
			}
			_conferenceCreatedMessages.Clear();

			foreach (var message in _conferenceEndDateChangedMessages)
			{
				_hub.Publish(message);
			}
			_conferenceEndDateChangedMessages.Clear();

			foreach (var message in _conferenceLocationChangedMessages)
			{
				_hub.Publish(message);
			}
			_conferenceLocationChangedMessages.Clear();

			foreach (var message in _conferencePublishedMessages)
			{
				_hub.Publish(message);
			}
			_conferencePublishedMessages.Clear();

			foreach (var message in _conferenceUpdatedMessages)
			{
				_hub.Publish(message);
			}
			_conferenceUpdatedMessages.Clear();

			foreach (var message in _sessionAddedMessages)
			{
				_hub.Publish(message);
			}
			_sessionAddedMessages.Clear();

			foreach (var message in _sessionRemovedMessages)
			{
				_hub.Publish(message);
			}
			_sessionRemovedMessages.Clear();

			foreach (var message in _sessionRoomChangedMessages)
			{
				_hub.Publish(message);
			}
			_sessionRoomChangedMessages.Clear();

			foreach (var message in _sessionStartDateChangedMessages)
			{
				_hub.Publish(message);
			}
			_sessionStartDateChangedMessages.Clear();

			foreach (var message in _sessionEndDateChangedMessages)
			{
				_hub.Publish(message);
			}
			_sessionEndDateChangedMessages.Clear();

			foreach (var message in _speakerAddedMessages)
			{
				_hub.Publish(message);
			}
			_speakerAddedMessages.Clear();

			foreach (var message in _speakerRemovedMessages)
			{
				_hub.Publish(message);
			}
			_speakerRemovedMessages.Clear();
		}

		public void Publish()
		{
			this.datePublished = DateTime.Now;
			this.isLive = true;

			_conferencePublishedMessages.Add(new ConferencePublishedMessage() { ConferenceName = this.name, ConferenceSlug = this.slug });
		}

		public void AddSession(SessionEntity session)
		{
			if (_sessions == null)
				_sessions = new List<SessionEntity>();

			session.RoomChanged += SessionRoomChangedHandler;
			session.StartDateChanged += SessionStartDateChangedHandler;
			session.EndDateChanged += SessionEndDateChangedHandler;

			_sessions.Add(session);

			if (!_isInitializingFromBson && isSaved)
				_sessionAddedMessages.Add(new SessionAddedMessage() { SessionSlug = session.slug, SessionTitle = session.title });
		}

		public void RemoveSession(SessionEntity session)
		{
			if (_sessions == null)
			{
				_sessions = new List<SessionEntity>();
			}
			_sessions.Remove(session);
			if (!_isInitializingFromBson && isSaved)
				_sessionRemovedMessages.Add(new SessionRemovedMessage() { ConferenceName = this.name, SessionSlug = session.slug, SessionTitle = session.title });
		}

		public void AddSpeakerToSession(string sessionSlug, SpeakerEntity speaker)
		{
			if (this.sessions == null)
				throw new ArgumentException("Cannot add speaker to session. Conference " + slug + " has no sessions.");

			var session = this.sessions.SingleOrDefault(s => s.slug == sessionSlug);

			if (session == null)
				throw new ArgumentException("Cannot add speaker to session. Conference : " + slug + " Session:" + sessionSlug);

			session.AddSpeaker(speaker);

			if (!_isInitializingFromBson && isSaved)
				_speakerAddedMessages.Add(new SpeakerAddedMessage() { SessionSlug = sessionSlug, SessionTitle = session.title, SpeakerSlug = speaker.slug, SpeakerName = speaker.fullName });
		}

		public void RemoveSpeakerFromSession(string sessionSlug, SpeakerEntity speaker)
		{
			if (this.sessions == null)
				return;

			var session = this.sessions.SingleOrDefault(s => s.slug == sessionSlug);

			if (session == null)
				return;

			session.RemoveSpeaker(speaker);

			if (!_isInitializingFromBson && isSaved)
				_speakerRemovedMessages.Add(new SpeakerRemovedMessage() { SessionSlug = sessionSlug, SessionTitle = session.title, SpeakerSlug = speaker.slug, SpeakerName = speaker.fullName });
		}

		public void BeginInit()
		{
			_isInitializingFromBson = true;
		}

		public void EndInit()
		{
			_isInitializingFromBson = false;
		}
	}
}