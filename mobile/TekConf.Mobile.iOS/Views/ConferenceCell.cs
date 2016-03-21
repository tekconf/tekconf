using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using TekConf.Mobile.Core.ViewModels;
using UIKit;

namespace TekConf.Mobile.iOS
{
	public partial class ConferenceCell : MvxTableViewCell
	{
		public static readonly NSString Key = new NSString("ConferenceCell");
		public static readonly UINib Nib;

		static ConferenceCell()
		{
			Nib = UINib.FromName("ConferenceCell", NSBundle.MainBundle);
		}

		protected ConferenceCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
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
