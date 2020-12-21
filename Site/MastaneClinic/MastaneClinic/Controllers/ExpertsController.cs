using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;

namespace MastaneClinic.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ExpertsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View(db.Experts.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expert expert = db.Experts.Find(id);
            if (expert == null)
            {
                return HttpNotFound();
            }
            return View(expert);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Expert expert, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed

                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Service/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    expert.ImageUrl = newFilenameUrl;
                }

                #endregion

                CodeGenerator codeGenerator=new CodeGenerator();
                expert.Code = codeGenerator.ReturnExpertCode();
                expert.IsDeleted=false;
				expert.CreationDate= DateTime.Now; 
                expert.Id = Guid.NewGuid();
                db.Experts.Add(expert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expert);
        }


        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expert expert = db.Experts.Find(id);
            if (expert == null)
            {
                return HttpNotFound();
            }
            return View(expert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Expert expert, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed

                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Service/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    expert.ImageUrl = newFilenameUrl;
                }

                #endregion
                expert.IsDeleted = false;
				expert.LastModifiedDate = DateTime.Now;
                db.Entry(expert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expert);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expert expert = db.Experts.Find(id);
            if (expert == null)
            {
                return HttpNotFound();
            }
            return View(expert);
        }

        // POST: Experts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Expert expert = db.Experts.Find(id);
			expert.IsDeleted=true;
			expert.DeletionDate=DateTime.Now;
 
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
        [Route("expert")]
        public ActionResult List()
        {
            ExpertListViewModel result = new ExpertListViewModel()
            {
                Experts = db.Experts.Where(c => c.IsDeleted == false && c.IsActive).OrderBy(c=>c.Order).ToList()
            };
            return View(result);
        }
    }
}
