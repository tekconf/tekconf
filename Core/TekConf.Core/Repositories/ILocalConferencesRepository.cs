using System.Collections;
using System.Collections.Generic;
using TekConf.Core.Entities;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	using System.Threading.Tasks;

	public interface ILocalConferencesRepository
	{
		ConferenceEntity Get(string conferenceSlug);
		SessionEntity Get(string conferenceSlug, string sessionSlug);

		Task<IList<ConferenceEntity>> ListFavoritesAsync();

		Task<IList<SessionEntity>> ListFavoriteSessionsAsync(string conferenceSlug);
		Task<IList<ConferenceEntity>> ListAsync();

		int Save(ConferenceEntity conference);
		int Save(string conferenceSlug, SessionEntity session);
		void Save(IList<ConferenceEntity> conferences);
		void AddSession(SessionEntity session);

		int Delete(ConferenceEntity conference);
	}
}