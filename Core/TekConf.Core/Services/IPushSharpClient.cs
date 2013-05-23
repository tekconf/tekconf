namespace TekConf.Core.Services
{
	public interface IPushSharpClient
	{
		void Unregister();
		void Register();
	}
}