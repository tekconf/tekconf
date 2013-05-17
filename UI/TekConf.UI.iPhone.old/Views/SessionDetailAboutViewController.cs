
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;
using MonoTouch.Accounts;
using System.Linq;

namespace TekConf.UI.iPhone
{
	public partial class SessionDetailAboutViewController : BaseUIViewController
	{

		private string _sessionSlug;
		private ACAccountStore _accountStore;
		private string AppId = "417883241605228";
		private SessionDto _session;

		public SessionDetailAboutViewController (string sessionSlug) : this()
		{
			_sessionSlug = sessionSlug;
		}

		public SessionDetailAboutViewController () : base ("SessionDetailAboutViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.contentDetailScrollView.ContentSize = new SizeF (width: this.View.Frame.Width, height: 600);
			this.contentDetailScrollView.ScrollEnabled = true;
			this.contentDetailScrollView.ClipsToBounds = true;
			this.contentDetailScrollView.ContentInset = new UIEdgeInsets (top: 0, left: 0, bottom: 900, right: 0);

			this.imAttendingButton.TouchUpInside += ImAttendingTouched;
			this.facebookButton.TouchUpInside += FacebookTouched;
			this.twitterButton.TouchUpInside += TwitterTouched;

			this.facebookButton.Hidden = true;
			this.twitterButton.Hidden = true;
			this.imAttendingButton.Hidden = true;
			
			this.facebookButton.SetBackgroundImage (UIImage.FromBundle (@"images/facebook-48x48"), UIControlState.Normal);
			this.twitterButton.SetBackgroundImage (UIImage.FromBundle (@"images/twitter-48x48"), UIControlState.Normal);
			this.imAttendingButton.SetBackgroundImage (UIImage.FromBundle (@"images/ImAttendingButtonBackground"), UIControlState.Normal);
			this.imAttendingButton.SetTitle ("          I'm Attending", UIControlState.Normal);
			this.imAttendingButton.SetTitleColor (UIColor.Black, UIControlState.Normal);
			this.imAttendingButton.Font = UIFont.FromName ("OpenSans", 14f);
		}

		void TwitterTouched (object sender, EventArgs e)
		{
			TrackAnalyticsEvent ("AttendingSessionTweeted-" + NavigationItems.ConferenceSlug + "-" + _sessionSlug);
		}
		
		void FacebookTouched (object sender, EventArgs e)
		{
			TrackAnalyticsEvent ("AttendingSessionPostedToFacebook-" + NavigationItems.ConferenceSlug + "-" + _sessionSlug);
		}
		
