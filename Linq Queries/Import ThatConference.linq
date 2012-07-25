<Query Kind="Expression">
  <Reference Relative="..\..\VPS\_resources\Logging\FluentMongo.dll">C:\dev\VPS\_resources\Logging\FluentMongo.dll</Reference>
  <Reference Relative="..\..\VPS\_resources\Logging\MongoDB.Bson.dll">C:\dev\VPS\_resources\Logging\MongoDB.Bson.dll</Reference>
  <Reference Relative="..\..\VPS\_resources\Logging\MongoDB.Driver.dll">C:\dev\VPS\_resources\Logging\MongoDB.Driver.dll</Reference>
  <Reference Relative="..\..\ArtekSoftware.Conference.Mobile\packages\ServiceStack.Text.3.7.9\lib\net35\ServiceStack.Text.dll">C:\dev\ArtekSoftware.Conference.Mobile\packages\ServiceStack.Text.3.7.9\lib\net35\ServiceStack.Text.dll</Reference>
  <Namespace>MongoDB.Bson</Namespace>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>FluentMongo</Namespace>
  <Namespace>FluentMongo.Linq</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>MongoDB.Bson.Serialization.Attributes</Namespace>
  <Namespace>MongoDB.Bson.Serialization.IdGenerators</Namespace>
</Query>

void Main()
{

	var server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
	var database = server.GetDatabase("app4727263");
	var collection = database.GetCollection<ConferenceEntity>("conferences");

	var conference = collection.AsQueryable().Where (c => c.slug == "ThatConference-2012").FirstOrDefault ();
	
	if (conference == null)
	{
		conference = new ConferenceEntity()
		{
			_id = Guid.NewGuid(),
			description = "With over 150 sessions to choose from, your head will eventually start to overheat. Cool off in one of the many nearby pools, because unlike your traditional technology conference, you will be camping at a giant indoor waterpark. Be sure to follow us on twitter and check us out on facebook. Then start practicing your cannon balls.",
			name = "ThatConference",
			slug = "ThatConference-2012",
			location = "Kalahari Resort, Wisconsin Dells, WI",
			twitterName = "@thatConference",
			start = DateTime.Parse("08/13/2012"),
			end = DateTime.Parse("08/15/2012"),
			homepageUrl = "http://thatconference.com",
			twitterHashTag = "#thatconference",
			facebookUrl = "https://www.facebook.com/ThatConference",
			lanyrdUrl = "",
			meetupUrl = "",
		};
	}
	else
	{
		Console.WriteLine("Found conference");
	}
	
	var conferenceSessions = GetSessions();
	//var conferenceSpeakers = GetSpeakers();
	conference.sessions = new List<SessionEntity>();
	foreach (var conferenceSession in conferenceSessions)
	{
		
		var session = MapSession(conferenceSession);
		if (!conference.sessions.Any(s => s.slug == session.slug))
		{
			conference.sessions.Add(session);
		}
	}
	//Console.WriteLine(conference.sessions.First().description);
	conference.Dump();
	//return conferences;
	
	collection.Save(conference);
}

public SpeakerEntity MapSpeaker(string persons)
{
	SpeakerEntity speaker = null;
	if (!string.IsNullOrWhiteSpace(persons) && persons.Contains(" "))
	{
		var nameParts = persons.Trim().Split(' ');
		speaker = new SpeakerEntity()
		{
			firstName = nameParts.First (),
			lastName = nameParts.Last (),
			blogUrl = "",
			twitterName = "",
			description = "",
			emailAddress = "",
			facebookUrl = "",
			linkedInUrl = "",
			phoneNumber = ""
		};
		speaker.slug = speaker.firstName.ToLower() + "-" + speaker.lastName.ToLower();
	}
	return speaker;
}

public string GetBlogUrl(string blog)
{
	if (!string.IsNullOrWhiteSpace(blog) && !blog.StartsWith("http://"))
	{
		blog = "http://" + blog;
	}
	
	return blog;
}

public string GetTwitterName(string twitter)
{
	if (!string.IsNullOrWhiteSpace(twitter) && !twitter.StartsWith("@") && !twitter.Contains(".com"))
	{
		twitter = "@" + twitter;
	}
	return twitter;
}

