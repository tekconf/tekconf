using System;
using System.Linq;
using TekConf.UI.Api;

namespace TekConf.Common.Entities
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

		public static void TrimAllProperties<T>(this T entity) where T : IEntity
		{
			var stringProperties = entity.GetType().GetProperties()
													.Where(p => p.PropertyType == typeof(string));

			foreach (var stringProperty in stringProperties)
			{
				var currentValue = (string)stringProperty.GetValue(entity, null);
				if (!currentValue.IsNullOrWhiteSpace() && stringProperty.CanWrite)
				{
					stringProperty.SetValue(entity, currentValue.Trim().Substring(0, currentValue.Length > 5000 ? 5000 : currentValue.Length), null);
				}
			}
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
