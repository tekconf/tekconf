using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TekConf.Common.Entities
{
	public class SessionEntity : IEntity
	{
		public event RoomChangedHandler RoomChanged;
		public event StartDateChangedHandler StartDateChanged;
		public event EndDateChangedHandler EndDateChanged;

		public delegate void RoomChangedHandler(SessionEntity m, RoomChangedArgs e);
		public delegate void StartDateChangedHandler(SessionEntity m, StartDateChangedArgs e);
		public delegate void EndDateChangedHandler(SessionEntity m, EndDateChangedArgs e);

		[BsonId(IdGenerator = typeof(CombGuidGenerator))]
		public Guid _id { get; set; }
		public string slug { get; set; }
		public string title
		{
			get { return _title; }
			set { _title = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		private DateTime _startDate;
		public DateTime start 
		{
			get { return _startDate; }
			set
			{
				if (_startDate != value)
				{
					if (StartDateChanged != null)
					{
						var args = new StartDateChangedArgs(this.slug, _startDate, value);

						StartDateChanged(this, args);
					}
				}
				_startDate = value;
			} 
		}

		private DateTime _endDate;
		public DateTime end
		{
			get { return _endDate; }
			set
			{
				if (_endDate != value)
				{
					if (EndDateChanged != null)
					{
						var args = new EndDateChangedArgs(this.slug, _startDate, value);

						EndDateChanged(this, args);
					}
				}
				_endDate = value;
			}
		}

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
						var roomChanged = new RoomChangedArgs(this.slug, _room, value);

						RoomChanged(this, roomChanged);
					}
				}
				_room = value.IsNullOrWhiteSpace() ? value : value.Trim();
			}
		}

		public string difficulty
		{
			get { return _difficulty; }
			set { _difficulty = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string description
		{
			get { return _description; }
			set { _description = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string twitterHashTag
		{
			get { return _twitterHashTag; }
			set { _twitterHashTag = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

		public string sessionType
		{
			get { return _sessionType; }
			set { _sessionType = value.IsNullOrWhiteSpace() ? value : value.Trim(); }
		}

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
		private string _title;
		private string _difficulty;
		private string _description;
		private string _twitterHashTag;
		private string _sessionType;

		public IEnumerable<SpeakerEntity> speakers
		{
			get
			{
					if (_speakers == null) _speakers = new List<SpeakerEntity>();
				return _speakers.AsEnumerable();
			}
			set
			{
				if (value == null)
					value = new List<SpeakerEntity>();

				_speakers = value.ToList();
			}
		}
	}
}