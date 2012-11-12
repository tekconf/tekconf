
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;
using MonoTouch.Dialog.Utilities;
using FA= FlurryAnalytics;
namespace TekConf.UI.iPhone
{
	public partial class ConferenceDetailAboutViewController : BaseUIViewController, IImageUpdated
	{
		private FullConferenceDto _conference;

		public ConferenceDetailAboutViewController (FullConferenceDto conference) : base ("ConferenceDetailAboutViewController", null)
		{
			_conference = conference;
			if (_conference != null) {
				Title = _conference.name;
			}
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.contentScrollView.ContentSize = new SizeF(width:this.View.Frame.Width, height:600);
			this.contentScrollView.ScrollEnabled = true;
			this.contentScrollView.ClipsToBounds = true;
			this.contentScrollView.ContentInset = new UIEdgeInsets(top:-30, left:0, bottom:900, right:0);
		}

		public override void LoadView ()
		{
			base.LoadView ();
			FA.FlurryAnalytics.LogAllPageViews(this);
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var loading = new UIAlertView (" Downloading Conference", "Please wait...", null, null, null);

			loading.Show ();
			
			var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
			indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
			indicator.StartAnimating (); 
			loading.AddSubview (indicator);

			InvokeOnMainThread (() => 
			{ 
				if (!string.IsNullOrWhiteSpace(_conference.imageUrl))
				{
					var logo = ImageLoader.DefaultRequestImage(new Uri("http://www.tekconf.com" + _conference.imageUrl), this);
					if(logo == null)
					{
						logoImage.Image = UIImage.FromBundle(@"images/DefaultConference");
					}
					else 
					{
						logoImage.Image = logo;
					}
				}

				SetTagLine ();
				SetStartLabel ();
				SetDescriptionLabel ();
				SetDetailsContainer ();


				loading.DismissWithClickedButtonIndex (0, true); 
			});

			if (_conference != null)
			{
				FA.FlurryAnalytics.LogEvent("ConferenceDetailAboutViewController-" + _conference.slug);


				NSError error;
				var success = GoogleAnalytics.GANTracker.SharedTracker.TrackPageView("ConferenceDetailAboutViewController-" + _conference.slug, out error);
			
				var tracker = new UIAlertView ("Tracked", success.ToString(), null, "Cancel", null);
				
				tracker.Show ();
			
			}

		}

		void SetTagLine ()
		{
			var font = UIFont.FromName ("OpenSans", 15f);
			this.taglineLabel.Font = font;

			if (!string.IsNullOrWhiteSpace (_conference.tagline)) {
				this.taglineLabel.Text = _conference.tagline;

				this.taglineLabel.Hidden = false;
				this.tagLineSeparatorBottom.Hidden = false;
				this.tagLineSeparatorTop.Hidden = false;

			} else {
				this.tagLineSeparatorTop.Hidden = false;
				this.tagLineSeparatorBottom.Hidden = true;
				this.taglineLabel.Hidden = true;
			}
		}

		void SetStartLabel ()
		{
			this.startLabel.Text = _conference.CalculateConferenceDates (_conference);
			var frame = this.startLabel.Frame;
			if (this.tagLineSeparatorBottom.Hidden) {
				frame.Y = this.tagLineSeparatorTop.Frame.Y + 30;
			} else {
				frame.Y = this.tagLineSeparatorBottom.Frame.Y + 30;
			}
			this.startLabel.Frame = frame;
		}

		void SetDescriptionLabel ()
		{
			this.descriptionLabel.Text = _conference.description;
			this.descriptionLabel.Lines = 0;
			this.descriptionLabel.SizeToFit ();
			var frame = this.descriptionLabel.Frame;
			frame.Y = this.startLabel.Frame.Y + 10;
			this.descriptionLabel.Frame = frame;
		}

		void SetDetailsContainer ()
		{
			if (string.IsNullOrWhiteSpace (_conference.twitterHashTag) 
			    && string.IsNullOrWhiteSpace (_conference.twitterName)
			    && string.IsNullOrWhiteSpace (_conference.homepageUrl)
			    && string.IsNullOrWhiteSpace (_conference.facebookUrl)
			    
			    ) {
				this.detailsContainerView.Hidden = true;
			} else {
				this.detailsSlashesLabel.TextColor = UIColor.FromRGBA(red:0.506f, 
				                                                      green:0.6f, 
				                                                      blue:0.302f, 
				                                                      alpha:1f);

				var font = UIFont.FromName("OpenSans-Bold", 14f);
				this.detailsSlashesLabel.Font = font;
				//this.moreInformationLabel.Font = font;

				this.detailsContainerView.Hidden = false;

				var frame = this.detailsContainerView.Frame;
				frame.Y = this.descriptionLabel.Frame.Location.Y + this.descriptionLabel.Frame.Height + 10;
				this.detailsContainerView.Frame = frame;
				this.detailsContainerView.BackgroundColor = UIColor.FromRGBA (red: 0.933f, green: 0.933f, blue: 0.933f, alpha: 1f);

				if (string.IsNullOrWhiteSpace(_conference.twitterHashTag))
				{
					this.twitterHashTagImage.Hidden = true;
					this.twitterHashTagLabel.Hidden = true;
				}
				else
				{
					this.twitterHashTagImage.Hidden = false;
					this.twitterHashTagLabel.Hidden = false;
					this.twitterHashTagImage.Image = UIImage.FromBundle(@"images/twitter-16x16");
					this.twitterHashTagLabel.Text = _conference.twitterHashTag;
				}

				if (string.IsNullOrWhiteSpace(_conference.twitterName))
				{
					this.twitterNameImage.Hidden = true;
					this.twitterNameLabel.Hidden = true;
				}
				else
				{
					this.twitterNameImage.Hidden = false;
					this.twitterNameLabel.Hidden = false;

					this.twitterNameImage.Image = UIImage.FromBundle(@"images/twitter-16x16");
					this.twitterNameLabel.Text = _conference.twitterName;
				}

				if (string.IsNullOrWhiteSpace(_conference.homepageUrl))
				{
					this.websiteImage.Hidden = true;
					this.websiteLabel.Hidden = true;
				}
				else
				{
					this.websiteImage.Hidden = false;
					this.websiteLabel.Hidden = false;
					this.websiteImage.Image = UIImage.FromBundle(@"images/website-16x16");
					this.websiteLabel.Text = _conference.homepageUrl.SafeReplace ("http://", "").SafeReplace("https://", "");
				}

				if (string.IsNullOrWhiteSpace(_conference.facebookUrl))
				{
					this.facebookImage.Hidden = true;
					this.facebookLabel.Hidden = true;
				}
				else
				{
					this.facebookImage.Hidden = false;
					this.facebookLabel.Hidden = false;
					this.facebookImage.Image = UIImage.FromBundle(@"images/facebook-16x16");
					this.facebookLabel.Text = _conference.facebookUrl.SafeReplace ("http://", "").SafeReplace("https://", "");
					this.facebookLabel.LineBreakMode = UILineBreakMode.CharacterWrap;
					this.facebookLabel.Lines = 0;
				}


			}

		}

		#region IImageUpdated implementation

		public void UpdatedImage (Uri uri)
		{
			logoImage.Image = ImageLoader.DefaultRequestImage(uri, this);
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

