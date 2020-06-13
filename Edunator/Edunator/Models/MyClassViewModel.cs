using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edunator.Models
{
    public class MyClassViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassName { get; set; }
        public List<Student> Students { get; set; }
    }
}