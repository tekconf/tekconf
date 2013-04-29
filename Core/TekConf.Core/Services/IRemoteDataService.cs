using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Services
{
	public interface IRemoteDataService
	{
		void GetConferences(
					string userName = null,
					string sortBy = "end",
					bool? showPastConferences = false,
					bool? showOnlyOpenCalls = false,
					bool? showOnlyOnSale = false,
					string search = null,
					string city = null,
					string state = null,
					string country = null,
					double? latitude = null,
					double? longitude = null,
					double? distance = null,
					Action<IEnumerable<FullConferenceDto>> success = null,
					Action<Exception> error = null);


		void GetConference(string slug, 
			Action<FullConferenceDto> success = null,
			Action<Exception> error = null);

		void GetSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success = null, Action<Exception> error = null);
		void GetSchedules(string userName, Action<IEnumerable<FullConferenceDto>> success = null, Action<Exception> error = null);

		void AddToSchedule(string userName, string conferenceSlug, Action<ScheduleDto> success = null, Action<Exception> error = null);
		void GetSession(string conferenceSlug, string sessionSlug, Action<FullSessionDto> success, Action<Exception> error);
	}
}
