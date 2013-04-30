namespace TekConf.Common.Entities
{
	public interface IEntityConfiguration
	{
		int cacheTimeout { get; }
		string webUrl { get; }
		string MongoServer { get; }

		string TwitterConsumerKey { get; }
		string TwitterConsumerSecret { get; }
		string TwitterAccessToken { get; }
		string TwitterAccessTokenSecret { get; }
	}
}