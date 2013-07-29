using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TekConf.Core.Models
{
	public static class Extensions
	{
		public static bool IsNotNull(this object value)
		{
			return value != null;
		}

		public static bool IsNull(this object value)
		{
			return value == null;
		}

		public static bool IsNullOrWhiteSpace(this string value)
		{
			return string.IsNullOrWhiteSpace(value);
		}

		public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
		{
			var col = new ObservableCollection<T>();
			foreach (var cur in enumerable)
			{
				col.Add(cur);
			}
			return col;
		}
	}
}