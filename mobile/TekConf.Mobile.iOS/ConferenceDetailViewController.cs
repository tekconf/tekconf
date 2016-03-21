using Foundation;
using System;
using UIKit;
using MvvmCross.iOS.Views;
using TekConf.Mobile.Core.ViewModels;

namespace TekConf.Mobile.iOS
{
	[MvxFromStoryboard("ConferencesStoryboard")]
	public partial class ConferenceDetailViewController : MvxViewController<ConferenceDetailViewModel>
    {
        public ConferenceDetailViewController (IntPtr handle) : base (handle)
        {
        }
    }
}