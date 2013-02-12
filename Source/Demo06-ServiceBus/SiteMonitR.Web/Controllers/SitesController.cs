using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteMonitR.Web.Models;
using System.Configuration;

namespace SiteMonitR.Web.Controllers
{
    public class SitesController : Controller
    {
        private SiteMonitRContext db = new SiteMonitRContext();

        //
        // GET: /Sites/

        public ActionResult Index()
        {
            ViewBag.environment = ConfigurationManager.AppSettings["environment"];
            return View(db.MonitoredSites.ToList());
        }

        //
        // GET: /Sites/Details/5

        public ActionResult Details(int id = 0)
        {
            MonitoredSite monitoredsite = db.MonitoredSites.Find(id);
            if (monitoredsite == null)
            {
                return HttpNotFound();
            }
            return View(monitoredsite);
        }

        //
        // GET: /Sites/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Sites/Create

        [HttpPost]
        public ActionResult Create(MonitoredSite monitoredsite)
        {
            if (ModelState.IsValid)
            {
                db.MonitoredSites.Add(monitoredsite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(monitoredsite);
        }

        //
        // GET: /Sites/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MonitoredSite monitoredsite = db.MonitoredSites.Find(id);
            if (monitoredsite == null)
            {
                return HttpNotFound();
            }
            return View(monitoredsite);
        }

        //
        // POST: /Sites/Edit/5

        [HttpPost]
        public ActionResult Edit(MonitoredSite monitoredsite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monitoredsite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(monitoredsite);
        }

        //
        // GET: /Sites/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MonitoredSite monitoredsite = db.MonitoredSites.Find(id);
            if (monitoredsite == null)
            {
                return HttpNotFound();
            }
            return View(monitoredsite);
        }

        //
        // POST: /Sites/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MonitoredSite monitoredsite = db.MonitoredSites.Find(id);
            db.MonitoredSites.Remove(monitoredsite);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}