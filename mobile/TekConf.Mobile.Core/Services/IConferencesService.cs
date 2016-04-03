using System.Threading.Tasks;
using System.Collections.Generic;
using Tekconf.DTO;
using Fusillade;

namespace TekConf.Mobile.Core.Services
{
	public interface IConferencesService
	{
		Task<List<ConferenceModel>> GetConferences();

		//Task<List<Conference>> GetConferences(string token, Priority priority);
		Task<Conference> GetConference(Priority priority, string slug);
	}
}