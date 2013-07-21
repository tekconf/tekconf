namespace TekConf.Core.Entities
{
	using Cirrious.MvvmCross.Plugins.Sqlite;

	public class UserEntity
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string UserName { get; set; }
	}
}