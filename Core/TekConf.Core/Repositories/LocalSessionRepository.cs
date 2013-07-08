//using Cirrious.MvvmCross.Plugins.File;
//using Newtonsoft.Json;
//using TekConf.RemoteData.Dtos.v1;

//namespace TekConf.Core.Repositories
//{
//	public interface ILocalSessionRepository
//	{
//		SessionDetailDto GetSession(string conferenceSlug, string sessionSlug);
//		void SaveSession(string conferenceSlug, FullSessionDto fullSession);
//	}

//	public class LocalSessionRepository : ILocalSessionRepository
//	{
//		private readonly IMvxFileStore _fileStore;

//		public LocalSessionRepository(IMvxFileStore fileStore)
//		{
//			_fileStore = fileStore;
//		}

//		public SessionDetailDto GetSession(string conferenceSlug, string sessionSlug)
//		{
//			var path = conferenceSlug + "-" + sessionSlug + ".json";

//			if (_fileStore.Exists(path))
//			{
//				string json;

//				if (_fileStore.TryReadTextFile(path, out json))
//				{
//					var sessionDto = JsonConvert.DeserializeObject<SessionDetailDto>(json);
//					return sessionDto;
//				}
//			}
	
//			return null;
//		}

//		public void SaveSession(string conferenceSlug, FullSessionDto fullSession)
//		{
//			var path = conferenceSlug + "-" + fullSession.slug + ".json";

//			if (_fileStore.Exists(path))
//			{
//				_fileStore.DeleteFile(path);
//			}
//			if (!_fileStore.Exists(path))
//			{
//				var sessionDetail = new SessionDetailDto(fullSession);
//				string json = JsonConvert.SerializeObject(sessionDetail);
//				_fileStore.WriteFile(path, json);
//			}
//		}

//	}
//}