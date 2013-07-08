using System.Collections;
using System.Collections.Generic;
using TekConf.Core.Entities;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Repositories
{
	public interface ILocalConferencesRepository
	{
		void Save(IEnumerable<ConferenceEntity> conferences);
		IEnumerable<ConferenceEntity> List();
		void Save(ConferenceEntity conference);
		ConferenceEntity Get(string conferenceSlug);
		void AddSession(SessionEntity session);
		SessionEntity Get(string conferenceSlug, string sessionSlug);
	}
}