		void ImAttendingTouched (object sender, EventArgs e)
		{
			if (this.IsReachable ()) {
				
				
				if (_accountStore == null)
					_accountStore = new ACAccountStore ();
				
				var options = new AccountStoreOptions () {
					FacebookAppId = AppId
				};
				
				options.SetPermissions (ACFacebookAudience.OnlyMe, new string[]{ "email" });
				var accountType = _accountStore.FindAccountType (ACAccountType.Facebook);
				
				_accountStore.RequestAccess (accountType, options, (granted, error) => {
					if (granted) {
						var facebookAccount = _accountStore.FindAccounts (accountType).First ();
						var oAuthToken = facebookAccount.Credential.OAuthToken;
						UIAlertView loading = null;
						InvokeOnMainThread (() => 
						                    { 
							loading = new UIAlertView (" Saving Schedule", "Please wait...", null, null, null);
							
							loading.Show ();
							
							var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
							indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
							indicator.StartAnimating (); 
							loading.AddSubview (indicator);
						});
						
						Repository.AddSessionToSchedule (conferenceSlug: NavigationItems.ConferenceSlug,
						                                 sessionSlug : _sessionSlug, 
						                                 userName: facebookAccount.Username, 
						                                 callback: schedule => 
						                                 { 	
							InvokeOnMainThread (() => 
							                    { 
								loading.DismissWithClickedButtonIndex (0, true);
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

			TrackAnalyticsEvent ("AttendingSession-" + NavigationItems.ConferenceSlug + "-" + _sessionSlug);
		}

		void SetTitle ()
		{
			var font = UIFont.FromName ("OpenSans-Light", 22f);
			this.titleLabel.Font = font;
			this.titleLabel.Text = _session.title;
			this.titleLabel.Lines = 0;
			this.titleLabel.SizeToFit ();
		}

		void SetSeparatorBelowTitle ()
		{
			var size = this.titleLabel.StringSize (this.titleLabel.Text, this.titleLabel.Font);
			var font = UIFont.FromName ("OpenSans-Bold", 24f);
			//this.separatorBelowTitle.Font = font;
			var frame = this.separatorBelowTitle.Frame;
			//frame.Y = this.titleLabel.Frame.Y + size.Height + 20;
			frame.Y = this.titleLabel.Frame.Y + this.titleLabel.Frame.Height + 5;
			
			this.separatorBelowTitle.Frame = frame;
		}

		void SetStart ()
		{
			this.startLabel.Text = _session.startDescription;
			var frame = this.startLabel.Frame;
			frame.Y = this.separatorBelowTitle.Frame.Y + this.separatorBelowTitle.Frame.Height + 5;
			this.startLabel.Frame = frame;
		}

		void SetRoom ()
		{
			if (string.IsNullOrEmpty (_session.room)) {
				this.roomLabel.Text = "No room set";
			} else {
				this.roomLabel.Text = _session.room;
			}

			var frame = this.roomLabel.Frame;
			frame.Y = this.startLabel.Frame.Y + this.startLabel.Frame.Height + 10;
			this.roomLabel.Frame = frame;
		}

		void SetSeparatorBelowRoom ()
		{
			var frame = this.separatorBelowRoom.Frame;
			frame.Y = this.roomLabel.Frame.Y + this.roomLabel.Frame.Height + 5;
			this.separatorBelowRoom.Frame = frame;
		}

		void SetDescription ()
		{
			this.descriptionLabel.Text = _session.description.Trim ();
			this.descriptionLabel.Lines = 0;
			this.descriptionLabel.SizeToFit ();

			var frame = this.descriptionLabel.Frame;

			frame.Y = this.imAttendingButton.Frame.Y + this.imAttendingButton.Frame.Height + 15;

			this.descriptionLabel.Frame = frame;
		}

		void SetMoreInformation ()
		{
			this.moreInformationView.BackgroundColor = UIColor.FromRGBA (red: 0.933f, green: 0.933f, blue: 0.933f, alpha: 1f);
			var frame = this.moreInformationView.Frame;

			if (this.titleLabel.Frame.Height > 40) {
				frame.Y = this.descriptionLabel.Frame.Y + this.descriptionLabel.Frame.Height - 25;
			} else {
				frame.Y = this.descriptionLabel.Frame.Y + this.descriptionLabel.Frame.Height + 10;
			}
			this.moreInformationView.Frame = frame;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			LoadSession ();

			if (_session != null) {
				TrackAnalyticsEvent ("SessionDetailAboutViewController-" + _session.slug);
			}
		}

		private void LoadSession ()
		{
			if (this.IsReachable ()) {
				var loading = new UIAlertView (" Downloading Session", "Please wait...", null, null, null);
				
				loading.Show ();
				
				var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
				indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
				indicator.StartAnimating (); 
				loading.AddSubview (indicator);
				
				Repository.GetSession (NavigationItems.ConferenceSlug, _sessionSlug, session => 
				{ 
					InvokeOnMainThread (() => 
					{ 
						if (session != null) {
							_session = session;
							SetTitle ();
							SetSeparatorBelowTitle ();
							SetStart ();
							SetRoom ();
							SetSeparatorBelowRoom ();
							SetButtons ();
							SetDescription ();
							SetMoreInformation ();
						} else {
							var notFound = new UIAlertView ("Not found", "Session not found", null, "OK", null);
							notFound.Show ();
						}
						
						loading.DismissWithClickedButtonIndex (0, true); 
					});
					
					if (session != null) {
						TrackAnalyticsEvent ("SessionDetailAboutViewController-" + session.slug);
					}
				});
			} else {
				UnreachableAlert ().Show ();
			}
			
			
		}

		void SetButtons ()
		{
			
			this.facebookButton.Hidden = false;
			this.twitterButton.Hidden = false;
			this.imAttendingButton.Hidden = false;
			
			this.facebookButton.SetBackgroundImage (UIImage.FromBundle (@"images/facebook-48x48"), UIControlState.Normal);
			this.twitterButton.SetBackgroundImage (UIImage.FromBundle (@"images/twitter-48x48"), UIControlState.Normal);
			
			var imAttendingFrame = this.imAttendingButton.Frame;
			var facebookFrame = this.facebookButton.Frame;
			var twitterFrame = this.twitterButton.Frame;
			

			imAttendingFrame.Y = this.separatorBelowRoom.Frame.Y + 30;
			facebookFrame.Y = this.separatorBelowRoom.Frame.Y + 30;
			twitterFrame.Y = this.separatorBelowRoom.Frame.Y + 30;

			this.imAttendingButton.Frame = imAttendingFrame;
			this.facebookButton.Frame = facebookFrame;
			this.twitterButton.Frame = twitterFrame;
			
		}

		[Obsolete]
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

