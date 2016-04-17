using Refit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tekconf.DTO;
using System;
using System.Net.Http;
using Fusillade;
using ModernHttpClient;

namespace TekConf.Mobile.Core.Services
{
	public interface ILocalConferencesService
	{
		Task Init();
		Task<List<ConferenceModel>> GetConferences();
		Task<int> Save(List<ConferenceModel> models);
	}
	
}