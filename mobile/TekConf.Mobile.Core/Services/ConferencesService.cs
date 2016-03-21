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
						Name = "Test Conference with somet asd asklnasdl kllsad lkjhf",
						Description = "Test description akshf jlka skljsa lkh aslhjk hkhas jh skjhsd akhsdf khs hlk hkj hlkhs  lkjsdf lk sjdkl l kljs dlj fsd l kdsjfs dlj lkjds fkjdfs ls lkjsd klj sdlkjdsljk ljksdflkj ljk slkjs dlkjds ljl lkj slkj slkjd lkjs djls l lkj jll lkllsdfjjkjkjdff  jlkjsdflkj lk klj kljkl jkljdfkljskdl jlkj dsl",
						City = "Farmington",
						EndDate = DateTime.Now.AddDays(7),
						HighlightColor = "24aad6",
						StartDate = DateTime.Now,
						State = "MI",
						ImageUrl = "http://tekauth.azurewebsites.net/images/conferences/codemash-2016.png"
					}
				};
			});
		}
	}
}

