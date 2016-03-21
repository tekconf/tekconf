using Foundation;
using System;
using UIKit;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.BindingContext;
using TekConf.Mobile.Core.ViewModels;

namespace TekConf.Mobile.iOS
{
	public partial class ConferenceCell : MvxTableViewCell
    {
		public static readonly NSString Key = new NSString("ConferenceCell");

        public ConferenceCell (IntPtr handle) : base (handle)
        {
			this.DelayBind(() =>
			{
				//_imageViewLoader = new MvxImageViewLoader(() => this.image);

				var set = this.CreateBindingSet<ConferenceCell, ConferenceListViewModel>();
				set.Bind(name).To(vm => vm.Name);
				//set.Bind(description).To(vm => vm.Description);
				//set.Bind(scheduleStatus).To(vm => vm.IsAddedToSchedule).WithConversion("AddedToSchedule");
				//set.Bind(date).To(vm => vm.ShortDate);
				//set.Bind(location).To(vm => vm.Location);
				//set.Bind(highlightColor).For(v => v.BackgroundColor).To(vm => vm.HighlightColor).WithConversion("RGBA");
				//set.Bind(scheduleStatusView).For(v => v.BackgroundColor).To(vm => vm.HighlightColor).WithConversion("RGBA");
				//set.Bind(_imageViewLoader).To(item => item.ImageUrl);

				set.Apply();
			});
        }
    }
}