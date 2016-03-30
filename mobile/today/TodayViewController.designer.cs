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

namespace today
{
    [Register ("TodayViewController")]
    partial class TodayViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel status { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (status != null) {
                status.Dispose ();
                status = null;
            }
        }
    }
}