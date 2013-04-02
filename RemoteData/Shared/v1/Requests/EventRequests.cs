using System.Collections.Generic;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;

namespace TekConf.RemoteData.Shared.v1.Requests
{
	[Route("/v1/events/sessionRoomChanged", "GET")]
	public class SessionRoomChanged : IReturn<List<SessionRoomChangedDto>>
	{
	}

	[Route("/v1/events/conferenceLocationChanged", "GET")]
	public class ConferenceLocationChanged : IReturn<List<ConferenceLocationChangedDto>>
	{
	}

	[Route("/v1/events/conferenceEndDateChanged", "GET")]
	public class ConferenceEndDateChanged : IReturn<List<ConferenceEndDateChangedDto>>
	{
	}


	[Route("/v1/events/conferencePublished", "GET")]
	public class ConferencePublished : IReturn<List<ConferencePublishedDto>>
	{
	}

	[Route("/v1/events/conferenceUpdated", "GET")]
	public class ConferenceUpdated : IReturn<List<ConferenceUpdatedDto>>
	{
	}

	[Route("/v1/events/conferenceCreated", "GET")]
	public class ConferenceCreated : IReturn<List<ConferenceCreatedDto>>
	{
	}

	[Route("/v1/events/conferenceStartDateChanged", "GET")]
	public class ConferenceStartDateChanged : IReturn<List<ConferenceStartDateChangedDto>>
	{
	}

	[Route("/v1/events/sessionAdded", "GET")]
	public class SessionAdded : IReturn<List<SessionAddedDto>>
	{
	}


	[Route("/v1/events/sessionRemoved", "GET")]
	public class SessionRemoved : IReturn<List<SessionRemovedDto>>
	{
	}


	[Route("/v1/events/speakerAdded", "GET")]
	public class SpeakerAdded : IReturn<List<SpeakerAddedDto>>
	{
	}

	[Route("/v1/events/speakerRemoved", "GET")]
	public class SpeakerRemoved : IReturn<List<SpeakerRemovedDto>>
	{
	}
}
