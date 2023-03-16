using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaProrotype
{
    internal class Link
    {
        public string FirstTargetType { get; set; }
        public int FirstTargetListId { get; set; }
        public string LastTargetType { get; set; }
        public int LastTargetListId { get; set; }
        public int LinkId { get; set; }


        public Link()
        {
            FirstTargetType = "";
            FirstTargetListId = 0;
            LastTargetType = "";
            LastTargetListId = 0;
            LinkId = 0;
        }
    }
}
