using System;
using System.Collections.Generic;
using System.Text;

namespace BlueInk.Shared
{
    public class ProjectType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
