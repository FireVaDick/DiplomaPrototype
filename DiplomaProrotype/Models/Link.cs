using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace DiplomaProrotype.Models
{
    public class Link
    {
        public string FirstTargetType { get; set; }
        public int FirstTargetListId { get; set; }
        public string LastTargetType { get; set; }
        public int LastTargetListId { get; set; }
        public int LinkId { get; set; }
        public Line LineInfo { get; set; }
        public Ellipse CircleInfo { get; set; }

        public Link()
        {
            FirstTargetType = "";
            FirstTargetListId = -1;
            LastTargetType = "";
            LastTargetListId = -1;
            LinkId = -1;
            LineInfo = new Line();
            CircleInfo = new Ellipse();
        }
    }
}
