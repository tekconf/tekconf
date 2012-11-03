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
		MonoTouch.UIKit.UILabel descriptionLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel taglineLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel startLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel twitterNameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel twitterHashTagLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView logoImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView contentScrollView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView detailsContainerView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel tagLineSeparatorTop { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel tagLineSeparatorBottom { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel detailsSlashesLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel moreInformationLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
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

			if (contentScrollView != null) {
				contentScrollView.Dispose ();
				contentScrollView = null;
			}

			if (detailsContainerView != null) {
				detailsContainerView.Dispose ();
				detailsContainerView = null;
			}

			if (tagLineSeparatorTop != null) {
				tagLineSeparatorTop.Dispose ();
				tagLineSeparatorTop = null;
			}

			if (tagLineSeparatorBottom != null) {
				tagLineSeparatorBottom.Dispose ();
				tagLineSeparatorBottom = null;
			}

			if (detailsSlashesLabel != null) {
				detailsSlashesLabel.Dispose ();
				detailsSlashesLabel = null;
			}

			if (moreInformationLabel != null) {
				moreInformationLabel.Dispose ();
				moreInformationLabel = null;
			}
		}
	}
}
