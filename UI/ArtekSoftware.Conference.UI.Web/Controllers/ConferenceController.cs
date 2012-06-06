using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ArtekSoftware.Conference.UI.Web.Controllers
{
  public class ConferenceController : AsyncController
  {
    //
    // GET: /Conference/
    public void IndexAsync()
    {
      var remoteData = new RemoteData.Shared.RemoteData();
      IList<RemoteData.Shared.Conference> conferences = null;
      AsyncManager.OutstandingOperations.Increment();
      remoteData.GetConferences(c =>
      {
        conferences = c;
        AsyncManager.Parameters["conferences"] = conferences;
        AsyncManager.OutstandingOperations.Decrement();
      });

    }
    public ActionResult IndexCompleted(IList<RemoteData.Shared.Conference> conferences)
    {
      return View(conferences);
    }

    //
    // GET: /Conference/Details/5

    public void DetailsAsync(string slug)
    {
      var remoteData = new RemoteData.Shared.RemoteData();
      RemoteData.Shared.Conference conference = null;
      AsyncManager.OutstandingOperations.Increment();
      remoteData.GetConference(slug, c =>
      {
        conference = c;
        AsyncManager.Parameters["conference"] = conference;
        AsyncManager.OutstandingOperations.Decrement();
      });
    }

    public ActionResult DetailsCompleted(RemoteData.Shared.Conference conference)
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
    public void CreateAsync(RemoteData.Shared.Conference conference)
    {
      var remoteData = new RemoteData.Shared.RemoteData();
      AsyncManager.OutstandingOperations.Increment();
      remoteData.SaveConference(conference, b =>
      {
        //AsyncManager.Parameters["conference"] = conference;
        AsyncManager.OutstandingOperations.Decrement();
      });
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
