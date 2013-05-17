using System;

namespace TekConf.UI.iPhone
{
	public static class StringExtensions
	{
		/// <summary>
		/// Safely replace all occurrences of specified System.String instance, with another System.String instance
		/// </summary>
		public static string SafeReplace(this string extendedString, string oldValue, string newValue)
		{
			if (string.IsNullOrEmpty(extendedString))
			{
				return extendedString;
			}
			
			return extendedString.Replace(oldValue, newValue);
		}
	}
}

