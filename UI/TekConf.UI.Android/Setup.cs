using Android.Content;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace TekConf.UI.Android
{
		public class Setup : MvxAndroidSetup
		{
				public Setup(Context applicationContext) : base(applicationContext)
				{
				}

				protected override IMvxApplication CreateApp()
				{
					MvxBindingTrace.TraceBindingLevel = MvxTraceLevel.Diagnostic;
						return new TekConf.Core.App();
				}
		}
}