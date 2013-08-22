using System;
using Android.App;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using TekConf.Core.ViewModels;
using Android.OS;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Views;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using TekConf.Core.Services;
using Cirrious.CrossCore;
using Android.Widget;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Core.Messages;
using System.ComponentModel;
using TekConf.UI.Android.Views;

namespace TekConf.UI.Android
{
	[Activity(Label = "Settings")]
	public class SettingsView : MvxActivity
	{
		private MvxSubscriptionToken _exceptionMessageToken;
		private MvxSubscriptionToken _authenticationMessageToken;
		private MvxSubscriptionToken _userNameChangedMessageToken;
		private MvxSubscriptionToken _settingsExceptionToken;
		private IAuthentication _authentication;
		private INetworkConnection _networkConnection;
		private IMessageBox _messageBox;

		public SettingsView ()
		{
			var messenger = Mvx.Resolve<IMvxMessenger>();
			_authentication = Mvx.Resolve<IAuthentication>();
			_networkConnection = Mvx.Resolve<INetworkConnection>();
			_messageBox = Mvx.Resolve<IMessageBox>();

			_exceptionMessageToken = messenger.Subscribe<ExceptionMessage>(message => RunOnUiThread(() => _messageBox.Show(message.ExceptionObject == null ? "An exception occurred but was not caught" : message.ExceptionObject.Message)));
			_authenticationMessageToken = messenger.Subscribe<AuthenticationMessage>(message => RunOnUiThread(()=> OnAuthenticateMessage(message)));
			_userNameChangedMessageToken = messenger.Subscribe<UserNameChangedMessage>(message => RunOnUiThread(()=> OnUserNameChanged(message)));
			_settingsExceptionToken = messenger.Subscribe<SettingsIsOauthUserRegisteredExceptionMessage>(message =>
			                                                                                             RunOnUiThread(() =>
			                       {
				if (message.ExceptionObject.Message == "The remote server returned an error: NotFound.")
				{
					const string errorMessage = "Could not connect to remote server. Please check your network connection and try again.";
					_messageBox.Show(errorMessage);
					//SettingsExceptionMessage.Text = errorMessage;
					//SettingsExceptionMessage.Visibility = Visibility.Visible;
				}
			}));



		}

		protected override void OnCreate(Bundle bundle)
		{
			RequestWindowFeature(WindowFeatures.ActionBar);

			base.OnCreate(bundle);

			SetContentView(Resource.Layout.SettingsView);

			var set = this.CreateBindingSet<SettingsView, SettingsViewModel>();
			set.Apply();

			ActionBar.SetBackgroundDrawable(new ColorDrawable(new Color(r:129,g:153,b:77)));
			ActionBar.SetDisplayShowHomeEnabled(false);

			var twitterButton = FindViewById<ImageButton> (Resource.Id.twitterLoginButton);
			twitterButton.Click += async (object sender, EventArgs e) => {
				await AuthenticateWithTwitter();
			};

			var googleButton = FindViewById<ImageButton> (Resource.Id.googlePlusLoginButton);
			googleButton.Click += async (object sender, EventArgs e) => {
				await AuthenticateWithGoogle();
			};

			var facebookButton = FindViewById<ImageButton> (Resource.Id.facebookLoginButton);
			facebookButton.Click += async (object sender, EventArgs e) => {
				await AuthenticateWithFacebook();
			};

			var tekconfButton = FindViewById<ImageButton> (Resource.Id.tekConfLoginButton);
			tekconfButton.Click += (object sender, EventArgs e) => {
				var vm = this.DataContext as SettingsViewModel;
				if (vm != null)
					vm.ShowTekConfLoginCommand.Execute(null);
			};

			SetLoggedInState();
		}

		private async void LoginWithTwitter_OnClick(View view)
		{
			if (Setup.MobileService.CurrentUser != null)
			{
				//TODO var result = _messageBox.Show(string.Format("You are logged in as {0}. Are you sure you want to login again?", Setup.UserName), "Logout?", MessageBoxButton.OKCancel);
				//if (result == _messageBox.OK)
				//{
				//	Logout_OnClick(sender, e);
				//}
			}

			var vm = DataContext as SettingsViewModel;
			if (vm != null) 
				vm.PropertyChanged += VmOnPropertyChanged;

			await AuthenticateWithTwitter();
		}

		private MobileServiceUser _user;
		private async Task AuthenticateWithTwitter()
		{
			if (!_networkConnection.IsNetworkConnected())
			{
				_messageBox.Show(_networkConnection.NetworkDownMessage);
			}
			else
			{
				while (_user == null)
				{
					try
					{
						_user = await Setup.MobileService.LoginAsync(this, MobileServiceAuthenticationProvider.Twitter);
					}
					catch
					{
						//MessageBox.Show(ex.Message);
					}
				}

				if (_user != null)
				{

					var vm = DataContext as SettingsViewModel;
					if (vm != null)
						vm.IsOauthUserRegistered(_user.UserId);
				}
				SetLoggedInState();
			}
		}

