using System;
using System.Collections.Generic;
using System.Text;

namespace BlueInkShared
{
    public class ProjectType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Project> Projects { get; set; }
    }
}
