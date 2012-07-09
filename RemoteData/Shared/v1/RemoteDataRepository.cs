using System;
using System.Collections.Generic;
using System.Net;
using ConferencesIO.RemoteData.Dtos;
using ConferencesIO.RemoteData.Dtos.v1;
using ServiceStack.Text;

namespace ConferencesIO.RemoteData.v1
{
  public class RemoteDataRepository
  {
    private string _baseUrl;
    public RemoteDataRepository(string baseUrl)
    {
      _baseUrl = baseUrl;
    }

    public void GetConferences(Action<IList<ConferencesDto>> callback)
    {
      string url = _baseUrl + "conferences";

      var client = new WebClient();
      client.Encoding = System.Text.Encoding.UTF8;
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
                                            {
                                              var conferences = JsonSerializer.DeserializeFromString<List<ConferencesDto>>(args.Result);
                                              callback(conferences);
                                            };

      client.DownloadStringAsync(new Uri(url));

    }

    public void GetConference(string slug, Action<ConferenceDto> callback)
    {
      string url = _baseUrl + "conferences/" + slug;
      var client = new WebClient();
      client.Encoding = System.Text.Encoding.UTF8;
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      client.DownloadStringCompleted += (sender, args) =>
      {
        var conference = JsonSerializer.DeserializeFromString<ConferenceDto>(args.Result);
        callback(conference);
      };

      client.DownloadStringAsync(new Uri(url));
    }

    //public void AddConference(Conference conference, Action<bool> callback)
    //{
    //  string url = _baseUrl + "conferences";
    //  var conferenceJson = JsonSerializer.SerializeToString(conference);

    //  var client = new WebClient();
    //  client.Encoding = System.Text.Encoding.UTF8;
    //  client.Headers[HttpRequestHeader.Accept] = "application/json";

    //  client.UploadStringCompleted += (sender, args) =>
    //                                    {
    //                                      var x = args.Result;
    //                                      callback(true);
    //                                    };
    //  client.UploadStringAsync(new Uri(url), "POST", conferenceJson);

    //}

    public void GetSpeakers(string conferenceSlug, string sessionSlug, Action<IList<SpeakersDto>> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/speakers";

      var client = new WebClient();
      client.Encoding = System.Text.Encoding.UTF8;
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
      {
        var speakers = JsonSerializer.DeserializeFromString<List<SpeakersDto>>(args.Result);
        callback(speakers);
      };

      client.DownloadStringAsync(new Uri(url));
    }

    public void GetSpeakers(string conferenceSlug, Action<IList<SpeakersDto>> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/speakers";

      var client = new WebClient();
      client.Encoding = System.Text.Encoding.UTF8;
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
      {
        var speakers = JsonSerializer.DeserializeFromString<List<SpeakersDto>>(args.Result);
        callback(speakers);
      };

      client.DownloadStringAsync(new Uri(url));

    }

    public void GetSpeaker(string conferenceSlug, string slug, Action<SpeakerDto> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/speakers/" + slug;

      var client = new WebClient();
      client.Encoding = System.Text.Encoding.UTF8;
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
      {
        var speaker = JsonSerializer.DeserializeFromString<SpeakerDto>(args.Result);
        callback(speaker);
      };

      client.DownloadStringAsync(new Uri(url));
    }

    public void GetSessions(string conferenceSlug, Action<IList<SessionsDto>> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/sessions";

      var client = new WebClient();
      client.Encoding = System.Text.Encoding.UTF8;
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
        {
          //var sessions = new List<SessionsDto>()
          //{
          //new SessionsDto() {title = "My Session", start = DateTime.Now},
          //new SessionsDto() {title = "AJ's Session", start = DateTime.Now}
          //};
          var sessions = JsonSerializer.DeserializeFromString<List<SessionsDto>>(args.Result);
          callback(sessions);
        };

      client.DownloadStringAsync(new Uri(url));

    }


    public void GetSession(string conferenceSlug, string slug, Action<SessionDto> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/sessions/" + slug;

      var client = new WebClient {Encoding = System.Text.Encoding.UTF8};
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
      {
        var session = JsonSerializer.DeserializeFromString<SessionDto>(args.Result);
        callback(session);
      };

      client.DownloadStringAsync(new Uri(url));
    }

    //public void AddSession(Session session, Action<bool> callback)
    //{
    //  string url = _baseUrl + "conferences/" + session.conferenceSlug + "/sessions";

    //  var client = new WebClient();
    //  client.Encoding = System.Text.Encoding.UTF8;
    //  client.Headers[HttpRequestHeader.Accept] = "application/json";

    //  var sessionJson = JsonSerializer.SerializeToString(session);
    //  client.UploadStringCompleted += (sender, args) =>
    //  {
    //    var x = args.Result;
    //    callback(true);
    //  };
    //  client.UploadStringAsync(new Uri(url), "POST", sessionJson);
    //}

    //public void AddSpeaker(Speaker speaker, Action<bool> callback)
    //{
    //  string url = _baseUrl + "conferences/" + speaker.conferenceSlug + "/sessions/" + speaker.SessionSlug + "/speakers";

    //  var client = new WebClient();
    //  client.Encoding = System.Text.Encoding.UTF8;";
    //  client.Headers[HttpRequestHeader.Accept] = "application/json";

    //  var sessionJson = JsonSerializer.SerializeToString(speaker);
    //  client.UploadStringCompleted += (sender, args) =>
    //  {
    //    var x = args.Result;
    //    callback(true);
    //  };
    //  client.UploadStringAsync(new Uri(url), "POST", sessionJson);
    //}
  }
}
