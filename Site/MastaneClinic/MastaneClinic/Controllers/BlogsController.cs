﻿using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace MastaneClinic.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class BlogsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

    [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var blogs = db.Blogs.Include(b => b.BlogGroup).Where(b=>b.IsDeleted==false).OrderByDescending(b=>b.CreationDate);
            return View(blogs.ToList());
        }
         

    [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.BlogGroupId = new SelectList(db.BlogGroups, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
        public ActionResult Create(Blog blog, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                string newFilenameUrl = string.Empty;
                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/Blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    blog.ImageUrl = newFilenameUrl;
                }
                #endregion
                blog.IsDeleted=false;
				blog.CreationDate= DateTime.Now; 
                blog.Id = Guid.NewGuid();
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogGroupId = new SelectList(db.BlogGroups, "Id", "Title", blog.BlogGroupId);
            return View(blog);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogGroupId = new SelectList(db.BlogGroups, "Id", "Title", blog.BlogGroupId);
            return View(blog);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Blog blog, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                string newFilenameUrl = string.Empty;
                if (fileUpload != null)
                {
                    string filename = Path.GetFileName(fileUpload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/Blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileUpload.SaveAs(physicalFilename);

                    blog.ImageUrl = newFilenameUrl;
                }
                #endregion
                blog.IsDeleted = false;
				blog.LastModifiedDate = DateTime.Now;
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogGroupId = new SelectList(db.BlogGroups, "Id", "Title", blog.BlogGroupId);
            return View(blog);
        }

    [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Blog blog = db.Blogs.Find(id);
			blog.IsDeleted=true;
			blog.DeletionDate=DateTime.Now;
 
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
        [Route("blog")]
        public ActionResult List()
        {
            BlogListViewModel result = new BlogListViewModel()
            {
                Blogs = db.Blogs.Where(c => c.IsDeleted == false && c.IsActive).OrderByDescending(c => c.CreationDate).ToList()
            };
            return View(result);
        }

        [AllowAnonymous]
        [Route("blog/post/{urlParam}")]
        public ActionResult Details(string urlParam)
        {
            Blog blog = db.Blogs.FirstOrDefault(c => c.UrlParam == urlParam && c.IsDeleted == false);
            if (blog == null)
            {
                return RedirectToAction("List");
            }

            blog.Visit += 1;
            db.SaveChanges();
            BlogDetailViewModel result = new BlogDetailViewModel()
            {
                Blog = blog,
                SidebarServices = db.Services.Where(c => c.IsDeleted == false && c.IsActive).OrderBy(c => c.Order).ToList(),
                SidebarBlog = db.Blogs.Where(c=>c.IsDeleted==false&&c.Id!=blog.Id&&c.IsActive).OrderByDescending(c=>c.CreationDate).Take(4).ToList()
            };

            return View(result);
        }

    }
}
