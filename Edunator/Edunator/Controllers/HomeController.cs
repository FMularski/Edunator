using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Edunator.Controllers
{
    public class HomeController : Controller
    {
        private EdunatorContext Context;

        public HomeController()
        {
            Context = new EdunatorContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Register(string fname, string lname, string email, string school, string password, string role)
        {
            
            if ( role.ToLower().Equals("teacher"))
            {
                Teacher newTeacher = new Teacher { FirstName = fname, LastName = lname, Email = email, School = school, Password = password };
                Context.Teachers.Add(newTeacher);
                Context.SaveChanges();
            }
            else
            {
                Student newStudent = new Student { FirstName = fname, LastName = lname, Email = email, School = school, Password = password };
                Context.Students.Add(newStudent);
                Context.SaveChanges();
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            Teacher teacher = Context.Teachers.SingleOrDefault(t => t.Email == email);
            Student student = Context.Students.SingleOrDefault(s => s.Email == email);

            if (teacher != null || student != null)
            {
                TempData["email"] = email;
                return RedirectToAction("Index", "Main");
            }
            else return Content("Invalid login data");
        }
        
    }
}