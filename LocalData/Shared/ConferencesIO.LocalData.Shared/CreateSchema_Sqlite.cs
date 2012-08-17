using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConferencesIO.LocalData.iOS;
using System.Diagnostics;
using System.IO;
using Catnap.Database.Sqlite;
using Catnap;
using Catnap.Mapping;
using Catnap.Migration;

namespace ConferencesIO.LocalData.Shared
{

	public class CreateSchema_Sqlite : BaseMigration
	{
		public CreateSchema_Sqlite() : base(
					SessionSpeakerEntity.CreateTableSql,
					ConferenceEntity.CreateTableSql,
					SessionEntity.CreateTableSql,
					SpeakerEntity.CreateTableSql
			) { }
	}
	
}
