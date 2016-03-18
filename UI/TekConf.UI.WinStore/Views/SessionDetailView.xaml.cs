using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using TekConf.Core.ViewModels;
using TekConf.UI.WinStore.Common;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace TekConf.UI.WinStore.Views
{
	using Windows.UI.Xaml;

	using Cirrious.CrossCore;
	using Cirrious.MvvmCross.Plugins.Sqlite;

	/// <summary>
	/// A page that displays details for a single item within a group while allowing gestures to
	/// flip through other items belonging to the same group.
	/// </summary>
	public sealed partial class SessionDetailView : TekConf.UI.WinStore.Common.LayoutAwarePage
	{
		public SessionDetailView()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Populates the page with content passed during navigation.  Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="navigationParameter">The parameter value passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
		/// </param>
		/// <param name="pageState">A dictionary of state preserved by this page during an earlier
		/// session.  This will be null the first time a page is visited.</param>
		protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
		{
			// Allow saved page state to override the initial item to display
			if (pageState != null && pageState.ContainsKey("SelectedItem"))
			{
				navigationParameter = pageState["SelectedItem"];
			}

			// TODO: Assign a bindable group to this.DefaultViewModel["Group"]
			// TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]
			// TODO: Assign the selected item to this.flipView.SelectedItem
		}

		/// <summary>
		/// Preserves state associated with this page in case the application is suspended or the
		/// page is discarded from the navigation cache.  Values must conform to the serialization
		/// requirements of <see cref="SuspensionManager.SessionState"/>.
		/// </summary>
		/// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
		protected override void SaveState(Dictionary<String, Object> pageState)
		{

		}

		private void AddFavorite_OnClick(object sender, RoutedEventArgs e)
		{
			var authentication = new Authentication(Mvx.Resolve<ISQLiteConnection>());
			if (authentication.IsAuthenticated)
			{
				var vm = this.DataContext as SessionDetailViewModel;
				if (vm != null)
					vm.AddFavoriteCommand.Execute(null);
			}
			else
			{
				var messageDialog = new Windows.UI.Popups.MessageDialog("You must be logged in to favorite a session");
				messageDialog.ShowAsync();
			}
		}

		private void Refresh_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as SessionDetailViewModel;
			if (vm != null && vm.Session != null)
			{
				var navigation = new SessionDetailViewModel.Navigation
				{
					ConferenceSlug = vm.ConferenceSlug,
					SessionSlug = vm.Session.slug
				};

				vm.Refresh(navigation);
			}
		}

		private void Settings_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as SessionDetailViewModel;
			if (vm != null)
				vm.ShowSettingsCommand.Execute(null);
		}
	}
}
