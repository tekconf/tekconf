//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using FluentMongo.Linq;
//using ServiceStack.Text;
//using TekConf.Common.Entities;

//namespace UberImporter.Importers.RailsConf2012
//{
//		public class RailsConfImporter
//		{

//				public void Import()
//				{
//						var url = "http://railsconf2012.busyconf.com/event.json";
//						var request = new WebClient();
//						var json = request.DownloadString(url);
//						var root = json.FromJson<root>();

//						if (root != null)
//						{
//								var connection = new MongoDbConnection();
//								var collection = connection.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");
//								if (!collection.AsQueryable().Any(c => c.slug == "RailsConf-2012"))
//								{
//										var conference = new ConferenceEntity()
//										{
//												//_id = Guid.NewGuid(),
//												description = @"RailsConf, the largest gathering of Rubyists and Rails developers worldwide, will be in Portland, OR. Join us April 29 - May 2, 2013!",
//												end = new DateTime(2012, 04, 25),
//												facebookUrl = "",
//												homepageUrl = "http://railsconf2012.org",
//												imageUrl = "/img/conferences/RailsConf.png",
//												lanyrdUrl = "",
//												location = "Austin, TX",
//												meetupUrl = "",
//												name = "RailsConf",
//												//sessions = new List<SessionEntity>(),
//												//slug = "RailsConf-2012",
//												start = new DateTime(2012, 04, 23),
//												tagLine = "",
//												twitterHashTag = "#railsConf",
//												twitterName = "@railsConf"
//										};

//										foreach (var activity in root.activities)
//										{
//												var timeSlot = root.time_slots.FirstOrDefault(t => t._id == activity.time_slot_id);
//												string location = string.Empty;
//												if (!string.IsNullOrWhiteSpace(activity.location_override))
//												{
//														location = activity.location_override;
//												}
//												else
//												{
//														var currentTrack = root.tracks.FirstOrDefault(t => t._id == timeSlot.track_id);
//														if (currentTrack != null)
//														{
//																location = currentTrack.location;
//														}
//												}

//												var sessionEntity = new SessionEntity()
//												{
//														_id = Guid.NewGuid(),
//														description = activity.description,
//														start = timeSlot.starts_at,
//														end = timeSlot.stops_at,
//														title = activity.title,
//														room = location,
//														slug = activity.title.ToLower().GenerateSlug(),
//														twitterHashTag = "#rc-" + activity.title.ToLower().Trim().Replace(" ", "-").SafeSubstring(0, 10),
//														difficulty = activity.category_name,
//														links = null,
//														prerequisites = null,
//														resources = null,
//														sessionType = null,
//														subjects = new List<string>() { activity.category_name },
//														tags = new List<string>() { activity.category_name },
//														speakers = new List<SpeakerEntity>(),
//												};

//												foreach (var speaker in activity.speakers)
//												{
//														if (!string.IsNullOrWhiteSpace(speaker.twitter_username) && !speaker.twitter_username.StartsWith("@"))
//														{
//																speaker.twitter_username = "@" + speaker.twitter_username;
//														}

//														var speakerEntity = new SpeakerEntity()
//																										{
//																												_id = Guid.NewGuid(),
//																												bitbucketUrl = "",
//																												blogUrl = speaker.url,
//																												codeplexUrl = null,
//																												coderWallUrl = null,
//																												company = speaker.company,
//																												description = speaker.bio,
//																												emailAddress = null,
//																												facebookUrl = null,
//																												firstName = speaker.name.Split(' ')[0],
//																												githubUrl = null,
//																												googlePlusUrl = null,
//																												isFeatured = false,
//																												lastName = speaker.name.Split(' ')[1],
//																												linkedInUrl = null,
//																												phoneNumber = null,
//																												profileImageUrl = null,
//																												slug = speaker.name.ToLower().GenerateSlug(),
//																												stackoverflowUrl = null,
//																												twitterName = null,
//																												vimeoUrl = null,
//																												youtubeUrl = null,
//																										};

//														sessionEntity.speakers.Add(speakerEntity);
//												}


//												conference.AddSession(sessionEntity);
//										}

//										conference.Save(collection);
//								}
//						}



//				}


//				public class root
//				{
//						public string _id { get; set; }
//						public string name { get; set; }
//						public string short_name { get; set; }
//						public string subdomain { get; set; }
//						public string time_zone_name { get; set; }
//						public string pixels_per_minute { get; set; }
//						public string minimum_time_slot_height { get; set; }
//						public DateTime created_at { get; set; }
//						public DateTime updated_at { get; set; }
//						public List<day> days { get; set; }
//						public List<time_slot> time_slots { get; set; }
//						public List<track> tracks { get; set; }
//						public List<activity> activities { get; set; }

//				}
//				public class day
//				{
//						public string _id { get; set; }
//						public DateTime starts_at { get; set; }
//						public DateTime stops_at { get; set; }
//						public string title { get; set; }
//						public string schedule_default { get; set; }
//						public int time_zone_offset { get; set; }
//						public DateTime created_at { get; set; }
//						public DateTime updated_at { get; set; }
//						public List<string> ordered_track_ids { get; set; }
//				}
//				public class time_slot
//				{
//						public string _id { get; set; }
//						public bool global { get; set; }
//						public string day_id { get; set; }
//						public string track_id { get; set; }
//						public DateTime starts_at { get; set; }
//						public DateTime stops_at { get; set; }
//						public string title { get; set; }
//						public DateTime created_at { get; set; }
//						public DateTime updated_at { get; set; }
//				}
//				public class track
//				{
//						public string _id { get; set; }
//						public string location { get; set; }
//						public string title { get; set; }
//						public DateTime created_at { get; set; }
//						public DateTime updated_at { get; set; }
//				}
//				public class activity
//				{
//						public string _id { get; set; }
//						public string time_slot_id { get; set; }
//						public string title { get; set; }
//						public string description { get; set; }
//						public string location_override { get; set; }
//						public int category { get; set; }
//						public string category_name { get; set; }
//						public DateTime created_at { get; set; }
//						public DateTime updated_at { get; set; }
//						public List<speaker> speakers { get; set; } 
//				}
//				public class speaker
//				{
//						public string _id { get; set; }
//						public string name { get; set; }
//						public string company { get; set; }
//						public string job_title { get; set; }
//						public string headline { get; set; }
//						public string bio { get; set; }
//						public string location { get; set; }
//						public string url { get; set; }
//						public string twitter_username { get; set; }
//						public DateTime created_at { get; set; }
//						public DateTime updated_at { get; set; }
//						public string avatar_square_b64 { get; set; }
//						public string avatar_square_type { get; set; }
//						public string avatar_square_url { get; set; }
//						public string avatar_url { get; set; }
//				}

//		}
//}