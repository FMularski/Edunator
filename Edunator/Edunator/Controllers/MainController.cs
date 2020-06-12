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

        [HttpGet]
        public ActionResult MyProfile()
        {
            Teacher teacher = Context.Teachers.SingleOrDefault(t => t.Email == Email);
            Student student = Context.Students.SingleOrDefault(s => s.Email == Email);

            if (teacher != null)
            {
                MyProfileViewModel mpvm = new MyProfileViewModel
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Email = teacher.Email,
                    School = teacher.School,
                    Password = teacher.Password
                };

                return View("MyProfile_Teacher", mpvm);
            }
            else
            {
                MyProfileViewModel mpvm = new MyProfileViewModel
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Email = student.Email,
                    School = student.School,
                    Password = student.Password
                };

                return View("MyProfile_Student", mpvm);
            }
        }

        public ActionResult LogOut()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult BackToMain()
        {
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

        public ActionResult ManageClasses()
        {
            Teacher teacher = Context.Teachers.SingleOrDefault(t => t.Email == Email);

            ManageClassesViewModel mcvm = new ManageClassesViewModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                _Classes = Context._Classes.ToList()
            };

            return View("ManageClasses_Teacher", mcvm);
        }

        [HttpGet]
        public ActionResult AddClass()
        {
            Teacher teacher = Context.Teachers.Single(t => t.Email == Email);

            MainViewModel mvm = new MainViewModel { FirstName = teacher.FirstName, LastName = teacher.LastName };
            return View("AddClass", mvm);
        }

        [HttpPost]
        public ActionResult AddClass(string name)
        {
            _Class newClass = new _Class { Name = name };
            Context._Classes.Add(newClass);
            Context.SaveChanges();

            Teacher teacher = Context.Teachers.Single(t => t.Email == Email);
            ManageClassesViewModel mcvm = new ManageClassesViewModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                _Classes = Context._Classes.ToList()
            };

            return View("ManageClasses_Teacher", mcvm);
        }
    }
}