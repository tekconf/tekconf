using System;
using System.Collections.Generic;
using System.Net;
using ServiceStack.Text;

namespace RemoteData.Shared
{
  public class RemoteData
  {
    private const string _baseUrl = "http://conferences.herokuapp.com/";

    public void GetConferences(Action<IList<Conference>> callback)
    {
      string url = _baseUrl + "conferences";
      var client = new WebClient();

      client.DownloadStringCompleted += (sender, args) =>
                                            {
                                              var conferences = JsonSerializer.DeserializeFromString<List<Conference>>(args.Result);
                                              callback(conferences);
                                            };

      client.DownloadStringAsync(new Uri(url));

    }

    public void GetConference(string slug, Action<Conference> callback)
    {
      string url = _baseUrl + "conferences/" + slug;
      var client = new WebClient();

      client.DownloadStringCompleted += (sender, args) =>
      {
        var conference = JsonSerializer.DeserializeFromString<Conference>(args.Result);
        callback(conference);
      };

      client.DownloadStringAsync(new Uri(url));
    }

    public void GetSpeakers(string conferenceSlug, Action<IList<Speaker>> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/speakers";
      var client = new WebClient();

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

      client.DownloadStringCompleted += (sender, args) =>
      {
        var sessions = JsonSerializer.DeserializeFromString<List<Session>>(args.Result);
        callback(sessions);
      };

      client.DownloadStringAsync(new Uri(url));

    }

    public void GetSession(string conferenceSlug, string slug, Action<Session> callback)
    {
      string url = _baseUrl + "conferences/" + conferenceSlug + "/sessions/" + slug;
      var client = new WebClient();

      client.DownloadStringCompleted += (sender, args) =>
      {
        var session = JsonSerializer.DeserializeFromString<Session>(args.Result);
        callback(session);
      };

      client.DownloadStringAsync(new Uri(url));
    }

  }
}