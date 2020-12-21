using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace MastaneClinic.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VisitRequestsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var visitRequests = db.VisitRequests.Include(v => v.Service).Where(v => v.IsDeleted == false).OrderByDescending(v => v.CreationDate);
            return View(visitRequests.ToList());
        }

        // GET: VisitRequests/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitRequest visitRequest = db.VisitRequests.Find(id);
            if (visitRequest == null)
            {
                return HttpNotFound();
            }
            return View(visitRequest);
        }

        // GET: VisitRequests/Create
        public ActionResult Create()
        {
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,CellNumber,DateAndTime,ServiceId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] VisitRequest visitRequest)
        {
            if (ModelState.IsValid)
            {
                visitRequest.IsDeleted = false;
                visitRequest.CreationDate = DateTime.Now;
                visitRequest.Id = Guid.NewGuid();
                db.VisitRequests.Add(visitRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Title", visitRequest.ServiceId);
            return View(visitRequest);
        }

     
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitRequest visitRequest = db.VisitRequests.Find(id);
            if (visitRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Title", visitRequest.ServiceId);
            return View(visitRequest);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,CellNumber,DateAndTime,ServiceId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] VisitRequest visitRequest)
        {
            if (ModelState.IsValid)
            {
                visitRequest.IsDeleted = false;
                visitRequest.LastModifiedDate = DateTime.Now;
                db.Entry(visitRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Title", visitRequest.ServiceId);
            return View(visitRequest);
        }

        
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VisitRequest visitRequest = db.VisitRequests.Find(id);
            if (visitRequest == null)
            {
                return HttpNotFound();
            }
            return View(visitRequest);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VisitRequest visitRequest = db.VisitRequests.Find(id);
            visitRequest.IsDeleted = true;
            visitRequest.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        public ActionResult SubmitRequest(string fullName, string serviceId, string cellNumber, string dateTime)
        {
            try
            {
                Guid? id = null;

                if (serviceId != null)
                {
                    id = new Guid(serviceId);
                    Service service = db.Services.Find(id);

                    if (service == null)
                        id = null;
                }

                VisitRequest visitRequest = new VisitRequest()
                {
                    CreationDate = DateTime.Now,
                    IsDeleted = false,
                    Id = Guid.NewGuid(),
                    ServiceId = id,
                    IsActive = false,
                    CellNumber = cellNumber,
                    FullName = fullName,
                    DateAndTime = dateTime,
                };

                db.VisitRequests.Add(visitRequest);
                db.SaveChanges();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        [Route("Appointment")]
        public ActionResult Appointment()
        {
            AppointmentViewModel appointment = new AppointmentViewModel();
            return View(appointment);
        }

    }
}
