using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Edunator.Models;

namespace Edunator.Controllers
{
    public class MainController : Controller
    {
        private EdunatorContext Context;
        private static string Email = string.Empty;

        public MainController()
        {
            Context = new EdunatorContext();
        }

        // GET: Main
        public ActionResult Index()
        {
            if (TempData["email"] != null)
                Email = TempData["email"].ToString();

            Teacher teacher = Context.Teachers.SingleOrDefault(t => t.Email == Email);
            Student student = Context.Students.SingleOrDefault(s => s.Email == Email);

            if (teacher != null)
            {
                MainViewModel mvm = new MainViewModel { FirstName = teacher.FirstName, LastName = teacher.LastName };
                return View("Main_Teacher", mvm);
            }
            else
            {
                MainViewModel mvm = new MainViewModel { FirstName = student.FirstName, LastName = student.LastName };
                return View("Main_Student", mvm);
            }
                
        }
    }
}