using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TekConf.Mobile.Core
{
	public interface IConferencesService
	{
		Task<IList<ConferenceModel>> Load();
	}

	public class ConferencesService : IConferencesService
	{
		public async Task<IList<ConferenceModel>> Load()
		{
			return await Task.Run(() => {
				return new List<ConferenceModel> { 
					new ConferenceModel {
						Name = "Xamarin Evolve",
						Description = "Xamarin Evolve is the largest cross-platform mobile event in the world, where over 1,500 developers, industry leaders, and Xamarin experts converge to advance the state of the art, discuss mobile strategy, and define the future of apps.",
						City = "Orlando",
						EndDate = new DateTime(2016, 04, 29),
						HighlightColor = "ad51ab",
						StartDate = new DateTime(2016, 04, 24),
						State = "FL",
						ImageUrl = "http://tekauth.azurewebsites.net/images/conferences/evolve-2016.png"
					}
				};
			});
		}
	}
}

