using System;
using System.Linq;

namespace TekConf.Common.Entities
{
	public static class Extensions
	{
		public static bool IsNullOrWhiteSpace(this string value)
		{
			return string.IsNullOrWhiteSpace(value);
		}
	}

	public static class TypeExtensions
	{
		public static string ToGenericTypeString(this Type t)
		{
			if (!t.IsGenericType)
				return t.Name;
			string genericTypeName = t.GetGenericTypeDefinition().Name;
			genericTypeName = genericTypeName.Substring(0,
					genericTypeName.IndexOf('`'));
			string genericArgs = string.Join(",",
					t.GetGenericArguments()
							.Select(ta => ToGenericTypeString(ta)).ToArray());
			return genericTypeName + "<" + genericArgs + ">";
		}
	}
}
