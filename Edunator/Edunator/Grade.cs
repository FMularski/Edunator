using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edunator
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public int Value { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
    }
}