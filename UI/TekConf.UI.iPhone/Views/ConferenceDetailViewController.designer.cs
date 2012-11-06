// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace TekConf.UI.iPhone
{
	[Register ("ConferenceDetailViewController")]
	partial class ConferenceDetailViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView sessionsTableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (sessionsTableView != null) {
				sessionsTableView.Dispose ();
				sessionsTableView = null;
			}
		}
	}
}
