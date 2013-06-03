using System.Windows;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.WindowsPhone.Views;
using TekConf.Core.ViewModels;
using Cirrious.CrossCore;

namespace TekConf.UI.WinPhone.Views
{
	public partial class OAuthRegisterView : MvxPhonePage
	{
		private MvxSubscriptionToken _oauthExceptionMessageToken;

		public OAuthRegisterView()
		{
			InitializeComponent();

			var messenger = Mvx.Resolve<IMvxMessenger>();

			_oauthExceptionMessageToken = messenger.Subscribe<CreateOAuthUserExceptionMessage>(message =>
				Dispatcher.BeginInvoke(() =>
				{
					if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
					{
						const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
						MessageBox.Show(errorMessage);
						//OAuthExceptionMessage.Text = "Could not connect to remote server. Please check your network connection and try again.";
						//OAuthExceptionMessage.Visibility = Visibility.Visible;
					}
				}));
		}
		private void Register_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = this.DataContext as OAuthRegisterViewModel;
			if (vm != null)
				vm.CreateOAuthUser();
		}
	}
}