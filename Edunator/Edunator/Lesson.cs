using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edunator
{
    public class Lesson
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public string LinkToImg { get; set; }
        public string LinkToMovie { get; set; }
        public string LinkToExtras { get; set; }

    }
}