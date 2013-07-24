using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Cirrious.CrossCore;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;

using TekConf.Core.Repositories;

namespace TekConf.UI.WinPhone.BackgroundAgent
{
	using Cirrious.MvvmCross.Plugins.Sqlite;
	using Cirrious.MvvmCross.Plugins.Sqlite.WindowsPhone;

	public class ScheduledAgent : ScheduledTaskAgent
	{
		/// <remarks>
		/// ScheduledAgent constructor, initializes the UnhandledException handler
		/// </remarks>
		static ScheduledAgent()
		{
			// Subscribe to the managed exception handler
			Deployment.Current.Dispatcher.BeginInvoke(delegate
			{
				Application.Current.UnhandledException += UnhandledException;
			});
		}

		/// Code to execute on Unhandled Exceptions
		private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			if (Debugger.IsAttached)
			{
				// An unhandled exception has occurred; break into the debugger
				Debugger.Break();
			}
		}

		/// <summary>
		/// Agent that runs a scheduled task
		/// </summary>
		/// <param name="task">
		/// The invoked task
		/// </param>
		/// <remarks>
		/// This method is called when a periodic or resource intensive task is invoked
		/// </remarks>
		protected override async void OnInvoke(ScheduledTask task)
		{
			//try
			//{
			//	if (ShellTile.ActiveTiles != null)
			//	{
			//		var factory = new MvxWindowsPhoneSQLiteConnectionFactory();
			//		using (var connection = factory.Create("tekconf.db"))
			//		{
			//			var localConferencesRepository = new LocalConferencesRepository(connection);
			//			var favorites = await localConferencesRepository.ListFavoritesAsync();
			//			var nextFavoriteConference = favorites.Where(x => x.End >= DateTime.Now).OrderBy(x => x.End).FirstOrDefault();
			//			if (nextFavoriteConference != null)
			//			{
			//				var appTile = ShellTile.ActiveTiles.First();

			//				var sessions = nextFavoriteConference.Sessions(connection).ToList();

			//				if (sessions.Any())
			//				{
			//					var nextSession = sessions.Where(x => x.End >= DateTime.Now).OrderBy(x => x.End).FirstOrDefault();

			//					if (nextSession != null)
			//					{
			//						var tileData = new FlipTileData()
			//													{
			//														BackContent = nextSession.Title,
			//														BackTitle = nextSession.StartDescription() + (string.IsNullOrWhiteSpace(nextSession.Room) ? "" : Environment.NewLine) + nextSession.Room,
			//														Title = "",
			//														WideBackContent = nextSession.Title + (string.IsNullOrWhiteSpace(nextFavoriteConference.Name) ? "" : " - " + nextFavoriteConference.Name)
			//													};

			//						appTile.Update(tileData);
			//					}
			//					else
			//					{
			//						var tileData = new FlipTileData()
			//													{
			//														BackContent = nextFavoriteConference.Name,
			//														BackTitle = nextFavoriteConference.DateRange(),
			//														Title = "",
			//														WideBackContent = nextFavoriteConference.Name + " " + nextFavoriteConference.DateRange() + " " + nextFavoriteConference.FormattedCity()
			//													};

			//						appTile.Update(tileData);
			//					}
			//				}
			//				else
			//				{
			//					var tileData = new FlipTileData()
			//												{
			//													BackContent = nextFavoriteConference.Name,
			//													BackTitle = nextFavoriteConference.DateRange(),
			//													Title = "",
			//													WideBackContent = nextFavoriteConference.Name + " " + nextFavoriteConference.DateRange() + " " + nextFavoriteConference.FormattedCity()
			//												};

			//					appTile.Update(tileData);

			//				}
			//			}
			//		}

			//		NotifyComplete();
			//	}
			//}
			//catch (Exception ex)
			//{
			//}

		}
	}
}