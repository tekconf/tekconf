using MvvmCross.Core.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoMapper;
using System.Threading.Tasks;
using PropertyChanged;

namespace TekConf.Mobile.Core.ViewModels
{
	[ImplementPropertyChanged]
	public class ConferencesViewModel : MvxViewModel
	{
		readonly IConferencesService _conferencesService;
		readonly IMapper _mapper;
		ObservableCollection<ConferenceListViewModel> conferences;

		//public ObservableCollection<ConferenceListViewModel> Conferences { get; set; } = new ObservableCollection<ConferenceListViewModel>();
		public ObservableCollection<ConferenceListViewModel> Conferences
		{
			get
			{
				return conferences;
			}

			set
			{
				SetProperty(ref conferences, value);
			}
		}

		public bool IsLoading { get; set; }

		public ConferencesViewModel(IConferencesService conferencesService, IMapper mapper)
		{
			_mapper = mapper;
			_conferencesService = conferencesService;
			Conferences = new ObservableCollection<ConferenceListViewModel>();
		}

		private ICommand _loadCommand;
		public ICommand LoadCommand
		{
			get
			{
				_loadCommand = _loadCommand ?? new MvxAsyncCommand(Load, CanLoad);
				return _loadCommand;
			}
		}

		private ICommand _showSettingsCommand;
		public ICommand ShowSettingsCommand
		{
			get
			{
				_showSettingsCommand = _showSettingsCommand ?? new MvxCommand(() => ShowViewModel<SettingsViewModel>());
				return _showSettingsCommand;
			}
		}

		private ICommand _showFilterCommand;
		public ICommand ShowFilterCommand
		{
			get
			{
				_showFilterCommand = _showFilterCommand ?? new MvxCommand(() => ShowViewModel<FilterViewModel>());
				return _showFilterCommand;
			}
		}

		private ICommand _closeCommand;
		public ICommand CloseCommand
		{
			get
			{
				_closeCommand = _closeCommand ?? new MvxCommand(() => Close(this));
				return _closeCommand;
			}
		}

		private async Task Load()
		{
			IsLoading = true;
			var conferenceModels = await _conferencesService.Load();
			var conferenceViewModels = _mapper.Map<IList<ConferenceListViewModel>>(conferenceModels);
			this.Conferences = new ObservableCollection<ConferenceListViewModel>(conferenceViewModels);
			IsLoading = false;
		}

		bool CanLoad()
		{
			return true;
		}

	}
}
