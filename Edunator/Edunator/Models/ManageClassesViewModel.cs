using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Edunator.Models
{
    public class ManageClassesViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<_Class> _Classes { get; set; }
    }
}