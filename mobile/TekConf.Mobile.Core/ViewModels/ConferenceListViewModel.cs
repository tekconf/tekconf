using MvvmCross.Core.ViewModels;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using PropertyChanged;

namespace TekConf.Mobile.Core.ViewModels
{
	[ImplementPropertyChanged]
	public class ConferenceListViewModel : MvxViewModel
	{
		string name;

		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				SetProperty(ref name, value);
			}
		}

		public string Description { get; set; }
		public string HighlightColor { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string ImageUrl { get; set; }
		public string City { get; set; }
		public string State { get; set; }
	}
}
