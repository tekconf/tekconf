//using System;
//using SQLite;
//
//namespace ConferencesIO.LocalData.iOS
//{
//	public class Database : SQLiteConnection
//	{
//		public Database (string path) : base(path)
//		{
//			CreateTable<SessionEntity> ();
//		}
//
////		public IEnumerable<Valuation> QueryValuations (Stock stock)
////		{
////			return Table<Valuation> ().Where(x => x.StockId == stock.Id);
////		}
////
////		public Valuation QueryLatestValuation (Stock stock)
////		{
////			return Table<Valuation> ().Where(x => x.StockId == stock.Id).OrderByDescending(x => x.Time).Take(1).FirstOrDefault();
////		}
////
////		public Stock QueryStock (string stockSymbol)
////		{
////			return	(from s in Table<Stock> ()
////			        where s.Symbol == stockSymbol
////			        select s).FirstOrDefault ();
////		}
////
////		public IEnumerable<Stock> QueryAllStocks ()
////		{
////			return	from s in Table<Stock> ()
////				orderby s.Symbol
////					select s;
////		}
////		
////		public void UpdateStock (string stockSymbol)
////		{
////			//
////			// Ensure that there is a valid Stock in the DB
////			//
////			var stock = QueryStock (stockSymbol);
////			if (stock == null) {
////				stock = new Stock { Symbol = stockSymbol };
////				Insert (stock);
////			}
////			
////			//
////			// When was it last valued?
////			//
////			var latest = QueryLatestValuation (stock);
////			var latestDate = latest != null ? latest.Time : new DateTime (1950, 1, 1);
////			
////			//
////			// Get the latest valuations
////			//
////			try {
////				var newVals = new YahooScraper ().GetValuations (stock, latestDate + TimeSpan.FromHours (23), DateTime.Now);
////				InsertAll (newVals);
////			} catch (System.Net.WebException ex) {
////				Console.WriteLine (ex);
////			}
////		}
//	}
//}
//
