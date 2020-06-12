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

        [HttpGet]
        public ActionResult EditClass(int classId)
        {
            _Class classToEdit = Context._Classes.Single(c => c.Id == classId);
            Teacher teacher = Context.Teachers.Single(t => t.Email == Email);

            EditClassViewModel ecvm = new EditClassViewModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                ClassId = classToEdit.Id,
                ClassName = classToEdit.Name,
                Students = Context.Students.Where(s => s._ClassId == classId).ToList()
            };

            return View("EditClass", ecvm);
        }

        public ActionResult AddStudentToClass(int _classId, string fname, string lname)
        {
            _Class _class = Context._Classes.Single(c => c.Id == _classId);
            List<Student> students = Context.Students.Where(s => s.FirstName == fname).ToList();

            if (students.Count == 0)
                return Content("Invalid student.");

            Student student = students.First(s => s.LastName == lname);

            if ( student != null)
            {
                student._ClassId = _classId;
                Context.SaveChanges();

                Teacher teacher = Context.Teachers.Single(t => t.Email == Email);

                EditClassViewModel ecvm = new EditClassViewModel
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    ClassId = _class.Id,
                    ClassName = _class.Name,
                    Students = Context.Students.Where(s => s._ClassId == _classId).ToList()
                };

                return View("EditClass", ecvm);
            }
            else
                return Content("Invalid student.");
        }

        public ActionResult RemoveStudentFromClass(int studentId)
        {
            Student studentToRemove = Context.Students.Single(s => s.Id == studentId);
            _Class _class = Context._Classes.Single(c => c.Id == studentToRemove._ClassId);

            studentToRemove._ClassId = 0;
            Context.SaveChanges();

            Teacher teacher = Context.Teachers.Single(t => t.Email == Email);

            EditClassViewModel ecvm = new EditClassViewModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                ClassId = _class.Id,
                ClassName = _class.Name,
                Students = Context.Students.Where(s => s._ClassId == _class.Id).ToList()
            };

            return View("EditClass", ecvm);
        }

        [HttpGet]
        public ActionResult GiveGrade(int studentId)
        {
            Student student = Context.Students.Single(s => s.Id == studentId);
            Teacher teacher = Context.Teachers.Single(t => t.Email == Email);

            GiveGradeViewModel ggvm = new GiveGradeViewModel
            {
                FirstNameTeacher = teacher.FirstName,
                LastNameTeacher = teacher.LastName,
                FirstNameStudent = student.FirstName,
                LastNameStudent = student.LastName,
                StudentId = student.Id,
                TeacherId = teacher.Id
            };

            return View("GiveGrade", ggvm);
        }

        [HttpPost]
        public ActionResult GiveGrade(int studentId, int teacherId, int grade, string desc)
        {
            Grade _grade = new Grade
            {
                StudentId = studentId,
                TeacherId = teacherId,
                Value = grade,
                Date = DateTime.Now.ToShortDateString(),
                Description = desc
            };

            Context.Grades.Add(_grade);
            Context.SaveChanges();

            Student student = Context.Students.Single(s => s.Id == studentId);
            _Class _class = Context._Classes.Single(c => c.Id == student._ClassId);
            Teacher teacher = Context.Teachers.Single(t => t.Id == teacherId);

            EditClassViewModel ecvm = new EditClassViewModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                ClassId = _class.Id,
                ClassName = _class.Name,
                Students = Context.Students.Where(s => s._ClassId == _class.Id).ToList()
            };

            return View("EditClass", ecvm);
        }
    }
}