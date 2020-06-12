using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edunator.Models
{
    public class GiveGradeViewModel
    {
        public string FirstNameTeacher { get; set; }
        public string LastNameTeacher { get; set; }
        public string FirstNameStudent { get; set; }
        public string LastNameStudent { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }

    }
}