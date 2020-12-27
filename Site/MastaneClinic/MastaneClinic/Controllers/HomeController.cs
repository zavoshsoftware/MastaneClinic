using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace MastaneClinic.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        [Route("")]
        public ActionResult Index()
        {
            HomeViewModel home=new HomeViewModel()
            {
                HomeServices = db.Services.Where(c=>c.IsDeleted==false&&c.IsActive).Take(8).ToList(),
                HomeBlogs = db.Blogs.Where(c => c.IsDeleted == false && c.IsActive).OrderByDescending(c=>c.CreationDate).Take(5).ToList(),
                HomeExperts = db.Experts.Where(c =>c.IsInHome&& c.IsDeleted == false && c.IsActive).OrderByDescending(c=>c.CreationDate).Take(5).ToList(),
            };

            return View(home);
        }

        [Route("contact")]
        public ActionResult Contact()
        {
            ContactViewModel contact = new ContactViewModel();
            return View(contact);
        }
        [Route("About")]
        public ActionResult About()
        {
            AboutViewModel contact = new AboutViewModel();
            return View(contact);
        }
    }
}