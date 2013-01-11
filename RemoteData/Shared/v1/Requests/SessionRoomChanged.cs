using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;
using TekConf.RemoteData.Dtos.v1;
using TekConf.UI.Api;

namespace TekConf.RemoteData.Shared.v1.Requests
{
	[Route("/v1/events/sessionRoomChanged", "GET")]
	public class SessionRoomChanged : IReturn<List<SessionRoomChangedMessage>>
	{
	}

	[Route("/v1/events/conferenceLocationChanged", "GET")]
	public class ConferenceLocationChanged : IReturn<List<ConferenceLocationChangedMessage>>
	{
	}

	[Route("/v1/events/conferenceEndDateChanged", "GET")]
	public class ConferenceEndDateChanged : IReturn<List<ConferenceEndDateChangedMessage>>
	{
	}


	[Route("/v1/events/conferencePublished", "GET")]
	public class ConferencePublished : IReturn<List<ConferencePublishedMessage>>
	{
	}

	[Route("/v1/events/conferenceSaved", "GET")]
	public class ConferenceSaved : IReturn<List<ConferenceSavedMessage>>
	{
	}


	[Route("/v1/events/conferenceStartDateChanged", "GET")]
	public class ConferenceStartDateChanged : IReturn<List<ConferenceStartDateChangedMessage>>
	{
	}

	[Route("/v1/events/sessionAdded", "GET")]
	public class SessionAdded : IReturn<List<SessionAddedMessage>>
	{
	}


	[Route("/v1/events/cessionRemoved", "GET")]
	public class SessionRemoved : IReturn<List<SessionRemovedMessage>>
	{
	}


	[Route("/v1/events/speakerAdded", "GET")]
	public class SpeakerAdded : IReturn<List<SpeakerAddedMessage>>
	{
	}

	[Route("/v1/events/speakerRemoved", "GET")]
	public class SpeakerRemoved : IReturn<List<SpeakerRemovedMessage>>
	{
	}
}
