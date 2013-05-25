using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Plugins.File;
using Newtonsoft.Json;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public interface ILocalNotificationsRepository
	{
		bool IsOptedInToNotifications { get; set; }
	}

	public class LocalNotificationsRepository : ILocalNotificationsRepository
	{
		private readonly IMvxFileStore _fileStore;
		private readonly IAuthentication _authentication;
		private const string _path = "notifications.json";

		public LocalNotificationsRepository(IMvxFileStore fileStore, IAuthentication authentication)
		{
			_fileStore = fileStore;
			_authentication = authentication;
		}

		public bool IsOptedInToNotifications
		{
			get
			{
				if (_authentication.IsAuthenticated && _fileStore.Exists(_path))
				{
					string json;
					if (_fileStore.TryReadTextFile(_path, out json))
					{
						var isOptedIn = JsonConvert.DeserializeObject<bool>(json);
						return isOptedIn;
					}
				}

				return false;
			}
			set
			{
				var isOptedIn = JsonConvert.SerializeObject(value);
				_fileStore.WriteFile(_path, isOptedIn);
			}
		}
	}


}