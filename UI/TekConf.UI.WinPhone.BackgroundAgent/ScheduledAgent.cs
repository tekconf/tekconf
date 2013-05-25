using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.File.WindowsPhone;
using Microsoft.Phone.Scheduler;
using TekConf.Core.Repositories;

namespace TekConf.UI.WinPhone.BackgroundAgent
{
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
		protected override void OnInvoke(ScheduledTask task)
		{
			IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
			IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("schedules.json", FileMode.Open, FileAccess.Read);
			using (var reader = new StreamReader(fileStream))
			{    //Visualize the text data in a TextBlock text
				var json = reader.ReadToEnd();
			}

			//TODO: Add code to perform your task in background
			MvxIsolatedStorageFileStore fileStore = new MvxIsolatedStorageFileStore();
			//string json = "";
			//fileStore.TryReadTextFile("schedules.json", out json);
			var scheduleRepository = new LocalScheduleRepository(fileStore);
			var xx = scheduleRepository.NextScheduledSession;
			NotifyComplete();
		}
	}
}