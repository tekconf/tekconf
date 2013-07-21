namespace TekConf.Azure
{
	public interface IImageSaverConfiguration
	{
		string ImageUrl { get; }
		string ConnectionString { get; }
	}
}
