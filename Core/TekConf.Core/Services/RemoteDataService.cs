using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Cirrious.CrossCore.Core;
using TekConf.Core.Models;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Services
{

	public class RemoteDataService : IRemoteDataService
	{
		public void GetConferences(
			string userName = null,
			string sortBy = "end",
			bool? showPastConferences = null,
			bool? showOnlyOpenCalls = null,
			bool? showOnlyOnSale = null,
			string search = null,
			string city = null,
			string state = null,
			string country = null,
			double? latitude = null,
			double? longitude = null,
			double? distance = null,
			Action<IEnumerable<FullConferenceDto>> success = null,
			Action<Exception> error = null)
		{
			ConferencesService.GetConferencesAsync(success, error);
			//return new List<FullConferenceDto>()
			//	{
			//		new FullConferenceDto()
			//		{
			//			name = "Build 2013", 
			//			imageUrl = "http://www.tekconf.com/img/conferences/DefaultConference.png", 
			//			tagline = ""
			//		}
			//	};
		}
	}
}