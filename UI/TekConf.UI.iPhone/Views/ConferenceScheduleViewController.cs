
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.Accounts;
using System.Linq;

namespace TekConf.UI.iPhone
{
	public partial class ConferenceScheduleViewController : BaseDialogViewController
	{
		private bool _showPastConferences = false;

		public string SearchString { get; set; }

		private ACAccountStore _accountStore;
		private string AppId = "417883241605228";

		public ConferenceScheduleViewController () : base(UITableViewStyle.Plain, new RootElement("Schedule"), false)
		{
			this.EnableSearch = true;
			
			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
				RefreshControl = new UIRefreshControl ();
				RefreshControl.ValueChanged += (sender, e) => {
					Refresh (); 
				};
			} else {
				// old style refresh button
				NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Refresh), false);
				NavigationItem.RightBarButtonItem.Clicked += (sender, e) => {
					Refresh (); };
			}
			
		}
		
		public override void LoadView ()
		{
			base.LoadView ();
			
			this.View = this.TableView;
			if (ParentViewController != null && ParentViewController.View != null) {
				ParentViewController.View.BackgroundColor = UIColor.Red;
			}
		}
		
		public void Refresh ()
		{
			if (this.IsReachable ()) {
				var loading = new UIAlertView (" Downloading Schedule", "Please wait...", null, null, null);
				
				loading.Show ();
				
				var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
				indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
				indicator.StartAnimating (); 
				loading.AddSubview (indicator);

				if (_accountStore == null)
					_accountStore = new ACAccountStore ();
				
				var options = new AccountStoreOptions () {
					FacebookAppId = AppId
				};
				options.SetPermissions (ACFacebookAudience.OnlyMe, new string[]{ "email" });
				var accountType = _accountStore.FindAccountType (ACAccountType.Facebook);
				
				_accountStore.RequestAccess (accountType, options, (granted, error) => {
					if (granted) 
					{
						var facebookAccount = _accountStore.FindAccounts (accountType).First ();
						var oAuthToken = facebookAccount.Credential.OAuthToken;

						Repository.GetSchedule (conferenceSlug: NavigationItems.ConferenceSlug, authenticationMethod: "Facebook", authenticationToken: facebookAccount.Username, callback: schedule => 
						{ 
							if (schedule != null) {
								var rootElement = new RootElement ("Schedule"){ new Section() };
								
								UIImage defaultImage = UIImage.FromBundle (@"images/DefaultConference.png");
								
								foreach (var session in schedule.sessions) {
									rootElement [0].Add (new StringElement (session.title));
									//rootElement [0].Add (new ConferenceElement (conference, defaultImage));
								}
								
								InvokeOnMainThread (() => 
								{ 
									Root = rootElement;
									this.ReloadData ();
									this.TableView.ScrollsToTop = true;
									
									loading.DismissWithClickedButtonIndex (0, true);
									
									if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
										RefreshControl.EndRefreshing ();
									}
									
								});					
								
							}
							else
							{
								InvokeOnMainThread (() => 
								{ 
									loading.DismissWithClickedButtonIndex (0, true);
								});
							}
							
						});

						return;
					} else {
						InvokeOnMainThread (() => 
						{ 
							loading.DismissWithClickedButtonIndex (0, true);
							var notLoggedInAlertView = new UIAlertView("Not logged in", "You must go to Settings and login to Facebook or Twitter before saving your schedule.", null, "OK", null);
							notLoggedInAlertView.Show();
						});
					}
					
					if (error.Code == 7) {
						return;
					}
					
					// some other error, e.g. user has not set up his Facebook account in Settings
					// fallback; 1) Facebook app, 2) in-app UIWebView or Safari
					
				});


			} else {
				UnreachableAlert ().Show ();
			}
			
			TrackAnalyticsEvent ("ConferenceScheduleViewController");
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			Refresh ();
			

			if (!string.IsNullOrEmpty (SearchString)) {
				this.PerformFilter (SearchString);
			}
			
		}
		
		public override void FinishSearch ()
		{
			base.FinishSearch ();
		}
		
		public override void OnSearchTextChanged (string text)
		{
			base.OnSearchTextChanged (text);
			SearchString = text;
			TableView.SetNeedsDisplay ();
		}
	
	}
}