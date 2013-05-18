﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using TekConf.Core.ViewModels;
using TekConf.UI.WinPhone.Bootstrap;

namespace TekConf.UI.WinPhone.Views
{
	public partial class ConferenceDetailView : MvxPhonePage
	{
		public ConferenceDetailView()
		{
			InitializeComponent();
		}

		private void SessionTitle_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var title = (sender as TextBlock);
			title.MaxWidth = this.ActualWidth;
		}

		private void ConferenceImage_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			var image = (sender as Image);
			image.Width = ActualWidth - 20;
			image.Height = 180 * (image.Width / 260);
		}

		private void BrowseSessions_OnClick(object sender, EventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;

			vm.ShowSessionsCommand.Execute(vm.Conference.slug);
		}

		private void AddFavorite_OnClick(object sender, EventArgs e)
		{
			var authentication = new Authentication();

			if (authentication.IsAuthenticated)
			{
				var vm = DataContext as ConferenceDetailViewModel;

				if (vm != null)
					vm.AddFavoriteCommand.Execute(vm.Conference.slug);
			}
			else
			{
				MessageBox.Show("You must be logged in to favorite a conference");
			}
		}

		private void ShowWebBrowser(string uri)
		{
			if (!string.IsNullOrWhiteSpace(uri))
			{
				var webBrowserTask = new WebBrowserTask {Uri = new Uri(uri)};
				webBrowserTask.Show();
			}
		}

		private void FacebookNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null) 
				ShowWebBrowser(vm.Conference.facebookUrl);
		}

		private void HomepageNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null)
				ShowWebBrowser(vm.Conference.homepageUrl);
		}

		private void LanyrdNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null)
				ShowWebBrowser(vm.Conference.lanyrdUrl);
		}

		private void MeetupNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null)
				ShowWebBrowser(vm.Conference.meetupUrl);
		}

		private void GooglePlusNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null)
				ShowWebBrowser(vm.Conference.googlePlusUrl);
		}

		private void VimeoNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null)
				ShowWebBrowser(vm.Conference.vimeoUrl);
		}

		private void YouTubeNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null)
				ShowWebBrowser(vm.Conference.youtubeUrl);
		}

		private void GitHubNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null)
				ShowWebBrowser(vm.Conference.githubUrl);
		}

		private void LinkedInNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			var vm = DataContext as ConferenceDetailViewModel;
			if (vm != null)
				ShowWebBrowser(vm.Conference.linkedInUrl);
		}

		private void TwitterHashTagNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void TwitterNameNavigate_OnClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Settings_OnClick(object sender, EventArgs e)
		{
			var vm = this.DataContext as ConferenceDetailViewModel;
			if (vm != null) vm.ShowSettingsCommand.Execute(null);
		}

		private void Refresh_OnClick(object sender, EventArgs e)
		{
			var vm = this.DataContext as ConferenceDetailViewModel;
			if (vm != null && vm.Conference != null)
			{
				vm.Refresh(vm.Conference.slug);
			}
		}
	}
}