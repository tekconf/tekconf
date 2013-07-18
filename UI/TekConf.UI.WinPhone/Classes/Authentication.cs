using TekConf.Core.Services;

namespace TekConf.UI.WinPhone.Bootstrap
{
	using Cirrious.MvvmCross.Plugins.Sqlite;

	using TekConf.Core.Entities;

	public class Authentication : IAuthentication
	{
		private readonly ISQLiteConnection _connection;

		private string _userName;

		public Authentication(ISQLiteConnection connection)
		{
			_connection = connection;
			_connection.CreateTable<UserEntity>();
			LoadUserName();
		}

		public bool IsAuthenticated { 
			get
			{
				return App.MobileService.CurrentUser != null || !string.IsNullOrWhiteSpace(UserName);
			}
		}

		public string OAuthProvider
		{
			get
			{
				if (IsAuthenticated)
					return App.MobileService.CurrentUser.UserId.Split(':')[0];
				
				return string.Empty;
			}
		}

		public string UserName
		{
			get
			{
				return _userName;
			}
			set
			{
				if (value != _userName)
				{
					_userName = value;
					SaveUserName(value);
				}
			}
		}

		private void LoadUserName()
		{
			var userEntity = _connection.Table<UserEntity>().FirstOrDefault();
			if (userEntity != null && !string.IsNullOrWhiteSpace(userEntity.UserName))
			{
				_userName = userEntity.UserName;
			}
		}

		private void SaveUserName(string value)
		{
			var userEntity = _connection.Table<UserEntity>().FirstOrDefault();
			if (userEntity != null)
			{
				_connection.Delete(userEntity);
			}

			if (!string.IsNullOrWhiteSpace(_userName))
			{
				var user = new UserEntity() { UserName = _userName };
				_connection.Insert(user);
			}
		}
	}
}
