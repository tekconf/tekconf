using System.Windows;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Core.Messages;
using TekConf.Core.ViewModels;
using Cirrious.CrossCore;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferenceSearchView
	{
		private MvxSubscriptionToken _token;
		public ConferenceSearchView()
		{
			InitializeComponent();
			var messenger = Mvx.Resolve<IMvxMessenger>();
			_token = messenger.Subscribe<ExceptionMessage>(message => Dispatcher.BeginInvoke(() => MessageBox.Show(message.ExceptionObject == null ? "An exception occurred but was not caught" : message.ExceptionObject.Message)));

		}
	}
}