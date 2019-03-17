using System;
using System.Collections.Generic;
using System.Text;

namespace BlueInk.Shared
{
    public class OwnerData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<SocialLink> SocialLinks { get; set; }
    }
}
