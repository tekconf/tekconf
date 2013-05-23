using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace TekConf.Core.ViewModels
{
	public class ConferencesSearchViewModel : MvxViewModel
	{
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
				return new MvxCommand(() => ShowViewModel<ConferencesListViewModel>(new { searchTerm = SearchText }));
			}
		}

	}
}