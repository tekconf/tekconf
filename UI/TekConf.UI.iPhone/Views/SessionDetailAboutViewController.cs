
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
		private SessionDto _fullSession;
		private string _sessionSlug;
		private ACAccountStore _accountStore;
		private string AppId = "417883241605228";

		public SessionDetailAboutViewController (string sessionSlug) : this()
		{
			_sessionSlug = sessionSlug;
		}

		void HandleTouchUpInside (object sender, EventArgs e)
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
					if (granted) 
					{
						var facebookAccount = _accountStore.FindAccounts (accountType).First ();
						var oAuthToken = facebookAccount.Credential.OAuthToken;
						UIAlertView loading = null;
						InvokeOnMainThread (() => 
						{ 
							loading = new UIAlertView (" Downloading Schedule", "Please wait...", null, null, null);
							
							loading.Show ();
							
							var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
							indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
							indicator.StartAnimating (); 
							loading.AddSubview (indicator);
						});
						
						Repository.AddSessionToSchedule (conferenceSlug: NavigationItems.ConferenceSlug,
						                                 sessionSlug : _fullSession.slug,
						                                 authenticationMethod: "Facebook", 
						                                 authenticationToken: facebookAccount.Username, 
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

							var notLoggedInAlertView = new UIAlertView("Not logged in", "You must go to Settings and login to Facebook or Twitter before saving your schedule.", null, "OK", null);
							notLoggedInAlertView.Show();
						});
					}
				});
			} else {
				UnreachableAlert ().Show ();
			}

			TrackAnalyticsEvent ("AddSessionToSchedule-" + _fullSession.slug);
			
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

			this.contentDetailScrollView.ContentSize = new SizeF(width:this.View.Frame.Width, height:600);
			this.contentDetailScrollView.ScrollEnabled = true;
			this.contentDetailScrollView.ClipsToBounds = true;
			this.contentDetailScrollView.ContentInset = new UIEdgeInsets(top:0, left:0, bottom:900, right:0);
			this.addRemoveSessionFromSchedule.TouchUpInside += HandleTouchUpInside;
		}

		void SetTitle ()
		{
			var font = UIFont.FromName("OpenSans-Light", 22f);
			this.titleLabel.Font = font;
			this.titleLabel.Text = _fullSession.title;
			this.titleLabel.Lines = 0;
			this.titleLabel.SizeToFit ();
		}

		void SetSeparatorBelowTitle ()
		{
			var size = this.titleLabel.StringSize(this.titleLabel.Text, this.titleLabel.Font);
			var font = UIFont.FromName("OpenSans-Bold", 24f);
			//this.separatorBelowTitle.Font = font;
			var frame = this.separatorBelowTitle.Frame;
			//frame.Y = this.titleLabel.Frame.Y + size.Height + 20;
			frame.Y = this.titleLabel.Frame.Y + this.titleLabel.Frame.Height + 5;
			
			this.separatorBelowTitle.Frame = frame;
		}

		void SetStart ()
		{
			this.startLabel.Text = _fullSession.startDescription;
			var frame = this.startLabel.Frame;
			frame.Y = this.separatorBelowTitle.Frame.Y + this.separatorBelowTitle.Frame.Height + 5;
			this.startLabel.Frame = frame;
		}

		void SetRoom ()
		{
			if (string.IsNullOrEmpty(_fullSession.room))
			{
				this.roomLabel.Text = "No room set";
			}
			else
			{
				this.roomLabel.Text = _fullSession.room;
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
			this.descriptionLabel.Text = _fullSession.description.Trim();
			this.descriptionLabel.Lines = 0;
			this.descriptionLabel.SizeToFit ();

			var frame = this.descriptionLabel.Frame;
			if (this.titleLabel.Frame.Height > 40)
			{
				frame.Y  = this.separatorBelowRoom.Frame.Y + this.separatorBelowRoom.Frame.Height - 50;
			}
			else
			{
				frame.Y  = this.separatorBelowRoom.Frame.Y + this.separatorBelowRoom.Frame.Height + 5;
			}
			this.descriptionLabel.Frame = frame;
		}

		void SetMoreInformation()
		{
			this.moreInformationView.BackgroundColor = UIColor.FromRGBA(red: 0.933f, green: 0.933f, blue: 0.933f, alpha: 1f);
			var frame = this.moreInformationView.Frame;

			if (this.titleLabel.Frame.Height > 40)
			{
				frame.Y = this.descriptionLabel.Frame.Y + this.descriptionLabel.Frame.Height - 25;
			}
			else
			{
				frame.Y = this.descriptionLabel.Frame.Y + this.descriptionLabel.Frame.Height + 10;
			}
			this.moreInformationView.Frame = frame;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			LoadSession();

			if (_fullSession != null)
			{
				TrackAnalyticsEvent("SessionDetailAboutViewController-" + _fullSession.slug);
			}
		}

		private void LoadSession()
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
							SetTitle ();
							SetSeparatorBelowTitle ();
							SetStart ();
							SetRoom ();
							SetSeparatorBelowRoom ();
							SetDescription ();
							SetMoreInformation();
						}
						else
						{
							var notFound = new UIAlertView("Not found", "Session not found", null, "OK", null);
							notFound.Show();
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

