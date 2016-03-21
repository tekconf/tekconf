using Foundation;
using System;
using UIKit;
using MvvmCross.iOS.Views;
using TekConf.Mobile.Core.ViewModels;

namespace TekConf.Mobile.iOS
{
	[MvxFromStoryboard("ConferencesStoryboard")]
	public partial class SettingsViewController : MvxViewController<SettingsViewModel>, IMvxModalIosView
    {
        public SettingsViewController (IntPtr handle) : base (handle)
        {
        }
    }
}