using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edunator
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string School { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int _ClassId { get; set; }
        public List<int> GradesId { get; set; }
    }
}