using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using TekConf.Core.ViewModels;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.UI.WinStore.Views
{
	using Cirrious.CrossCore;

	using TekConf.Core.Services;

	/// <summary>
	/// A page that displays an overview of a single group, including a preview of the items
	/// within the group.
	/// </summary>
	public sealed partial class ConferenceDetailView : TekConf.UI.WinStore.Common.LayoutAwarePage
	{
		public ConferenceDetailView()
		{
			this.InitializeComponent();
			sessionsGridView.SelectionMode = ListViewSelectionMode.Multiple;
			sessionsListView.SelectionMode = ListViewSelectionMode.Multiple;
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
			// TODO: Assign a bindable group to this.DefaultViewModel["Group"]
			// TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]
		}

		private void ConferenceImage_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{

		}

		private void Session_OnTap(object sender, TappedRoutedEventArgs e)
		{
			var gridView = (sender as GridView);
			if (gridView != null)
			{
				var session = gridView.SelectedItem as FullSessionDto;
				if (session != null)
				{
					var vm = this.DataContext as ConferenceDetailViewModel;
					if (vm != null)
					{
						vm.ShowSessionDetailCommand.Execute(new SessionDetailViewModel.Navigation() { ConferenceSlug = vm.Conference.slug, SessionSlug = session.slug });
					}
				}
			}
		}

		private void AddFavorite_OnClick(object sender, RoutedEventArgs e)
		{
			var authentication = Mvx.Resolve<IAuthentication>();

			if (authentication.IsAuthenticated)
			{
				var vm = DataContext as ConferenceDetailViewModel;

				if (vm != null)
					vm.AddFavoriteCommand.Execute(vm.Conference.slug);
			}
			else
			{
				var messageDialog = new Windows.UI.Popups.MessageDialog("You must be logged in to favorite a conference");
				messageDialog.ShowAsync();
			}
		}

		private void Settings_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null) 
				vm.ShowSettingsCommand.Execute(null);
		}

		private void Refresh_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null && vm.Conference != null)
				vm.Refresh(vm.Conference.slug);
		}

		//private void RefreshFavoriteIcon()
		//{
		//	var vm = DataContext as ConferenceDetailViewModel;
		//	if (vm != null)
		//	{
		//		string imageUrl = "/img/appbar.heart.png";
		//		if (vm.Conference != null)
		//		{
		//			if (vm.Conference.isAddedToSchedule == true)
		//			{
		//				imageUrl = "/img/appbar.heart.cross.png";
		//			}
		//		}

		//		((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IconUri = new Uri(imageUrl, UriKind.Relative);
		//	}
		//}
	}



}
