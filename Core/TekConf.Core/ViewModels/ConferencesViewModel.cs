using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;
using TekConf.Core.Services;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.ViewModels
{
    public class ConferencesViewModel : MvxViewModel
    {
	    private readonly IRemoteDataService _remoteDataService;

	    public ConferencesViewModel()
	    {
		    //_remoteDataService = remoteDataService;
		    LoadConferences();
	    }

			private List<FullConferenceDto> _conferences;
			public List<FullConferenceDto> Conferences
			{
				get
				{
					return _conferences;
				}
				set
				{
					_conferences = value;
					RaisePropertyChanged(() => Conferences);
				}
			}

			private void LoadConferences()
			{
				//Conferences = _remoteDataService.GetConferences();
				Conferences = new List<FullConferenceDto>()
				{
					new FullConferenceDto()
					{
						name = "Test", 
						imageUrl = "http://www.tekconf.com/img/conferences/DefaultConference.png", 
						tagline = "tag"
					}
				};
			}
    }
}
