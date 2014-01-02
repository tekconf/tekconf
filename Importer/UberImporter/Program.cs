using UberImporter.Importers.CodeMash2014EventBoard;
using UberImporter.Importers.MonkeySpace2012;

namespace UberImporter
{
	using UberImporter.Importers.CodeMash2014;

	class Program
	{
		static void Main(string[] args)
		{
			//MongoDbConnection connection = new MongoDbConnection();
			//var collection = connection.RemoteDatabase.GetCollection<ConferenceEntity>("conferences");

			//var slugs = new List<string>()
			//                {
			//                    "MonkeySpace-2012",
			//                    "RailsConf-2012",
			//                    "CodeStock-2012",
			//                    "1DevDayDetroit-2012"
			//                };
			//foreach (var slug in slugs)
			//{
			//    IMongoQuery query1 = Query.EQ("slug", slug);
			//    collection.Remove(query1);

			//    IMongoQuery query2 = Query.EQ("slug", slug.ToLower());
			//    collection.Remove(query2);
			//}

			var codeMash2014 = new CodeMash2014EventBoardImporter();
			//var monkeySpace = new MonkeySpaceImporter();

			//var railsConf = new RailsConfImporter();
			//var codestock = new CodeStockImporter();
			//var oneDevDayDetroit = new OneDevDayDetroitImporter();
			//var codeMash2013 = new CodeMash2013Importer();

			//oneDevDayDetroit.Import();
			codeMash2014.Import();
			//railsConf.Import();
			//codestock.Import();
			//codeMash2013.Import();
		}
	}
}
