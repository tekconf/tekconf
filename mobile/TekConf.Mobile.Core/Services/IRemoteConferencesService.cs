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
	public interface IRemoteConferencesService
	{
		Task<List<Conference>> GetConferences(Priority priority);

		//Task<List<Conference>> GetConferences(string token, Priority priority);
		Task<Conference> GetConference(Priority priority, string slug);
	}
}