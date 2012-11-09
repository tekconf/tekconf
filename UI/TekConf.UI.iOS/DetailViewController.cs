using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace TekConf.UI.iOS
{
	public partial class DetailViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		UIPopoverController popoverController;
		string detailItem;
		
		[Export("detailItem")]
		public string DetailItem {
			get {
				return detailItem;
			}
			set {
				SetDetailItem (value);
			}
		}
		
		public DetailViewController (IntPtr handle) : base (handle)
		{
		}
		
		public void SetDetailItem (string newDetailItem)
		{
			if (detailItem != newDetailItem) {
				detailItem = newDetailItem;
				
				// Update the view
				ConfigureView ();
			}
			
			if (this.popoverController != null)
				this.popoverController.Dismiss (true);
		}
		
		void ConfigureView ()
		{
			// Update the user interface for the detail item
			if (DetailItem != null)
				this.detailDescriptionLabel.Text = DetailItem.ToString ();
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		#region View lifecycle
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();
			
			if (!UserInterfaceIdiomIsPhone)
				SplitViewController.Delegate = new SplitViewControllerDelegate ();
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Release any retained subviews of the main view.
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
		
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}
		
		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		
		#endregion
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UserInterfaceIdiomIsPhone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}
		
		#region Split View
		
		class SplitViewControllerDelegate : UISplitViewControllerDelegate
		{
			public override void WillHideViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
			{
				var dv = svc.ViewControllers [1] as DetailViewController;
				barButtonItem.Title = "Master";
				var items = new List<UIBarButtonItem> ();
				items.Add (barButtonItem);
				items.AddRange (dv.toolbar.Items);
				dv.toolbar.SetItems (items.ToArray (), true);
				dv.popoverController = pc;
			}
			
			public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
			{
				var dv = svc.ViewControllers [1] as DetailViewController;
				var items = new List<UIBarButtonItem> (dv.toolbar.Items);
				items.RemoveAt (0);
				dv.toolbar.SetItems (items.ToArray (), true);
				dv.popoverController = null;
			}
		}
		
		#endregion
	}
}

