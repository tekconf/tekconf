using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferencesListView : MvxPhonePage
	{
		private MvxSubscriptionToken _token;

		public ConferencesListView()
		{
			InitializeComponent();
			var messenger = Mvx.Resolve<IMvxMessenger>();
			_token = messenger.Subscribe<ExceptionMessage>(message => Dispatcher.BeginInvoke(() => MessageBox.Show(message.ExceptionObject == null ? "An exception occurred but was not caught" : message.ExceptionObject.Message)));
		}

		private void Conference_OnSelected(object sender, GestureEventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			var stackPanel = sender as StackPanel;
			if (stackPanel == null) 
				return;
			var conference = (stackPanel.DataContext) as FullConferenceDto;
			if (vm != null && conference != null) 
				vm.ShowDetailCommand.Execute(conference.slug);
		}

		private void ConferenceName_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var textBlock = (sender as TextBlock);
			if (textBlock != null) 
				textBlock.MaxWidth = ActualWidth - 20;
		}

		private void ConferenceImage_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var image = (sender as Image);
			if (image != null)
			{
				image.Width = ActualWidth - 20;
				image.Height = 180 * (image.Width / 260);
			}
		}

		private void Settings_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			if (vm != null) vm.ShowSettingsCommand.Execute(null);
		}

		private void Refresh_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			if (vm != null)
			{
				vm.Refresh();
			}
		}

		private void Search_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferencesListViewModel;
			if (vm != null)
				vm.ShowSearchCommand.Execute(null);
		}

		private void Conferences_OnDoubleTap(object sender, GestureEventArgs e)
		{
			StartPeriodicAgent();
		}


		public static void UpdatePrimaryTile(int count, string content)
		{
			FlipTileData primaryTileData = new FlipTileData();
			primaryTileData.Count = count;
			primaryTileData.BackContent = content;

			ShellTile primaryTile = ShellTile.ActiveTiles.First();
			primaryTile.Update(primaryTileData);
		}

		PeriodicTask periodicTask;
		string periodicTaskName = "BackgroundAgent";
		public bool agentsAreEnabled = true;

		private void StartPeriodicAgent()
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

			periodicTask = new PeriodicTask(periodicTaskName);

			// The description is required for periodic agents. This is the string that the user
			// will see in the background services Settings page on the device.
			periodicTask.Description = "This demonstrates a periodic task.";

			// Place the call to Add in a try block in case the user has disabled agents.
			try
			{
				ScheduledActionService.Add(periodicTask);
				//PeriodicStackPanel.DataContext = periodicTask;

				// If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if(DEBUG)
    ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(1));
#endif
			}
			catch (InvalidOperationException exception)
			{
				if (exception.Message.Contains("BNS Error: The action is disabled"))
				{
					MessageBox.Show("Background agents for this application have been disabled by the user.");
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

		private void RemoveAgent(string name)
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