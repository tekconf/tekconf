using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using FluentMongo.Linq;
using ServiceStack.Text;
using TekConf.Common.Entities;
using TinyMessenger;

namespace UberImporter.Importers.CodeMash2014EventBoard
{
	//
	//
	public class CodeMash2014EventBoardImporter
	{
		public void Import()
		{
			var allJsonPath = @"C:\Users\rob\AppData\Local\Packages\FalafelSoftwareInc.EventBoard_5hk9g6r40fw2m\LocalState";

			var eventBoardPath = Path.Combine(allJsonPath, "Conference_282.data.json");
			var eventBoardJson = File.ReadAllText(eventBoardPath);
			var eventBoardRoot = eventBoardJson.FromJson<eventBoardRoot>();

			if (eventBoardRoot != null)
			{
				var connection = new MongoDbConnection();
				var collection = connection.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
				var existingConf = collection.AsQueryable().FirstOrDefault(c => c.slug == "codemash-2014");
				ConferenceEntity conference = null;

				if (existingConf == null)
				{
					IEntityConfiguration entityConfiguration = new EntityConfiguration();
					IConferenceRepository conferenceRepository = new ConferenceRepository(entityConfiguration);
					ITinyMessengerHub hub = new TinyMessengerHub();
					conference = new ConferenceEntity(hub, conferenceRepository)
					{
						//_id = Guid.NewGuid(),
						start = new DateTime(2014, 1, 7),
						end = new DateTime(2014, 1, 10),
						description =
								@"CodeMash is a unique event that will educate developers on current practices, methodologies, and technology trends in a variety of platforms and development languages such as Java, .Net, Ruby, Python and PHP.",
						facebookUrl = "",
						homepageUrl = "http://codemash.org",
						imageUrl = @"http://tekconf.blob.core.windows.net/images/conferences/codemash-2014.jpg",
						lanyrdUrl = "",
						location = "Sandusky, OH",
						meetupUrl = "",
						name = "CodeMash 2014",
						//sessions = new List<SessionEntity>(),
						//slug = "CodeStock-2012",
						tagLine = "Get Your Gears On",
						twitterHashTag = "#codemash",
						twitterName = "@codemash",
						isLive = true
					};
				}
				else
				{
					conference = existingConf;
				}

				foreach (var session in eventBoardRoot.Sessions)
				{
					var slug = session.Name.GenerateSlug();
					SessionEntity sessionEntity = conference.sessions.SingleOrDefault(x => x.slug == slug);

					bool isNew = false;
					if (sessionEntity == null)
					{
						isNew = true;
						sessionEntity = new SessionEntity() { _id = Guid.NewGuid(), slug = slug };
					}

					sessionEntity.description = session.Description;
					sessionEntity.start = eventBoardRoot.TimeSlots.Single(x => x.ID == session.TimeSlotID).StartTime;
					sessionEntity.end = eventBoardRoot.TimeSlots.Single(x => x.ID == session.TimeSlotID).EndTime;
					sessionEntity.title = session.Name;
					if (session.LocationID != 0)
					{
						sessionEntity.room = eventBoardRoot.Rooms.Single(x => x.ID == session.LocationID).Name;
					}
					sessionEntity.twitterHashTag = "#codemash-" + session.Name.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10);
					sessionEntity.subjects = new List<string>() { };
					//sessionEntity.sessionType = session.SessionTypeID;
					sessionEntity.tags = new List<string>() { };
					if (session.LevelID != 0)
					{
						sessionEntity.difficulty = eventBoardRoot.Levels.Single(x => x.ID == session.LevelID).Name;
					}

					if (isNew)
					{
						conference.AddSession(sessionEntity);
						conference.Save();
					}

					session session1 = session;
					speaker speaker = null;
					var sessionSpeakers = eventBoardRoot.SessionSpeakers.Where(x => x.SessionID == session.ID).ToList();
					if (sessionSpeakers.Any())
					{
						var speakerID = sessionSpeakers.First(x => x.SessionID == session.ID).SpeakerID;
						speaker = eventBoardRoot.Speakers.FirstOrDefault(s => s.ID == speakerID);
					}

					if (speaker != null)
					{
						var speakerEntity = new SpeakerEntity();

						if (!string.IsNullOrWhiteSpace(speaker.Twitter) && !speaker.Twitter.StartsWith("@"))
						{
							speaker.Twitter = "@" + speaker.Twitter;
						}


						speakerEntity.firstName = speaker.FirstName;
						speakerEntity.lastName = speaker.LastName;
						speakerEntity.slug = (speakerEntity.firstName.ToLower() + " " + speakerEntity.lastName.ToLower()).Trim().Replace(" ", "-");
						speakerEntity.profileImageUrl = speaker.ImageUrl; //speaker.PhotoUrl;
						speakerEntity.twitterName = speaker.Twitter;
						speakerEntity.blogUrl = speaker.URL; //speaker.Website;

						//sessionEntity.speakers = new List<SpeakerEntity>() { speakerEntity };

						conference.AddSpeakerToSession(sessionEntity.slug, speakerEntity);
					}

				}

				conference.Save();


			}



		}

