using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using TinyMessenger;

namespace TekConf.UI.Api
{
	public class RoomChanged : EventArgs
	{
		public RoomChanged(string sessionSlug, string oldValue, string newValue)
		{
			this.SessionSlug = sessionSlug;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		public string SessionSlug { get; private set; }
		public string OldValue { get; private set; }
		public string NewValue { get; private set; }
	}

	public class SessionEntity
	{
		public event RoomChangedHandler RoomChanged;
		public delegate void RoomChangedHandler(SessionEntity m, RoomChanged e);

		[BsonId(IdGenerator = typeof(CombGuidGenerator))]
		public Guid _id { get; set; }
		public string slug { get; set; }
		public string title { get; set; }
		public DateTime start { get; set; }
		public DateTime end { get; set; }
		private string _room;
		public string room
		{
			get { return _room; }
			set
			{
				if (_room != value)
				{
					if (RoomChanged != null)
					{
						var roomChanged = new RoomChanged(this.slug, _room, value);

						RoomChanged(this, roomChanged);
					}
				}
				_room = value;
			}
		}

		public string difficulty { get; set; }
		public string description { get; set; }
		public string twitterHashTag { get; set; }
		public string sessionType { get; set; }
		public List<string> links { get; set; }
		public List<string> tags { get; set; }
		public List<string> subjects { get; set; }
		public List<string> resources { get; set; }
		public List<string> prerequisites { get; set; }

		internal void AddSpeaker(SpeakerEntity speaker)
		{
			if (_speakers == null)
				_speakers = new List<SpeakerEntity>();

			_speakers.Add(speaker);
		}

		internal void RemoveSpeaker(SpeakerEntity speaker)
		{
			if (_speakers == null)
				return;

			_speakers.Remove(speaker);
		}

		private IList<SpeakerEntity> _speakers = new List<SpeakerEntity>();
		public IEnumerable<SpeakerEntity> speakers
		{
			get { return _speakers.AsEnumerable(); }
			set
			{
				if (value == null)
					value = new List<SpeakerEntity>();

				_speakers = value.ToList();
			}
		}
	}
}