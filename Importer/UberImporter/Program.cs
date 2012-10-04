using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using TekConf.UI.Api;
using UberImporter.MonkeySpace;
using UberImporter.RailsConf;

namespace UberImporter
{
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

            

            var monkeySpace = new MonkeySpaceImporter();
            var railsConf = new RailsConfImporter();
            var codestock = new CodeStockImporter();
            var oneDevDayDetroit = new OneDevDayDetroitImporter();

            oneDevDayDetroit.Import();
            monkeySpace.Import();
            railsConf.Import();
            codestock.Import();
        }
    }
}
