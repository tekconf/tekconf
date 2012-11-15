using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;
using MonoTouch.Dialog.Utilities;

namespace TekConf.UI.iPhone
{
	public partial class ConferenceDetailAboutViewController : BaseUIViewController, IImageUpdated
	{
		public ConferenceDetailAboutViewController () : base ("ConferenceDetailAboutViewController", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.contentScrollView.ContentSize = new SizeF (width: this.View.Frame.Width, height: 600);
			this.contentScrollView.ScrollEnabled = true;
			this.contentScrollView.ClipsToBounds = true;
			this.contentScrollView.ContentInset = new UIEdgeInsets (top: -30, left: 0, bottom: 900, right: 0);
			this.attendingButton.TouchUpInside += ImAttendingTouched;
			this.facebookButton.TouchUpInside += FacebookTouched;
			this.twitterButton.TouchUpInside += TwitterTouched;

			this.facebookButton.Hidden = true;
			this.twitterButton.Hidden = true;
			this.attendingButton.Hidden = true;

			this.facebookButton.SetBackgroundImage(UIImage.FromBundle(@"images/facebook-48x48"), UIControlState.Normal);
			this.twitterButton.SetBackgroundImage(UIImage.FromBundle(@"images/twitter-48x48"), UIControlState.Normal);
			this.attendingButton.SetBackgroundImage(UIImage.FromBundle(@"images/ImAttendingButtonBackground"), UIControlState.Normal);
			this.attendingButton.SetTitle("          I'm Attending", UIControlState.Normal);
			this.attendingButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
			this.attendingButton.Font = UIFont.FromName("OpenSans", 14f);
		}

		void TwitterTouched (object sender, EventArgs e)
		{

			TrackAnalyticsEvent("AttendingTweeted-" + NavigationItems.ConferenceSlug);
		}

		void FacebookTouched (object sender, EventArgs e)
		{
			TrackAnalyticsEvent("AttendingPostedToFacebook-" + NavigationItems.ConferenceSlug);
		}

		void ImAttendingTouched (object sender, EventArgs e)
		{
			TrackAnalyticsEvent("Attending-" + NavigationItems.ConferenceSlug);
		}

		public override void LoadView ()
		{
			base.LoadView ();

		}

