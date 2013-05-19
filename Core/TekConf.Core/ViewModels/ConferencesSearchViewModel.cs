using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Interfaces;
using TekConf.Core.Services;

namespace TekConf.Core.ViewModels
{
	public class ConferencesSearchViewModel : MvxViewModel
	{
		private readonly IRemoteDataService _remoteDataService;
		private readonly IAnalytics _analytics;
		private readonly IAuthentication _authentication;
		private readonly IMvxMessenger _messenger;

		public ConferencesSearchViewModel(IRemoteDataService remoteDataService, IAnalytics analytics, IAuthentication authentication, IMvxMessenger messenger)
		{
			_remoteDataService = remoteDataService;
			_analytics = analytics;
			_authentication = authentication;
			_messenger = messenger;
		}

		public void Init(string fake)
		{
		}

		private string _searchText;
		public string SearchText
		{
			get
			{
				return _searchText;
			}
			set
			{
				_searchText = value;
				RaisePropertyChanged(() => SearchText);
			}
		}

		public ICommand SearchCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<ConferencesListViewModel>(new { searchTerm = this.SearchText }));
			}
		}

	}
}