		private class eventBoardRoot
		{
			public List<session> Sessions { get; set; }
			public List<room> Rooms { get; set; }
			public List<level> Levels { get; set; }
			public List<sessionSpeaker> SessionSpeakers { get; set; }
			public List<timeslot> TimeSlots { get; set; }
			public List<speaker> Speakers { get; set; }

		}

		private class sessionsRoot
		{
			public List<session> Sessions { get; set; }
		}

		private class session
		{
			public int ID { get; set; }
			public DateTime ModifiedOn { get; set; }
			public bool CanInviteOthers { get; set; }
			public string Code { get; set; }
			public string Description { get; set; }
			public bool IsOfficial { get; set; }
			public bool IsPrivate { get; set; }
			public bool IsSponsorSession { get; set; }
			public int LevelID { get; set; }
			public int LocationID { get; set; }
			public string Name { get; set; }
			public DateTime RegistrationStartDate { get; set; }
			public bool RequiresRegistration { get; set; }
			public int SessionTypeID { get; set; }
			public bool ShowOnEveryonesAgenda { get; set; }
			public int TimeSlotID { get; set; }
			public Guid UKey { get; set; }
		}

		private class roomsRoot
		{
			public List<room> Rooms { get; set; }
		}

		private class levelsRoot
		{
			public List<level> Levels { get; set; }
		}

		private class level
		{
			public int ID { get; set; }
			public DateTime ModifiedOn { get; set; }
			public string Description { get; set; }
			public string Name { get; set; }
		}
		private class sessionSpeakersRoot
		{
			public List<sessionSpeaker> SessionSpeakers { get; set; }
		}
		private class sessionSpeaker
		{
			public int SessionID { get; set; }
			public int SpeakerID { get; set; }
		}

		private class room
		{
			public int ID { get; set; }
			public DateTime ModifiedOn { get; set; }
			public string Building { get; set; }
			public int Capacity { get; set; }
			public double Latitude { get; set; }
			public double Longitude { get; set; }
			public string Name { get; set; }
		}
		private class timeslotsRoot
		{
			public List<timeslot> TimeSlots { get; set; }
		}

		private class timeslot
		{
			public int ID { get; set; }
			public DateTime ModifiedOn { get; set; }
			public string Description { get; set; }
			public DateTime EndTime { get; set; }
			public DateTime StartTime { get; set; }
		}
		private class speakersRoot
		{
			public List<speaker> Speakers { get; set; }
		}

		private class speaker
		{
			public int ID { get; set; }
			public DateTime ModifiedOn { get; set; }
			public string Bio { get; set; }
			public string Company { get; set; }
			public string FirstName { get; set; }
			public string ImageUrl { get; set; }
			public string LastName { get; set; }
			public string Title { get; set; }
			public string Twitter { get; set; }
			public string URL { get; set; }

		}
	}




}
