using TekConf.Core.Interfaces;

namespace TekConf.UI.WinStore
{
	public class WinStoreAnalytics : IAnalytics
	{
		public void SendView(string view)
		{
			GoogleAnalytics.EasyTracker.GetTracker().SendView(view);
		}
	}
}