using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtekSoftware.Conference.UI.Web.Controllers
{
  public class SpeakerController : AsyncController
  {
    public string BaseUrl()
    {
      return ConfigurationManager.AppSettings["baseUrl"];
    }

    public void IndexAsync(string conferenceSlug, string sessionSlug)
    {
      var remoteData = new RemoteData.Shared.RemoteData(BaseUrl());
      AsyncManager.OutstandingOperations.Increment();
      remoteData.GetSpeakers(conferenceSlug, sessionSlug, sessions =>
      {
        AsyncManager.Parameters["sessions"] = sessions;
        AsyncManager.OutstandingOperations.Decrement();
      });
    }

    public ActionResult IndexCompleted(List<RemoteData.Shared.Speaker> speakers)
    {
      return View(speakers);
    }

    [HttpPost]
    public void CreateAsync(RemoteData.Shared.Speaker speaker)
    {
      var remoteData = new RemoteData.Shared.RemoteData(BaseUrl());
      AsyncManager.OutstandingOperations.Increment();
      remoteData.AddSpeaker(speaker, b =>
      {
        //AsyncManager.Parameters["conference"] = conference;
        AsyncManager.OutstandingOperations.Decrement();
      });
    }

    public ActionResult Create()
    {
      return View();
    }

    public void DetailsAsync(object conferenceslug, string slug)
    {
      throw new NotImplementedException();
    }

    public ActionResult DetailsCompleted()
    {
      throw new NotImplementedException();
    }
  }
}