public SessionEntity MapSession(ConferenceSession conferenceSession)
{
	//var conferenceSpeaker = speakers.Where(s =>  "/" + s.SpeakerURI == conferenceSession.SpeakerURI).FirstOrDefault();
	var speaker = MapSpeaker(conferenceSession.Persons);
	
	var session = new SessionEntity()
	{
		 description = conferenceSession.Description,
		 //difficulty = conferenceSession.Di,
		 start = conferenceSession.ScheduledDateTime,
		 end = conferenceSession.ScheduledDateTime.AddHours(1),
		 links = new List<string>(),
		 prerequisites = new List<string>(),
		 resources = new List<string>(),
		 room = conferenceSession.ScheduledRoom,
		 slug = conferenceSession.Title.ToLower()
		 		.Replace(" ", "-")
				.Replace(":", "")
				.Replace("(","")
				.Replace(")","")
				.Replace("#", "")
				.Replace("%", "")
				.Replace("&", "")
				.Replace("*", "")
				.Replace("$", "")
				.Replace("@", "")
				.Replace("!", ""),
		sessionType = conferenceSession.Category,
		subjects = new List<string>() { },
		tags = new List<string>() { },
		title = conferenceSession.Title,
		twitterHashTag = "#cm-" + conferenceSession.Title.ToLower().SafeSubstring(0, 10).Replace(" ", "-")
	};
	
	if (speaker != null)
	{
		session.speakers = new List<SpeakerEntity>() { speaker };
	}
	return session;
}

public List<ConferenceSession> GetSessions()
{
	string url = "http://thatconference.com/api/session";

	var client = new WebClient();
	client.Encoding = Encoding.UTF8;
	client.Headers[HttpRequestHeader.Accept] = "application/json";
	var returnString = client.DownloadString(new Uri(url));
	//Console.WriteLine(returnString);
	var sessions = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<ConferenceSession>>(returnString);
	//sessions.Dump();
	return sessions;
}

public List<ConferenceSpeaker> GetSpeakers()
{
//	string url = "http://codemash.org/rest/speakers.json";
//
//	var client = new WebClient();
//	client.Encoding = Encoding.UTF8;
//	client.Headers[HttpRequestHeader.Accept] = "application/json";
//	var returnString = client.DownloadString(new Uri(url));
//	//Console.WriteLine(returnString);
//	var speakers = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<ConferenceSpeaker>>(returnString);
//	//speakers.Dump();
	var speakers = new List<ConferenceSpeaker>();
	return speakers;
}

public static class StringExtensions
{
	public static string SafeSubstring(this string input, int startIndex, int length)
	{
		// Todo: Check that startIndex + length does not cause an arithmetic overflow
		if (input.Length >= (startIndex + length))
		{
			return input.Substring(startIndex, length);
		}
		else
		{
			if (input.Length > startIndex)
			{
				return input.Substring(startIndex);
			}
			else
			{
				return string.Empty;
			}
		}
	}
}

public class ConferenceSpeaker
{
	public string SpeakerURI {get;set;}
	public string Name {get;set;}
	public string SpeakerLookup {get;set;}
	public string Biography {get;set;}
	public string ContactInfo  {get;set;}
	public string TwitterHandle {get;set;}
	public string BlogURL {get;set;}
}

// Define other methods and classes here
public class ConferenceSession
{
	public string URI {get;set;}
	public string Title {get;set;}
	public string Description  {get;set;}
	public string Category  {get;set;}
	public string Level  {get;set;}
	public string Tag  {get;set;}
	public string Persons  {get;set;}
	public DateTime ScheduledDateTime {get;set;}
	public string ScheduledRoom {get;set;}
}

	public class ConferenceEntity
	{
		[BsonId(IdGenerator = typeof(CombGuidGenerator))]
		public Guid _id { get; set; }
		public string description { get; set; }
		public string facebookUrl { get; set; }
		public string slug { get; set; }
		public string homepageUrl { get; set; }
		public string lanyrdUrl { get; set; }
		public string location { get; set; }
		public string meetupUrl { get; set; }
		public string name { get; set; }
		public object start { get; set; }
		public object end { get; set; }
		public string twitterHashTag { get; set; }
		public string twitterName { get; set; }
		public List<SessionEntity> sessions { get; set; }
	}
  
	public class SessionEntity
	{
		public string slug { get; set; }
		public string title { get; set; }
		public object start { get; set; }
		public object end { get; set; }
		public string room { get; set; }
		public string difficulty { get; set; }
		public string description { get; set; }
		public string twitterHashTag { get; set; }
		public string sessionType { get; set; }
		public List<string> links { get; set; }
		public List<string> tags { get; set; }
		public List<string> subjects { get; set; }
		public List<string> resources { get; set; }
		public List<string> prerequisites { get; set; } 
		public List<SpeakerEntity> speakers { get; set; }
	}
	
	public class SpeakerEntity
  	{
		public string slug { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string description {get;set;}
		public string blogUrl {get;set;}
		public string twitterName {get;set;}
		public string facebookUrl {get;set;}
		public string linkedInUrl {get;set;}
		public string emailAddress {get;set;}
		public string phoneNumber {get;set;}
  }