using TekConf.Core.Services;

namespace TekConf.UI.WinPhone.Bootstrap
{
	public class WinPhoneNetworkConnection : INetworkConnection
	{
		public bool IsNetworkConnected()
		{
			return (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType != Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None);
		}

		public string NetworkDownMessage
		{
			get
			{
				return "Could not connect to remote server. Please check your network connection and try again.";
			}
		}
	}
}