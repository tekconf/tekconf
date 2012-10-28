
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{
	public partial class SpeakerDetailAboutViewController : BaseUIViewController
	{
		private FullSpeakerDto _speaker;
		public SpeakerDetailAboutViewController (FullSpeakerDto speaker) : this()
		{
			_speaker = speaker;
		}

		public SpeakerDetailAboutViewController () : base ("SpeakerDetailAboutViewController", null)
		{
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
			this.fullNameLabel.Text = _speaker.fullName;
		
			this.descriptionLabel.Text = _speaker.description;
			this.emailAddressLabel.Text = _speaker.emailAddress;
			this.facebookLabel.Text = _speaker.facebookUrl;
			this.googlePlusLabel.Text = _speaker.googlePlusUrl;
			this.fullNameLabel.Text = _speaker.profileImageUrl;
			this.twitterNameLabel.Text = _speaker.twitterName;
			// Perform any additional setup after loading the view, typically from a nib.
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

