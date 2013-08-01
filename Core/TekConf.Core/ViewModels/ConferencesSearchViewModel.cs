using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace TekConf.Core.ViewModels
{
	public class ConferencesSearchViewModel : MvxViewModel
	{
		public void Init(string fake)
		{
		}

		public string SearchText { get; set; }

		public ICommand SearchCommand
		{
			get
			{
				return new MvxCommand(() => 
					ShowViewModel<ConferencesListViewModel>(new { searchTerm = SearchText })
					);
			}
		}

	}
}