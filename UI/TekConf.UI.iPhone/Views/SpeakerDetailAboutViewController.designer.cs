// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace TekConf.UI.iPhone
{
	[Register ("SpeakerDetailAboutViewController")]
	partial class SpeakerDetailAboutViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel fullNameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel descriptionLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel emailAddressLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel facebookLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel googlePlusLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel twitterNameLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView contentDetailsScrollView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView profileImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView moreInformationView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel separatorBelowName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (fullNameLabel != null) {
				fullNameLabel.Dispose ();
				fullNameLabel = null;
			}

			if (descriptionLabel != null) {
				descriptionLabel.Dispose ();
				descriptionLabel = null;
			}

			if (emailAddressLabel != null) {
				emailAddressLabel.Dispose ();
				emailAddressLabel = null;
			}

			if (facebookLabel != null) {
				facebookLabel.Dispose ();
				facebookLabel = null;
			}

			if (googlePlusLabel != null) {
				googlePlusLabel.Dispose ();
				googlePlusLabel = null;
			}

			if (twitterNameLabel != null) {
				twitterNameLabel.Dispose ();
				twitterNameLabel = null;
			}

			if (contentDetailsScrollView != null) {
				contentDetailsScrollView.Dispose ();
				contentDetailsScrollView = null;
			}

			if (profileImageView != null) {
				profileImageView.Dispose ();
				profileImageView = null;
			}

			if (moreInformationView != null) {
				moreInformationView.Dispose ();
				moreInformationView = null;
			}

			if (separatorBelowName != null) {
				separatorBelowName.Dispose ();
				separatorBelowName = null;
			}
		}
	}
}
