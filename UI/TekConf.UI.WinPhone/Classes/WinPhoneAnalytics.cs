using TekConf.Core.Interfaces;

namespace TekConf.UI.WinPhone
{
	public class WinPhoneAnalytics : IAnalytics
	{
		public void SendView(string view)
		{
			GoogleAnalytics.EasyTracker.GetTracker().SendView(view);
		}
	}
}