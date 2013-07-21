<Query Kind="Program">
  <Reference>C:\dev\VPS\_resources\Logging\FluentMongo.dll</Reference>
  <Reference>C:\dev\VPS\_resources\Logging\MongoDB.Bson.dll</Reference>
  <Reference>C:\dev\VPS\_resources\Logging\MongoDB.Driver.dll</Reference>
  <Namespace>MongoDB.Bson</Namespace>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>FluentMongo</Namespace>
  <Namespace>FluentMongo.Linq</Namespace>
</Query>

void Main()
{
	var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
	var _database = _server.GetDatabase("app4727263");
	var collection =  _database.GetCollection<Conference>("conferences");
	
	var conference = collection.AsQueryable().Where(c => c.slug == "ThatConference-2012").SingleOrDefault();
	
	if (conference.sessions == null)
	{
		conference.sessions = new List<Session>();
	}
	
	var sessionSlug = "ActiveJDBC".ToLower().Replace(" ", "-");
	var session = conference.sessions.Where(s => s.slug == sessionSlug).SingleOrDefault();
	
	if (session == null)
	{
		session = new Session()
		{
			_id = Guid.NewGuid(),
			conferenceSlug = conference.slug,
			description = "ActiveJDBC is a newcomer to the Java ORM space. It is modeled on the Active Record design pattern. It is fast, lightweight and provides simple and intuitive APIs. ActiveJDBC uses a convention over configuration paradigm to minimize boilerplate code. ActiveWeb is a full stack Java web framework. It handles everything you need for modern web development and inherently promotes good BDD/TDD practices. It also improves developer productivity by refreshing new code in the application, thereby eliminating the compile/build/deploy cycle. In this session, learn how to be more productive by using the powerful combination of ActiveJDBC and ActiveWeb on your next project. This session is hands on and you will develop a simple application.",
			difficulty = "300",
			end = DateTime.Now,
			links = new List<string>(),
			room = "",
			sessionType = "Web",
			slug = sessionSlug,
			speakers = new List<Speaker>(),
			start = DateTime.Now,
			subjects = new List<string>(),
			tags = new List<string>() {"activerecord", "framework", "orm", "web", "java"},
			title = "Activate your Web development with ActiveJDBC and ActiveWeb",
			twitterHashTag = "#tc-ActiveJDBC".ToLower()
		};
		
		conference.sessions.Add(session);
	}
	else
	{
//		if (session.resources == null)
//		{
//			session.resources = new List<string>();
//		}
//		if (session.links == null)
//		{
//			session.links == new List<string>();
//		}
//		if (session.prerequisites == null)
//		{
//			session.prerequisites == new List<string>();
//		}		
		session.resources = new List<string>() { "http://resource1.com", "http://resource2.com" };
		session.links = new List<string>() { "http://link1.com", "http://link2.com" };
		session.prerequisites = new List<string>() { "http://pre1.com", "http://pre2.com" };
		session.subjects = new List<string>() { "subj1", "subj2" };
	}
	
	if (session.speakers == null)
	{
		session.speakers = new List<Speaker>();
	}
	
	var speakerSlug = "Igor Polevoy".ToLower().Replace(" ", "-");
	if (!session.speakers.Any(s => s.slug == speakerSlug))
	{
		var speaker = new Speaker()
		{
			_id = Guid.NewGuid(),
			slug = speakerSlug,
			firstName = "Igor",
			lastName = "Polevoy",
		};
		session.speakers.Add(speaker);
	}
	//conference.Dump();
	
	collection.Save(conference);
	
	conference.Dump();

}
  public class Conference
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
	public List<Session> sessions { get; set; }

  }
  
  	public class Session
	{
		public Guid _id { get; set; }
		public string slug { get; set; }
		public string conferenceSlug { get; set; }
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
		public List<Speaker> speakers { get; set; }
		public List<string> resources {get;set;}
		public List<string> prerequisites {get;set;}
	}
	
	  public class Speaker
  {

	public Guid _id { get; set; }
	public string slug { get; set; }
	public string firstName { get; set; }
	public string lastName { get; set; }
	public string conferenceSlug { get; set; } //TODO
	public string sessionSlug { get; set; } //TODO
  }