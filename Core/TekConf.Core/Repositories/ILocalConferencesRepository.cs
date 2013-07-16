using System.Collections;
using System.Collections.Generic;
using TekConf.Core.Entities;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	using System.Threading.Tasks;

	public interface ILocalConferencesRepository
	{
		void Save(IList<ConferenceEntity> conferences);
		Task<IList<ConferenceEntity>> ListFavoritesAsync();
		Task<IList<ConferenceEntity>> ListAsync();

		void Save(ConferenceEntity conference);
		ConferenceEntity Get(string conferenceSlug);
		void AddSession(SessionEntity session);
		SessionEntity Get(string conferenceSlug, string sessionSlug);
	}
}