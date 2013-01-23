namespace TekConf.UI.Api
{
	public interface IConfiguration
	{
		int cacheTimeout { get; }
		string webUrl { get; }
		string MongoServer { get; }
	}
}