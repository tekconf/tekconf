using System.Windows;
using TekConf.Core.Services;

namespace TekConf.UI.WinPhone.Bootstrap
{
	public class WinPhoneMessageBox : IMessageBox
	{
		public void Show(string message)
		{
			MessageBox.Show(message);
		}
	}
}