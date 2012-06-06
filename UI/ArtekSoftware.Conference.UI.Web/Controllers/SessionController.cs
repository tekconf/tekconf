using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtekSoftware.Conference.UI.Web.Controllers
{
    public class SessionController : AsyncController
    {
        //
        // GET: /Session/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Session/Details/5

        public void DetailsAsync(string conferenceSlug, string slug)
        {
          var remoteData = new RemoteData.Shared.RemoteData();
          RemoteData.Shared.Session session = null;
          AsyncManager.OutstandingOperations.Increment();
          remoteData.GetSession(conferenceSlug, slug, c =>
          {
            session = c;
            AsyncManager.Parameters["session"] = session;
            AsyncManager.OutstandingOperations.Decrement();
          });
        }

        public ActionResult DetailsCompleted(RemoteData.Shared.Session session)
        {
          return View(session);
        }

        //
        // GET: /Session/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Session/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Session/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Session/Edit/5

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
        // GET: /Session/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Session/Delete/5

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
