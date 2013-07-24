using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;

namespace TekConf.UI.WinPhone.Classes
{
	public static class LiveTileAgent
	{
		private static void UpdatePrimaryTile(int count, string content)
		{
			var primaryTileData = new FlipTileData
			{
				Count = count,
				BackContent = content
			};

			var primaryTile = ShellTile.ActiveTiles.First();
			primaryTile.Update(primaryTileData);
		}

		private static PeriodicTask periodicTask;
		private static string periodicTaskName = "BackgroundAgent";
		public static bool agentsAreEnabled = true;

		public static void StartPeriodicAgent()
		{
			// Variable for tracking enabled status of background agents for this app.
			agentsAreEnabled = true;

			// Obtain a reference to the period task, if one exists
			periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

			// If the task already exists and background agents are enabled for the
			// application, you must remove the task and then add it again to update 
			// the schedule
			if (periodicTask != null)
			{
				RemoveAgent(periodicTaskName);
			}

			periodicTask = new PeriodicTask(periodicTaskName) { Description = "Update TekConf Schedule" };

			// The description is required for periodic agents. This is the string that the user
			// will see in the background services Settings page on the device.

			// Place the call to Add in a try block in case the user has disabled agents.
			try
			{
				ScheduledActionService.Add(periodicTask);
				//PeriodicStackPanel.DataContext = periodicTask;

				// If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if(DEBUG)
    ScheduledActionService.LaunchForTest(periodicTaskName, new TimeSpan(0, 0, 0, 20));
#endif
			}
			catch (InvalidOperationException exception)
			{
				if (exception.Message.Contains("BNS Error: The action is disabled"))
				{
					//MessageBox.Show("Background agents for this application have been disabled by the user.");
					agentsAreEnabled = false;
					//PeriodicCheckBox.IsChecked = false;
				}

				if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
				{
					// No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.

				}
				//PeriodicCheckBox.IsChecked = false;
			}
			catch (SchedulerServiceException)
			{
				// No user action required.
				//PeriodicCheckBox.IsChecked = false;
			}
		}

		private static void RemoveAgent(string name)
		{
			try
			{
				ScheduledActionService.Remove(name);
			}
			catch (Exception)
			{
			}
		}
	}
}
