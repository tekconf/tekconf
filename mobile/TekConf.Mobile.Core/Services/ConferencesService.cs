using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TekConf.Mobile.Core
{
	public interface IConferencesService
	{
		Task<IList<ConferenceModel>> Load();
	}
	//public class ConferencesService : IConferencesService
	//{
	//	public ConferencesService()
	//	{
	//	}
	//}
}

