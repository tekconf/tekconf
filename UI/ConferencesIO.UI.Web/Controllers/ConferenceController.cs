using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using ConferencesIO.RemoteData.Dtos.v1;
using ConferencesIO.RemoteData.v1;

namespace ConferencesIO.UI.Web.Controllers
{
  public class ConferenceController : AsyncController
  {
    public string BaseUrl()
    {
      return ConfigurationManager.AppSettings["baseUrl"];
    }

    public void IndexAsync()
    {
      var remoteData = new RemoteDataRepository(BaseUrl());
      IList<ConferencesDto> conferences = null;
      AsyncManager.OutstandingOperations.Increment();
      remoteData.GetConferences(c =>
      {
        conferences = c;
        AsyncManager.Parameters["conferences"] = conferences;
        AsyncManager.OutstandingOperations.Decrement();
      });

    }
    public ActionResult IndexCompleted(IList<ConferencesDto> conferences)
    {
      return View(conferences);
    }

    //
    // GET: /Conference/Details/5

    public void DetailsAsync(string slug)
    {
      var remoteData = new RemoteDataRepository(BaseUrl());
      ConferenceDto conference = null;
      AsyncManager.OutstandingOperations.Increment();
      remoteData.GetConference(slug, c =>
      {
        conference = c;
        AsyncManager.Parameters["conference"] = conference;
        AsyncManager.OutstandingOperations.Decrement();
      });
    }

    public ActionResult DetailsCompleted(ConferenceDto conference)
    {
      return View(conference);
    }

    //
    // GET: /Conference/Create

    public ActionResult Create()
    {
      return View();
    }

    //
    // POST: /Conference/Create

    [HttpPost]
    public void CreateAsync(ConferenceDto conference)
    {
      var remoteData = new RemoteDataRepository(BaseUrl());
      AsyncManager.OutstandingOperations.Increment();
      //remoteData.AddConference(conference, b =>
      //{
        //AsyncManager.Parameters["conference"] = conference;
        AsyncManager.OutstandingOperations.Decrement();
      //});
    }

    public ActionResult CreateCompleted()
    {
      return RedirectToAction("Index");
    }

    //
    // GET: /Conference/Edit/5

    public ActionResult Edit(int id)
    {
      return View();
    }

    //
    // POST: /Conference/Edit/5

    [HttpPost]
    public ActionResult Edit(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add update logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    //
    // GET: /Conference/Delete/5

    public ActionResult Delete(int id)
    {
      return View();
    }

    //
    // POST: /Conference/Delete/5

    [HttpPost]
    public ActionResult Delete(int id, FormCollection collection)
    {
      try
      {
        // TODO: Add delete logic here

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }
  }
}
