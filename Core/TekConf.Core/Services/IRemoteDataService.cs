using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.Core.Services
{
	public interface IRemoteDataService
	{
		List<FullConferenceDto> GetConferences(
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
					double? distance = null);
	}

	public class RemoteDataServce : IRemoteDataService
	{
		public List<FullConferenceDto> GetConferences(
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
			double? distance = null)
		{
			return new List<FullConferenceDto>() { new FullConferenceDto() { name = "Test " } };
		}
	}
}
