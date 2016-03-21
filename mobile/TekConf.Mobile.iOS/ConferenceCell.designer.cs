// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TekConf.Mobile.iOS
{
    [Register ("ConferenceCell")]
    partial class ConferenceCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView contentView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel date { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel description { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView highlightColor { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView image { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel location { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel name { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (contentView != null) {
                contentView.Dispose ();
                contentView = null;
            }

            if (date != null) {
                date.Dispose ();
                date = null;
            }

            if (description != null) {
                description.Dispose ();
                description = null;
            }

            if (highlightColor != null) {
                highlightColor.Dispose ();
                highlightColor = null;
            }

            if (image != null) {
                image.Dispose ();
                image = null;
            }

            if (location != null) {
                location.Dispose ();
                location = null;
            }

            if (name != null) {
                name.Dispose ();
                name = null;
            }
        }
    }
}