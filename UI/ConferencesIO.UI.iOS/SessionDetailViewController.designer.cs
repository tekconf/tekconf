// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace ConferencesIO.UI.iOS
{
	[Register ("SessionDetailViewController")]
	partial class SessionDetailViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel sessionTitleLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel sessionStartLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel sessionEndLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel sessionTwitterHashTagLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (sessionTitleLabel != null) {
				sessionTitleLabel.Dispose ();
				sessionTitleLabel = null;
			}

			if (sessionStartLabel != null) {
				sessionStartLabel.Dispose ();
				sessionStartLabel = null;
			}

			if (sessionEndLabel != null) {
				sessionEndLabel.Dispose ();
				sessionEndLabel = null;
			}

			if (sessionTwitterHashTagLabel != null) {
				sessionTwitterHashTagLabel.Dispose ();
				sessionTwitterHashTagLabel = null;
			}
		}
	}
}
