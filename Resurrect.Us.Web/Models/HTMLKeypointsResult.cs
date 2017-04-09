using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Models
{
    public class HTMLKeypointsResult
    {
        public HTMLKeypointsResult()
        {
            this.Keywords = new List<string>();
        }

        public string Title { get; set; }
        public List<string> Keywords { get; set; }
    }
}
