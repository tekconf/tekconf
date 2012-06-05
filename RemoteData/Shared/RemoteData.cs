using System;
using System.Collections.Generic;
using System.Net;
using ServiceStack.Text;

namespace RemoteData.Shared
{
  public class RemoteData
  {
    private const string _baseUrl = "http://conferences.herokuapp.com/";

    public void GetAllConferences(Action<IList<Conference>> callback)
    {
      string url = _baseUrl + "conferences";
      var client = new WebClient();
      Console.WriteLine("Here");
      client.DownloadStringCompleted += (sender, args) =>
                                            {
                                              var conferences = JsonSerializer.DeserializeFromString<List<Conference>>(args.Result);
                                              Console.WriteLine(conferences.Count);
                                              callback(conferences);
                                            };

      client.DownloadStringAsync(new Uri(url));
    }

  }
}