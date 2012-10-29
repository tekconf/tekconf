
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TekConf.UI.iPhone
{
	public partial class ConferenceDetailAboutViewController : BaseUIViewController
	{
		private string conferenceSlug = "codemash-2013";

		public ConferenceDetailAboutViewController () : base ("ConferenceDetailAboutViewController", null)
		{
			Title = "About";
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var loading = new UIAlertView (" Downloading Conference", "Please wait...", null, null, null);
			//var fonts = UIFont.FontNamesForFamilyName("Open Sans");
			//var font = UIFont.FromName("OpenSans-Light", 16f);

//			this.nameLabel.Font = font;
//			this.descriptionLabel.Font = font;
//			this.endLabel.Font = font;
//			this.startLabel.Font = font;
//			this.taglineLabel.Font = font;
//			this.twitterHashTagLabel.Font = font;
//			this.twitterNameLabel.Font = font;

			loading.Show ();
			
			var indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge); 
			indicator.Center = new System.Drawing.PointF (loading.Bounds.Width / 2, loading.Bounds.Size.Height - 40); 
			indicator.StartAnimating (); 
			loading.AddSubview (indicator);
			
			Repository.GetConference (conferenceSlug, conference => 
			{ 
				InvokeOnMainThread (() => 
				{ 
					this.nameLabel.Text = conference.name;
					this.descriptionLabel.Text = conference.description;
					this.endLabel.Text = conference.end.ToString ();
					this.startLabel.Text = conference.start.ToString();
					this.taglineLabel.Text = conference.tagline;
					this.twitterHashTagLabel.Text = conference.twitterHashTag;
					this.twitterNameLabel.Text = conference.twitterName;
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
			});


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