		private async Task AuthenticateWithFacebook()
		{
			if (!_networkConnection.IsNetworkConnected())
			{
				_messageBox.Show(_networkConnection.NetworkDownMessage);
			}
			else
			{
				while (_user == null)
				{
					try
					{
						_user = await Setup.MobileService.LoginAsync(this, MobileServiceAuthenticationProvider.Facebook);
					}
					catch (InvalidOperationException)
					{
						//MessageBox.Show(ex.Message);
					}
				}

				if (_user != null)
				{
					var vm = DataContext as SettingsViewModel;
					if (vm != null)
						vm.IsOauthUserRegistered(_user.UserId);
				}
				SetLoggedInState();
			}
		}

		private async Task AuthenticateWithGoogle()
		{
			if (!_networkConnection.IsNetworkConnected())
			{
				_messageBox.Show(_networkConnection.NetworkDownMessage);
			}
			else
			{
				while (_user == null)
				{
					try
					{
						_user = await Setup.MobileService.LoginAsync(this, MobileServiceAuthenticationProvider.Google);
					}
					catch (InvalidOperationException)
					{
						//MessageBox.Show(ex.Message);
					}
				}

				if (_user != null)
				{

					var vm = DataContext as SettingsViewModel;
					if (vm != null)
						vm.IsOauthUserRegistered(_user.UserId);
				}

				SetLoggedInState();
			}
		}


		private void OnUserNameChanged(UserNameChangedMessage obj)
		{
			SetLoggedInState();
		}

		private void OnAuthenticateMessage(AuthenticationMessage message)
		{
			Setup.UserName = message.UserName;
		}

		private void VmOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == "UserName")
			{
				var vm = sender as SettingsViewModel;
				if (vm != null) 
					Setup.UserName = vm.UserName;
			}
		}

		private void SetLoggedInState()
		{
			var twitterButton = FindViewById<ImageButton> (Resource.Id.twitterLoginButton);
			var facebookButton = FindViewById<ImageButton> (Resource.Id.facebookLoginButton);
			var googleButton = FindViewById<ImageButton> (Resource.Id.googlePlusLoginButton);
			var tekconfButton = FindViewById<ImageButton> (Resource.Id.tekConfLoginButton);
			var loginName = FindViewById<MyTextView> (Resource.Id.loginName);
			var logoutButton = FindViewById<Button> (Resource.Id.logoutButton);
			var loggedInLayout = FindViewById<TableRow> (Resource.Id.loggedInState);

			var isLoggedIn = _authentication.IsAuthenticated;
			if (!string.IsNullOrWhiteSpace(Setup.UserName) && string.IsNullOrWhiteSpace(_authentication.UserName))
			{
				_authentication.UserName = Setup.UserName;
			}

			if (isLoggedIn) 
			{
				facebookButton.Visibility = ViewStates.Gone;
				googleButton.Visibility = ViewStates.Gone;
				tekconfButton.Visibility = ViewStates.Gone;
				twitterButton.Visibility = ViewStates.Gone;

				loggedInLayout.Visibility = ViewStates.Visible;			
				logoutButton.Visibility = ViewStates.Visible;
				loginName.Visibility = !string.IsNullOrWhiteSpace(_authentication.UserName) ? ViewStates.Visible : ViewStates.Gone;
				loginName.Text = !string.IsNullOrWhiteSpace(_authentication.UserName) ? "Logged in as " + _authentication.UserName : "";
			} 
			else 
			{
				facebookButton.Visibility = ViewStates.Visible;
				googleButton.Visibility = ViewStates.Visible;
				tekconfButton.Visibility = ViewStates.Visible;
				twitterButton.Visibility = ViewStates.Visible;

				logoutButton.Visibility = ViewStates.Gone;
				loginName.Visibility = ViewStates.Gone;
				loginName.Text = "";
				loggedInLayout.Visibility = ViewStates.Gone;
			}


			if (isLoggedIn)
			{
				var vm = DataContext as SettingsViewModel;
				if (vm != null && !vm.IsOptedInToNotifications)
				{
					//TODO var result = MessageBox.Show("Enable Push Notifications?", "Push Notifications", MessageBoxButton.OKCancel);
					//TODO if (result == MessageBoxResult.OK)
					//TODO {
					//TODO 	vm.IsOptedInToNotifications = true;
					//TODO }
					//TODO else if (result == MessageBoxResult.Cancel)
					//TODO {
					//TODO 	vm.IsOptedInToNotifications = false;
					//TODO }
				}
				else
				{
					if (vm != null) 
						vm.IsOptedInToNotifications = true;
				}
			}
			else
			{
				var vm = DataContext as SettingsViewModel;
				if (vm != null) 
					vm.IsOptedInToNotifications = false;
			}
		}

		private void Logout_OnClick()
		{
			Setup.MobileService.Logout();
			Setup.UserName = "";
			_user = null;
			_authentication.UserName = "";

			SetLoggedInState();
		}

		private void Register_OnClick()
		{
			_messageBox.Show("Not yet implemented");
		}


	}
}

