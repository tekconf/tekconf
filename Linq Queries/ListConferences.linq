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
  <Namespace>MongoDB.Bson.Serialization.Attributes</Namespace>
  <Namespace>MongoDB.Bson.Serialization.IdGenerators</Namespace>
</Query>

void Main()
{
	var _server = MongoServer.Create("mongodb://admin:goldie12@flame.mongohq.com:27100/app4727263?safe=true");
	var _database = _server.GetDatabase("app4727263");
	var collection =  _database.GetCollection<ConferenceEntity>("conferences");
	
	var conferences = collection.AsQueryable()
							.ToList();
	
	conferences.Dump();
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