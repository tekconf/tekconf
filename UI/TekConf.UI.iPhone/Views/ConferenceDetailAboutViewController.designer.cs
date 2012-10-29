// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace TekConf.UI.iPhone
{
	[Register ("ConferenceDetailAboutViewController")]
	partial class ConferenceDetailAboutViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel nameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel descriptionLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel taglineLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel startLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel endLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel twitterNameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel twitterHashTagLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView logoImage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (nameLabel != null) {
				nameLabel.Dispose ();
				nameLabel = null;
			}

			if (descriptionLabel != null) {
				descriptionLabel.Dispose ();
				descriptionLabel = null;
			}

			if (taglineLabel != null) {
				taglineLabel.Dispose ();
				taglineLabel = null;
			}

			if (startLabel != null) {
				startLabel.Dispose ();
				startLabel = null;
			}

			if (endLabel != null) {
				endLabel.Dispose ();
				endLabel = null;
			}

			if (twitterNameLabel != null) {
				twitterNameLabel.Dispose ();
				twitterNameLabel = null;
			}

			if (twitterHashTagLabel != null) {
				twitterHashTagLabel.Dispose ();
				twitterHashTagLabel = null;
			}

			if (logoImage != null) {
				logoImage.Dispose ();
				logoImage = null;
			}
		}
	}
}