		private void LoadConference ()
		{
			if (this.IsReachable ()) {
				var loading = new UIAlertView (" Downloading Conference", "Please wait...", null, null, null);
				
				loading.Show ();
				
				var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
				indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
				indicator.StartAnimating (); 
				loading.AddSubview (indicator);

				var conferenceSlug = NavigationItems.ConferenceSlug;
				Repository.GetConference (conferenceSlug, conference => 
				{ 
					NavigationItems.Conference = conference;
					InvokeOnMainThread (() => 
					{ 
						if (conference != null) {
							if (!string.IsNullOrWhiteSpace (conference.imageUrl)) {
								var logo = ImageLoader.DefaultRequestImage (new Uri ("http://www.tekconf.com" + conference.imageUrl), this);
								if (logo == null) {
									logoImage.Image = UIImage.FromBundle (@"images/DefaultConference");
								} else {
									logoImage.Image = logo;
								}
							}
						
							SetTagLine (conference);
							SetButtons ();
							SetStartLabel (conference);
							SetDescriptionLabel (conference);
							SetDetailsContainer (conference);
						}
						else
						{
							var notFound = new UIAlertView("Not found", "Conference not found", null, "OK", null);
							notFound.Show();
						}
						
						loading.DismissWithClickedButtonIndex (0, true); 
					});

					if (conference != null) {
						TrackAnalyticsEvent ("ConferenceDetailAboutViewController-" + conference.slug);
					}
				});
			} else {
				UnreachableAlert ().Show ();
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			LoadConference ();
		}

		void SetTagLine (FullConferenceDto conference)
		{
			var font = UIFont.FromName ("OpenSans", 15f);
			this.taglineLabel.Font = font;
				this.tagLineSeparatorBottom.Text = "........................................................................";
				this.tagLineSeparatorTop.Text = "........................................................................";
			
			if (!string.IsNullOrWhiteSpace (conference.tagline)) {
				this.taglineLabel.Text = conference.tagline;
				this.taglineLabel.Hidden = false;
				

				this.tagLineSeparatorBottom.Hidden = false;
				this.tagLineSeparatorTop.Hidden = false;

			} else {
				this.tagLineSeparatorTop.Hidden = false;
				this.tagLineSeparatorBottom.Hidden = true;
				this.taglineLabel.Hidden = true;
			}
		}

		void SetButtons()
		{

			this.facebookButton.Hidden = false;
			this.twitterButton.Hidden = false;
			this.attendingButton.Hidden = false;

			this.facebookButton.SetBackgroundImage(UIImage.FromBundle(@"images/facebook-48x48"), UIControlState.Normal);
			this.twitterButton.SetBackgroundImage(UIImage.FromBundle(@"images/twitter-48x48"), UIControlState.Normal);

			var imAttendingFrame = this.attendingButton.Frame;
			var facebookFrame = this.facebookButton.Frame;
			var twitterFrame = this.twitterButton.Frame;

			if (this.tagLineSeparatorBottom.Hidden) {
				imAttendingFrame.Y = this.tagLineSeparatorTop.Frame.Y + 30;
				facebookFrame.Y = this.tagLineSeparatorTop.Frame.Y + 30;
				twitterFrame.Y = this.tagLineSeparatorTop.Frame.Y + 30;
			} else {
				imAttendingFrame.Y = this.tagLineSeparatorBottom.Frame.Y + 30;
				facebookFrame.Y = this.tagLineSeparatorBottom.Frame.Y + 30;
				twitterFrame.Y = this.tagLineSeparatorBottom.Frame.Y + 30;
			}
			
			this.attendingButton.Frame = imAttendingFrame;
			this.facebookButton.Frame = facebookFrame;
			this.twitterButton.Frame = twitterFrame;

		}

		void SetStartLabel (FullConferenceDto conference)
		{
			this.startLabel.Text = conference.CalculateConferenceDates (conference);
			var frame = this.startLabel.Frame;

			frame.Y = this.attendingButton.Frame.Y + this.attendingButton.Frame.Height + 15;

			this.startLabel.Frame = frame;
		}

		void SetDescriptionLabel (FullConferenceDto conference)
		{
			this.descriptionLabel.Text = conference.description;
			this.descriptionLabel.Lines = 0;
			this.descriptionLabel.SizeToFit ();
			var frame = this.descriptionLabel.Frame;
			frame.Y = this.startLabel.Frame.Y + 30;
			this.descriptionLabel.Frame = frame;
		}

		void SetDetailsContainer (FullConferenceDto conference)
		{
			if (string.IsNullOrWhiteSpace (conference.twitterHashTag) 
				&& string.IsNullOrWhiteSpace (conference.twitterName)
				&& string.IsNullOrWhiteSpace (conference.homepageUrl)
				&& string.IsNullOrWhiteSpace (conference.facebookUrl)
			    
			    ) {
				this.detailsContainerView.Hidden = true;
			} else {
				this.detailsSlashesLabel.TextColor = UIColor.FromRGBA (red: 0.506f, 
				                                                      green: 0.6f, 
				                                                      blue: 0.302f, 
				                                                      alpha: 1f);

				var font = UIFont.FromName ("OpenSans-Bold", 14f);
				this.detailsSlashesLabel.Font = font;
				//this.moreInformationLabel.Font = font;

				this.detailsContainerView.Hidden = false;

				var frame = this.detailsContainerView.Frame;
				frame.Y = this.descriptionLabel.Frame.Location.Y + this.descriptionLabel.Frame.Height + 10;
				this.detailsContainerView.Frame = frame;
				this.detailsContainerView.BackgroundColor = UIColor.FromRGBA (red: 0.933f, green: 0.933f, blue: 0.933f, alpha: 1f);

				if (string.IsNullOrWhiteSpace (conference.twitterHashTag)) {
					this.twitterHashTagImage.Hidden = true;
					this.twitterHashTagLabel.Hidden = true;
				} else {
					this.twitterHashTagImage.Hidden = false;
					this.twitterHashTagLabel.Hidden = false;
					this.twitterHashTagImage.Image = UIImage.FromBundle (@"images/twitter-16x16");
					this.twitterHashTagLabel.Text = conference.twitterHashTag;
				}

				if (string.IsNullOrWhiteSpace (conference.twitterName)) {
					this.twitterNameImage.Hidden = true;
					this.twitterNameLabel.Hidden = true;
				} else {
					this.twitterNameImage.Hidden = false;
					this.twitterNameLabel.Hidden = false;

					this.twitterNameImage.Image = UIImage.FromBundle (@"images/twitter-16x16");
					this.twitterNameLabel.Text = conference.twitterName;
				}

				if (string.IsNullOrWhiteSpace (conference.homepageUrl)) {
					this.websiteImage.Hidden = true;
					this.websiteLabel.Hidden = true;
				} else {
					this.websiteImage.Hidden = false;
					this.websiteLabel.Hidden = false;
					this.websiteImage.Image = UIImage.FromBundle (@"images/website-16x16");
					this.websiteLabel.Text = conference.homepageUrl.SafeReplace ("http://", "").SafeReplace ("https://", "");
				}

				if (string.IsNullOrWhiteSpace (conference.facebookUrl)) {
					this.facebookImage.Hidden = true;
					this.facebookLabel.Hidden = true;
				} else {
					this.facebookImage.Hidden = false;
					this.facebookLabel.Hidden = false;
					this.facebookImage.Image = UIImage.FromBundle (@"images/facebook-16x16");
					this.facebookLabel.Text = conference.facebookUrl.SafeReplace ("http://", "").SafeReplace ("https://", "");
					this.facebookLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
					this.facebookLabel.Lines = 0;
				}


			}

		}

		#region IImageUpdated implementation

		public void UpdatedImage (Uri uri)
		{
			logoImage.Image = ImageLoader.DefaultRequestImage (uri, this);
		}

		#endregion

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

