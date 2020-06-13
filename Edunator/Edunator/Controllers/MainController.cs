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

            if (student != null)
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

        public ActionResult ManageSubjects()
        {
            Teacher teacher = Context.Teachers.SingleOrDefault(t => t.Email == Email);

            ManageSubjectsViewModel msvm = new ManageSubjectsViewModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Subjects = Context.Subjects.ToList()
            };

            return View("ManageSubjects_Teacher", msvm);
        }

        [HttpGet]
        public ActionResult AddSubject()
        {
            Teacher teacher = Context.Teachers.Single(t => t.Email == Email);

            MainViewModel mvm = new MainViewModel { FirstName = teacher.FirstName, LastName = teacher.LastName };
            return View("AddSubject", mvm);
        }

        [HttpPost]
        public ActionResult AddSubject(string name, string forclass)
        {
            Teacher teacher = Context.Teachers.Single(t => t.Email == Email);
            _Class _class = Context._Classes.SingleOrDefault(c => c.Name == forclass);

            if (_class == null)
                return Content("Invalid class.");


            Subject newSubject = new Subject { Name = name, _ClassId = _class.Id, TeacherId = teacher.Id };

            Context.Subjects.Add(newSubject);
            Context.SaveChanges();


            ManageSubjectsViewModel msvm = new ManageSubjectsViewModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Subjects = Context.Subjects.ToList()
            };

            return View("ManageSubjects_Teacher", msvm);
        }

        public ActionResult ManageLessons()
        {
            Teacher teacher = Context.Teachers.Single(t => t.Email == Email);
            ManageLessonsViewModel mlvm = new ManageLessonsViewModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Subjects = Context.Subjects.Where(s => s.TeacherId == teacher.Id).ToList()
            };

            return View("ManageLessons_Subjects", mlvm);
        }

        [HttpGet]
        public ActionResult AddLesson(int subjectId)
        {
            Teacher teacher = Context.Teachers.Single(t => t.Email == Email);
            Subject subject = Context.Subjects.Single(s => s.Id == subjectId);

            AddLessonViewModel alvm = new AddLessonViewModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Subject = subject,
                Lessons = Context.Lessons.Where(l => l.SubjectId == subject.Id).ToList()
            };

            return View("AddLesson", alvm);
        }

        [HttpPost]
        public ActionResult AddLesson(int subjectId, string topic, string content, string imglink, string videolink, string extraslink)
        {
            Lesson newLesson = new Lesson
            {
                SubjectId = subjectId,
                Topic = topic,
                Content = content,
                LinkToImg = imglink.Length > 0 ? imglink : null,
                LinkToMovie = videolink.Length > 0 ? videolink : null,
                LinkToExtras = extraslink.Length > 0 ? extraslink : null,
                Date = DateTime.Now.ToShortDateString()
            };

            Context.Lessons.Add(newLesson);
            Context.SaveChanges();

            Teacher teacher = Context.Teachers.Single(t => t.Email == Email);
            Subject subject = Context.Subjects.Single(s => s.Id == subjectId);

            AddLessonViewModel alvm = new AddLessonViewModel
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Subject = subject,
                Lessons = Context.Lessons.Where(l => l.SubjectId == subject.Id).ToList()
            };

            return View("AddLesson", alvm);
        }

        public ActionResult MyClass()
        {
            Student student = Context.Students.Single(s => s.Email == Email);
            List<Student> students = Context.Students.Where(s => s._ClassId == student._ClassId).ToList();
            _Class _class = Context._Classes.Single(c => c.Id == student._ClassId);

            MyClassViewModel mcvm = new MyClassViewModel
            {
                Students = students,
                FirstName = student.FirstName,
                LastName = student.LastName,
                ClassName = _class.Name
            };

            return View("MyClass", mcvm);
        }

        public ActionResult MySubjects()
        {
            Student student = Context.Students.Single(s => s.Email == Email);
            _Class _class = Context._Classes.Single(c => c.Id == student._ClassId);
            List<Subject> subjects = Context.Subjects.Where(s => s._ClassId == _class.Id).ToList();


            MySubjectsViewModel msvm = new MySubjectsViewModel
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Subjects = subjects
            };

            return View("MySubjects", msvm);

        }

        public ActionResult GoToLessons(int subjectId)
        {
            Student student = Context.Students.Single(s => s.Email == Email);
            Subject subject = Context.Subjects.Single(s => s.Id == subjectId);

            List<Lesson> lessons = Context.Lessons.Where(l => l.SubjectId == subject.Id).ToList();

            AddLessonViewModel alvm = new AddLessonViewModel
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Subject = subject,
                Lessons = lessons
            };

            return View("GoToLessons", alvm);

        }

        public ActionResult MyGrades()
        {
            Student student = Context.Students.Single(s => s.Email == Email);
            List<Grade> grades = Context.Grades.Where(g => g.StudentId == student.Id).ToList();

            MyGradesViewModel mgvm = new MyGradesViewModel
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Grades = grades
            };

            return View("MyGrades", mgvm);
        }
    }
}