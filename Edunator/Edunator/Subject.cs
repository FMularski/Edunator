using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edunator
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int _ClassId { get; set; }
        public int TeacherId { get; set; }

    }
}