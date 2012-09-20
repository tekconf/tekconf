using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using TekConf.RemoteData.Dtos.v1;
using TekConf.RemoteData.v1;

namespace TekConf.UI.Web.Controllers
{
  public class SpeakerController : AsyncController
  {
    public string BaseUrl()
    {
      return ConfigurationManager.AppSettings["baseUrl"];
    }

    public void IndexAsync(string conferenceSlug, string sessionSlug)
    {
      var remoteData = new RemoteDataRepository(BaseUrl());
      AsyncManager.OutstandingOperations.Increment();
      remoteData.GetSpeakers(conferenceSlug, sessionSlug, sessions =>
      {
        AsyncManager.Parameters["sessions"] = sessions;
        AsyncManager.OutstandingOperations.Decrement();
      });
    }

    public ActionResult IndexCompleted(List<SpeakersDto> speakers)
    {
      return View(speakers);
    }

    [HttpPost]
    public void CreateAsync(SpeakerDto speaker)
    {
      var remoteData = new RemoteDataRepository(BaseUrl());
      AsyncManager.OutstandingOperations.Increment();
      //remoteData.AddSpeaker(speaker, b =>
      //{
        //AsyncManager.Parameters["conference"] = conference;
        AsyncManager.OutstandingOperations.Decrement();
      //});
    }

    public ActionResult Create()
    {
      return View();
    }

    public void DetailAsync(string conferenceSlug, string sessionSlug, string speakerSlug)
    {
        var remoteData = new RemoteDataRepository(BaseUrl());
        AsyncManager.OutstandingOperations.Increment();
        remoteData.GetSpeaker(conferenceSlug, speakerSlug, speaker =>
        {
            AsyncManager.Parameters["speaker"] = speaker;
            AsyncManager.OutstandingOperations.Decrement();
        });
    }

    public ActionResult DetailCompleted(SpeakerDto speaker)
    {
        return View(speaker);
    }
  }
}
