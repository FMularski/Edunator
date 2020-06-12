using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edunator
{
    public class _Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> StudentsIds { get; set; }
        public List<int> SubjectsIds { get; set; }
    }
}