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
						Name = "Test Conference with somet asd asklnasdl kllsad lkjhf"
					}
				};
			});
		}
	}
}

