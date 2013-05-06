using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.CrossCore.WindowsPhone.Converters;
using Cirrious.MvvmCross.Plugins.Visibility;
using TekConf.Core.ValueConverters;

namespace TekConf.UI.WinPhone.ValueConverters
{
	public class NativeVisibilityConverter : MvxNativeValueConverter<MvxVisibilityValueConverter>
	{
	}

	public class NativeInvertedVisibilityConverter : MvxNativeValueConverter<MvxInvertedVisibilityValueConverter>
	{
	}

	public class NativeInverseBoolConverter : MvxNativeValueConverter<InverseBoolValueConverter>
	{
	}
}
