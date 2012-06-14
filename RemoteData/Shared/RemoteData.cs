using System;
using System.Collections.Generic;
using System.Net;
using ServiceStack.Text;

namespace RemoteData.Shared
{
  public class RemoteData
  {
    private string _baseUrl;
    public RemoteData(string baseUrl)
    {
      _baseUrl = baseUrl;
    }
    //private const string _baseUrl = "http://localhost:10248/api/";
    ////private const string _baseUrl = "http://conference.azurewebsites.net/api/";

    public void GetConferences(Action<IList<Conference>> callback)
    {
      string url = _baseUrl + "conferences";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
                                            {
                                              var conferences = new List<Conference>() { new Conference() { Name = "ThatConference-2012", Start = DateTime.Now } };
                                             // var conferences = JsonSerializer.DeserializeFromString<List<Conference>>(args.Result);
                                              callback(conferences);
                                            };

      client.DownloadStringAsync(new Uri(url));

    }

    public void GetConference(string slug, Action<Conference> callback)
    {
      string url = _baseUrl + "conferences/" + slug;
      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";
      client.DownloadStringCompleted += (sender, args) =>
      {
        var conference = JsonSerializer.DeserializeFromString<Conference>(args.Result);
        callback(conference);
      };

      client.DownloadStringAsync(new Uri(url));
    }

    public void AddConference(Conference conference, Action<bool> callback)
    {
      string url = _baseUrl + "conferences";
      var conferenceJson = JsonSerializer.SerializeToString(conference);

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.UploadStringCompleted += (sender, args) =>
                                        {
                                          var x = args.Result;
                                          callback(true);
                                        };
      client.UploadStringAsync(new Uri(url), "POST", conferenceJson);

    }

    public void GetSpeakers(string conferenceSlug, string sessionSlug, Action<IList<Speaker>> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/speakers";

      var client = new WebClient();
      client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
      {
        var speakers = JsonSerializer.DeserializeFromString<List<Speaker>>(args.Result);
        callback(speakers);
      };

      client.DownloadStringAsync(new Uri(url));
    }

    public void GetSpeakers(string conferenceSlug, Action<IList<Speaker>> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/speakers";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
      {
        var speakers = JsonSerializer.DeserializeFromString<List<Speaker>>(args.Result);
        callback(speakers);
      };

      client.DownloadStringAsync(new Uri(url));

    }

    public void GetSpeaker(string conferenceSlug, string slug, Action<Speaker> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/speakers/" + slug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
      {
        var speaker = JsonSerializer.DeserializeFromString<Speaker>(args.Result);
        callback(speaker);
      };

      client.DownloadStringAsync(new Uri(url));
    }

    public void GetSessions(string conferenceSlug, Action<IList<Session>> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/sessions";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
        {
          var sessions = new List<Session>()
            {
              new Session() {Title = "My Session", Start = DateTime.Now},
              new Session() {Title = "AJ's Session", Start = DateTime.Now}
            };
        //var sessions = JsonSerializer.DeserializeFromString<List<Session>>(args.Result);
        callback(sessions);
      };

      client.DownloadStringAsync(new Uri(url));

    }


    public void GetSession(string conferenceSlug, string slug, Action<Session> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/sessions/" + slug;

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      client.DownloadStringCompleted += (sender, args) =>
      {
        var session = JsonSerializer.DeserializeFromString<Session>(args.Result);
        callback(session);
      };

      client.DownloadStringAsync(new Uri(url));
    }

    public void AddSession(Session session, Action<bool> callback)
    {
      string url = _baseUrl + "conferences/" + session.ConferenceSlug + "/sessions";

      var client = new WebClient();
      //client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      var sessionJson = JsonSerializer.SerializeToString(session);
      client.UploadStringCompleted += (sender, args) =>
      {
        var x = args.Result;
        callback(true);
      };
      client.UploadStringAsync(new Uri(url), "POST", sessionJson);
    }

    public void AddSpeaker(Speaker speaker, Action<bool> callback)
    {
      string url = _baseUrl + "conferences/" + speaker.ConferenceSlug + "/sessions/" + speaker.SessionSlug + "/speakers";

      var client = new WebClient();
      client.Headers[HttpRequestHeader.ContentType] = "application/json";
      client.Headers[HttpRequestHeader.Accept] = "application/json";

      var sessionJson = JsonSerializer.SerializeToString(speaker);
      client.UploadStringCompleted += (sender, args) =>
      {
        var x = args.Result;
        callback(true);
      };
      client.UploadStringAsync(new Uri(url), "POST", sessionJson);
    }
  }
}
