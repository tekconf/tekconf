using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.SlideoutNavigation;
using MonoTouch.Dialog;
using FA=FlurryAnalytics;
using MonoTouch.Accounts;
using TekConf.RemoteData.v1;

namespace TekConf.UI.iPhone
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		public SlideoutNavigationController Menu { get; private set; }

		const string account = "UA-20184526-3";

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			FA.FlurryAnalytics.StartSession ("J57HPDDQQ8J8MVGKBKF7");
			FA.FlurryAnalytics.SetSessionReportsOnPause (true);

			var font = UIFont.FromName ("OpenSans-Light", 14f);
			var headerFont = UIFont.FromName ("OpenSans", 16f);
			var detailFont = UIFont.FromName ("OpenSans", 12f);


			UISearchBar.Appearance.TintColor = UIColor.FromRGBA (red: 0.506f, 
			                                                    green: 0.6f, 
			                                                    blue: 0.302f,
			                                                    alpha: 1f);

			UISearchBar.Appearance.BackgroundColor = UIColor.FromRGBA (red: 0.506f, 
				                                                          green: 0.6f, 
				                                                          blue: 0.302f,
			                                                          alpha: 1f);


			UITextAttributes buttonAttributes = new UITextAttributes () { Font = detailFont };
			
			UIBarButtonItem.Appearance.SetTitleTextAttributes (buttonAttributes, UIControlState.Normal);
			UITextAttributes attributes = new UITextAttributes () { Font = headerFont };
			UINavigationBar.Appearance.SetTitleTextAttributes (attributes);
			UILabel.Appearance.Font = font;
		
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			Menu = new SlideoutNavigationController ();
			Console.WriteLine ("AD1");
			Menu.TopView = new ConferencesDialogViewController ();
			//Menu.TopView = new ConferencesListViewController();
			Console.WriteLine ("AD2");
			
			Menu.MenuView = new SideListController ();
			
			window.RootViewController = Menu;
			window.MakeKeyAndVisible ();





			return true;
		}
	}

	public class SideListController : DialogViewController
	{
		private ACAccountStore _accountStore;
		private string AppId = "417883241605228";

		//private string _baseUrl = "http://api.tekconf.com";
		private string _baseUrl = "http://192.168.1.116/TekConf.UI.Api";
		private RemoteDataRepository _client;
		private RemoteDataRepository Repository
		{
			get
			{
				if (this._client == null)
				{
					this._client = new RemoteDataRepository(_baseUrl);
				}
				
				return this._client;
			}
		}

		public SideListController () 
			: base(UITableViewStyle.Plain, new RootElement(""))
		{
		}

		protected bool IsReachable()
		{
			return Reachability.IsHostReachable("api.tekconf.com");
		}

		protected UIAlertView UnreachableAlert()
		{
			return new UIAlertView("Unreachable", "Can not access TekConf.com. Check internet connection.", null, "OK", null);
		}

		protected void TrackAnalyticsEvent(string eventName)
		{
			FlurryAnalytics.FlurryAnalytics.LogEvent(eventName);		
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Root.Add (new Section () {
				new StyledStringElement("Conferences", () => { NavigationController.PushViewController(new ConferencesDialogViewController(), true); }) { Font = BaseUIViewController.TitleFont },
				new StyledStringElement("Settings", () => { NavigationController.PushViewController(new SettingsViewController(), true); }) { Font = BaseUIViewController.TitleFont },
			});

			if (NavigationController != null) {
				NavigationController.NavigationBar.TintColor = UIColor.FromRGBA (red: 0.506f, 
				                                                                 green: 0.6f, 
				                                                                 blue: 0.302f, 
				                                                                 alpha: 1f);
				
			}

			if (this.IsReachable ()) {
				if (_accountStore == null) {
					_accountStore = new ACAccountStore ();
				}
				
				var options = new AccountStoreOptions () {
					FacebookAppId = AppId
				};
				
				options.SetPermissions (ACFacebookAudience.OnlyMe, new string[]{ "email" });
				var accountType = _accountStore.FindAccountType (ACAccountType.Facebook);
				
				_accountStore.RequestAccess (accountType, options, (granted, error) => {
					if (granted) {
						var facebookAccount = _accountStore.FindAccounts (accountType).First ();
						var oAuthToken = facebookAccount.Credential.OAuthToken;

						//TODO : Don't hardcode password
						Repository.Register("Facebook", facebookAccount.Username, facebookAccount.Identifier, "password", facebookAccount.Username);

						Repository.GetSchedules (authenticationMethod: "Facebook", 
						                                 authenticationToken: facebookAccount.Username, 
						                         		userName:facebookAccount.Username,
					                         			password:"password",
						                                 callback: schedules => 
						{ 	
							InvokeOnMainThread (() => 
							{ 
								foreach (var schedule in schedules)
								{
									Root[0].Add(
										new StyledStringElement(schedule.conferenceName, 
									                        () => { 
																	NavigationItems.ConferenceSlug = schedule.conferenceSlug; 
																	NavigationController.PushViewController(new ConferenceDetailTabBarController(), true); 
																  }
															) 
									{ Font = BaseUIViewController.TitleFont }
									);
								}

								//loading.DismissWithClickedButtonIndex (0, true);
							});
						});
						
						return;
					} else {
						InvokeOnMainThread (() => 
						{ 	
							var notLoggedInAlertView = new UIAlertView ("Not logged in", "You must go to Settings and login to Facebook or Twitter before saving your schedule.", null, "OK", null);
							notLoggedInAlertView.Show ();
						});
					}
				});
			} else {
				UnreachableAlert ().Show ();
			}
			
			TrackAnalyticsEvent ("AppLaunched");


		}
	}
}

