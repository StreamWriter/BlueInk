using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueInkShared
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }
        public string ProjectUri { get; set; }
        [ForeignKey("ProjectTypeId")]
        public ProjectType ProjectType { get; set; }
    }
}
