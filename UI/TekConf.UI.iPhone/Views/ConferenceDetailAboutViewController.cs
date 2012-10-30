
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
		private FullConferenceDto _conference;

		public ConferenceDetailAboutViewController (FullConferenceDto conference) : base ("ConferenceDetailAboutViewController", null)
		{
			_conference = conference;
			Title = _conference.name;
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

				this.descriptionLabel.Text = _conference.description;

				this.descriptionLabel.Lines  = 0;

				this.descriptionLabel.SizeToFit();

				this.detailsContainerView.Frame.Location = new PointF(x: this.descriptionLabel.Frame.Bottom + 20, y: this.descriptionLabel.Frame.Location.Y);
				this.detailsContainerView.BackgroundColor = UIColor.FromRGBA(red: 0.933f, green: 0.933f, blue: 0.933f, alpha: 1f);
				//[UIColor colorWithRed:0.933 green:0.933 blue:0.933 alpha:1] /*#eeeeee*/
				this.startLabel.Text = _conference.CalculateConferenceDates(_conference);
				this.taglineLabel.Text = _conference.tagline;
				var font = UIFont.FromName("OpenSans", 15f);
				this.taglineLabel.Font = font;
				this.twitterHashTagLabel.Text = _conference.twitterHashTag;
				this.twitterNameLabel.Text = _conference.twitterName;

				if (!string.IsNullOrWhiteSpace(_conference.imageUrl))
				{
					var logo = ImageLoader.DefaultRequestImage(new Uri("http://www.tekconf.com" + _conference.imageUrl), this);
					if(logo == null)
					{
						//logoImage.Image = DefaultImage;
					}
					else 
					{
						logoImage.Image = logo;
					}
				}
//					conference.address;
//					conference.description;
//					conference.end;
//					conference.facebookUrl;
//					conference.googlePlusUrl;
//					conference.homepageUrl;
//					conference.imageUrl;
//					conference.name;
//					conference.registrationOpens;
//					conference.registrationCloses;
//					conference.start;
//					conference.tagline;
//					conference.twitterHashTag;
//					conference.twitterName;

				loading.DismissWithClickedButtonIndex (0, true); 
			});

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

