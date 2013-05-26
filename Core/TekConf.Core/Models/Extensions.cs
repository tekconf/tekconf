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
	}
}