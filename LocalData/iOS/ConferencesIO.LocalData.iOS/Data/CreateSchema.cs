using System;
using Catnap.Database;
using Catnap;
using Catnap.Migration;
using System.Collections.Generic;
using System.Linq;

namespace ConferencesIO.LocalData.iOS
{
	public class CreateSchema : BaseMigration
	{
		public CreateSchema() : base(
			//SessionSpeakerEntity.CreateTableSql,
			ConferenceEntity.CreateTableSql
			//SessionEntity.CreateTableSql,
			//SpeakerEntity.CreateTableSql
			) { }

//		private readonly List<string> sqls = new List<string>
//		{
//			SessionEntity.CreateTableSql,
//			SpeakerEntity.CreateTableSql,
//			RefreshEntity.CreateTableSql,
//			ScheduledSessionEntity.CreateTableSql,
//			RemoteQueueEntity.CreateTableSql,
//		};
//		
//		public string Name {
//			get { return "create_schema"; }
//		}
//		
//		public Action Action {
//			get { return () => sqls.Select (x => new DbCommandSpec ().SetCommandText (x)).ToList ().ForEach (Execute); }
//		}
//		
//		private void Execute (DbCommandSpec command)
//		{
//			UnitOfWork.Current.Session.ExecuteNonQuery (command);
//		}
	}
}

