// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace ConferencesIO.UI.iOS
{
	[Register ("SpeakerDetailViewController")]
	partial class SpeakerDetailViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel speakerNameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (speakerNameLabel != null) {
				speakerNameLabel.Dispose ();
				speakerNameLabel = null;
			}
		}
	}
}
