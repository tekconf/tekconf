namespace TekConf.Core.Services
{
	public interface INetworkConnection
	{
		bool IsNetworkConnected();
		string NetworkDownMessage { get; }
	}
}