using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tasks.Data;

namespace Tasks.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Task> Tasks { get; set; }
        public User CurrentUser { get; set; }
    }
}