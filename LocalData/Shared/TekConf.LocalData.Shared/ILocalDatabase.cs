using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TekConf.LocalData.iOS;
using System.Diagnostics;
using System.IO;
using Catnap.Database.Sqlite;
using Catnap;
using Catnap.Mapping;
using Catnap.Migration;

namespace TekConf.LocalData.Shared
{
	public interface ILocalDatabase
	{
		ISessionFactory CreateDatabase ();

		void SaveSessions (IEnumerable<SessionEntity> sessions);

		void SaveConference (ConferenceEntity conference, IEnumerable<SessionEntity> sessions, IEnumerable<SpeakerEntity> speakers);
	}


}
