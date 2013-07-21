<Query Kind="Program">
  <Reference>C:\dev\VPS\_resources\Logging\FluentMongo.dll</Reference>
  <Reference>C:\dev\VPS\_resources\Logging\MongoDB.Bson.dll</Reference>
  <Reference>C:\dev\VPS\_resources\Logging\MongoDB.Driver.dll</Reference>
  <Reference>C:\dev\ArtekSoftware.Conference.Mobile\packages\ServiceStack.Text.3.7.9\lib\net35\ServiceStack.Text.dll</Reference>
  <Namespace>MongoDB.Bson</Namespace>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>FluentMongo</Namespace>
  <Namespace>FluentMongo.Linq</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{

	var server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
	var database = server.GetDatabase("app4727263");
	var collection = database.GetCollection<ConferenceEntity>("conferences");
	var conferences = collection.AsQueryable().ToL;
	
	conference.Dump();
}

public SpeakerEntity MapSpeaker(CodeMashSpeaker codemashSpeaker)
{
	SpeakerEntity speaker = null;
	if (codemashSpeaker != null && codemashSpeaker.Name.Contains(" "))
	{
		speaker = new SpeakerEntity()
		{
			_id = Guid.NewGuid(),
			firstName = codemashSpeaker.Name.Split(' ')[0],
			lastName = codemashSpeaker.Name.Split(' ')[1],
			blogUrl = GetBlogUrl(codemashSpeaker.BlogURL),
			twitterName = GetTwitterName(codemashSpeaker.TwitterHandle),
			description = codemashSpeaker.Biography,
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
public SessionEntity MapSession(CodeMashSession codemashSession, List<CodeMashSpeaker> speakers)
{
	var codemashSpeaker = speakers.Where(s =>  "/" + s.SpeakerURI ==codemashSession.SpeakerURI).FirstOrDefault();
	var speaker = MapSpeaker(codemashSpeaker);
	
	var session = new SessionEntity()
	{
		 _id = Guid.NewGuid(),
		 description = codemashSession.Abstract,
		 difficulty = codemashSession.Difficulty,
		 start = codemashSession.Start,
		 end = codemashSession.Start.AddHours(1),
		 links = new List<string>(),
		 prerequisites = new List<string>(),
		 resources = new List<string>(),
		 room = codemashSession.Room,
		 slug = codemashSession.Title.ToLower()
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
		sessionType = codemashSession.Technology,
		subjects = new List<string>() { codemashSession.Technology },
		tags = new List<string>() { codemashSession.Technology },
		title = codemashSession.Title,
		twitterHashTag = "#cm-" + codemashSession.Title.ToLower().SafeSubstring(0, 10)
	};
	
	if (speaker != null)
	{
		session.speakers = new List<SpeakerEntity>() { speaker };
	}
	return session;
}

public List<CodeMashSession> GetCodeMashSessions()
{
	string url = "http://codemash.org/rest/sessions.json";

	var client = new WebClient();
	client.Encoding = Encoding.UTF8;
	client.Headers[HttpRequestHeader.Accept] = "application/json";
	var returnString = client.DownloadString(new Uri(url));
	//Console.WriteLine(returnString);
	var sessions = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<CodeMashSession>>(returnString);
	//sessions.Dump();
	return sessions;
}

public List<CodeMashSpeaker> GetCodeMashSpeakers()
{
	string url = "http://codemash.org/rest/speakers.json";

	var client = new WebClient();
	client.Encoding = Encoding.UTF8;
	client.Headers[HttpRequestHeader.Accept] = "application/json";
	var returnString = client.DownloadString(new Uri(url));
	//Console.WriteLine(returnString);
	var speakers = ServiceStack.Text.JsonSerializer.DeserializeFromString<List<CodeMashSpeaker>>(returnString);
	//speakers.Dump();
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

public class CodeMashSpeaker
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
public class CodeMashSession
{
	public string URI {get;set;}
	public string Title {get;set;}
	public string Abstract  {get;set;}
	public DateTime Start {get;set;}
	public string Room {get;set;}
	public string Difficulty {get;set;}
	public string SpeakerName {get;set;}
	public string Technology {get;set;}
	public string SpeakerURI {get;set;}
	public string Lookup {get;set;}
}

	public class ConferenceEntity
	{
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
		public Guid _id { get; set; }
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
		public string conferenceSlug {get;set;}
	}
	
	public class SpeakerEntity
  	{
		public Guid _id { get; set; }
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
		public string conferenceSlug {get;set;}
		public string sessionSlug {get;set;}
  }