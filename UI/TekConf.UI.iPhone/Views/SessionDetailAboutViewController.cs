
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


			SetTitle ();

			SetSeparatorBelowTitle ();
			
			SetStart ();
			
			SetRoom ();

			SetSeparatorBelowRoom ();
			
			SetDescription ();
			
			SetMoreInformation();
		}

		void SetTitle ()
		{
			var font = UIFont.FromName("OpenSans-Light", 22f);
			this.titleLabel.Font = font;
			this.titleLabel.Text = _session.title;
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
			this.startLabel.Text = _session.startDescription;
			var frame = this.startLabel.Frame;
			frame.Y = this.separatorBelowTitle.Frame.Y + this.separatorBelowTitle.Frame.Height + 5;
			this.startLabel.Frame = frame;
		}

		void SetRoom ()
		{
			if (string.IsNullOrEmpty(_session.room))
			{
				this.roomLabel.Text = "No room set";
			}
			else
			{
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
			this.descriptionLabel.Text = _session.description.Trim();
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

			if (_session != null)
			{
				NSError error;
				var success = GoogleAnalytics.GANTracker.SharedTracker.TrackPageView("SessionDetailAboutViewController-" + _session.slug, out error);
			}
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

