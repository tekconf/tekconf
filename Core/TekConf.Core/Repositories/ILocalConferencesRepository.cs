using System.Collections;
using System.Collections.Generic;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public interface ILocalConferencesRepository
	{
		void SaveConferences(IEnumerable<FullConferenceDto> conferences);
		IEnumerable<ConferencesListViewDto> GetConferencesListView();
		ConferenceDetailViewDto GetConferenceDetail(string slug);
		void SaveConference(FullConferenceDto conference);
	}
}