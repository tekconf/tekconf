// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace TekConf.UI.iPhone
{
	[Register ("ConferenceDetailSpeakersViewController")]
	partial class ConferenceDetailSpeakersViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView speakersTableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (speakersTableView != null) {
				speakersTableView.Dispose ();
				speakersTableView = null;
			}
		}
	}
}
