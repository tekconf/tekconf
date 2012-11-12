
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{
	public partial class SpeakerDetailSessionsViewController : BaseUIViewController
	{
		private FullSpeakerDto _speaker;
		public SpeakerDetailSessionsViewController (FullSpeakerDto speaker) : this()
		{
			_speaker = speaker;
		}

		public SpeakerDetailSessionsViewController () : base ("SpeakerDetailSessionsViewController", null)
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

			if (_speaker != null)
			{
				NSError error;
				var success = GoogleAnalytics.GANTracker.SharedTracker.TrackPageView("SpeakerDetailSessionsViewController-" + _speaker.slug, out error);
			}
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

