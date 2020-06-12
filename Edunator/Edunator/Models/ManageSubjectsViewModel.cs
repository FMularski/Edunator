using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edunator.Models
{
    public class ManageSubjectsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}