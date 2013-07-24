using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.WindowsPhone.Converters;
using Cirrious.MvvmCross.Plugins.Visibility;
using TekConf.Core.ValueConverters;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.WinPhone.ValueConverters
{
	public class NativeBitmapImageValueConverter : MvxNativeValueConverter<BitmapImageValueConverter>
	{
	}

	public class NativeSocialImageValueConverter : MvxNativeValueConverter<SocialImageValueConverter>
	{

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

	public class NativeConferenceFavoriteValueConverter : MvxNativeValueConverter<ConferenceFavoriteValueConverter>
	{
		
	}
	public class ConferenceFavoriteValueConverter : MvxValueConverter<FullConferenceDto, string>
	{
		protected override string Convert(FullConferenceDto value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.isAddedToSchedule == true)
				return "/img/appbar.heart.cross.png";
			
			return "/img/appbar.heart.png";
		}
	}

	public class SocialImageValueConverter : MvxValueConverter<string, string>
	{
		protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return "/img/social/" + value + ".png";
		}
	}

	public class BitmapImageValueConverter : MvxValueConverter<byte[], BitmapImage>
	{
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
			catch (Exception exception)
			{
#if DEBUG
				throw;
#endif
			}

			return bitmapImage;
		}
	}


}
