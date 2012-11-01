
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.iPhone
{
	public partial class SessionDetailAboutViewController : BaseUIViewController
	{
		private FullSessionDto _session;
		public SessionDetailAboutViewController (FullSessionDto session) : this()
		{
			_session = session;
		}

		public SessionDetailAboutViewController () : base ("SessionDetailAboutViewController", null)
		{
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
			this.contentDetailScrollView.ContentSize = new SizeF(width:this.View.Frame.Width, height:600);
			this.contentDetailScrollView.ScrollEnabled = true;
			this.contentDetailScrollView.ClipsToBounds = true;
			this.contentDetailScrollView.ContentInset = new UIEdgeInsets(top:0, left:0, bottom:900, right:0);


			this.titleLabel.Text = _session.title;
			this.titleLabel.Lines = 0;
			this.titleLabel.SizeToFit();
			
			this.startLabel.Text = _session.startDescription;
			this.startLabel.Frame.Location = new PointF(y: this.titleLabel.Frame.Location.Y + this.titleLabel.Frame.Height + 20, x: this.titleLabel.Frame.Location.X);
			
			this.roomLabel.Text = _session.room;
			this.roomLabel.Frame.Location = new PointF(x: this.startLabel.Frame.Bottom + 44, y: this.startLabel.Frame.Location.Y);
			
			this.descriptionLabel.Text = _session.description;
			this.descriptionLabel.Lines  = 0;
			this.descriptionLabel.SizeToFit();
			this.descriptionLabel.Frame.Location = new PointF(x: this.roomLabel.Frame.Bottom + 44, y: this.roomLabel.Frame.Location.Y);
			
			this.moreInformationView.Frame.Location = new PointF(x: this.descriptionLabel.Frame.Bottom + 60, y: this.descriptionLabel.Frame.Location.Y);
			this.moreInformationView.BackgroundColor = UIColor.FromRGBA(red: 0.933f, green: 0.933f, blue: 0.933f, alpha: 1f);

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}

