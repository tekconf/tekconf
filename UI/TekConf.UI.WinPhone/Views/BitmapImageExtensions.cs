﻿using System.IO;
using System.Windows.Media.Imaging;

namespace TekConf.UI.WinPhone.Views
{
	public static class BitmapImageExtensions
	{
		public static byte[] ConvertToBytes(this BitmapImage bitmapImage)
		{
			using (var ms = new MemoryStream())
			{
				var btmMap = new WriteableBitmap
					(bitmapImage.PixelWidth, bitmapImage.PixelHeight);

				// write an image into the stream
				btmMap.SaveJpeg(ms, bitmapImage.PixelWidth, bitmapImage.PixelHeight, 0, 100);

				return ms.ToArray();
			}
		}
	}
}