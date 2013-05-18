using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.WindowsPhone.Converters;
using Cirrious.MvvmCross.Plugins.Visibility;
using TekConf.Core.ValueConverters;

namespace TekConf.UI.WinPhone.ValueConverters
{
	public class NativeBitmapImageValueConverter : MvxNativeValueConverter<BitmapImageValueConverter>
	{
		
	}

	public class BitmapImageValueConverter : MvxValueConverter<byte[], BitmapImage>
	{
		protected override byte[] ConvertBack(BitmapImage value, Type targetType, object parameter, CultureInfo culture)
		{
			return base.ConvertBack(value, targetType, parameter, culture);
		}

		protected override BitmapImage Convert(byte[] value, Type targetType, object parameter, CultureInfo culture)
		{

			var bitmapImage = new BitmapImage();
			try
			{
				if (value != null)
				{
					var ms = new MemoryStream(value);
					bitmapImage.SetSource(ms);
				}
			}
			catch (Exception)
			{

			}


			return bitmapImage;
		}
	}

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
