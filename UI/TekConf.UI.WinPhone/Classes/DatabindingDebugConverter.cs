﻿namespace TekConf.UI.WinPhone.Bootstrap
{
	using System;
	using System.Windows.Data;

	public class DatabindingDebugConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}
	}
}