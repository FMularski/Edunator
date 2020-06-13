using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edunator.Models
{
    public class AddLessonViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Subject Subject { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}