using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConferencesIO.LocalData.iOS;
using SQLite;
using System.Diagnostics;
using System.IO;

namespace ConferencesIO.LocalData.Shared
{
  public interface ILocalDatabase
  {
    bool CreateDatabase();
	void SaveSessions (IEnumerable<SessionEntity> sessions);
  }

}
