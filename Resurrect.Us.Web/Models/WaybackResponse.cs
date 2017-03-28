using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Models
{
    [DataContract]
    public class WaybackResponse
    {
        public string GetClosestUrl()
        {
            return (this.Closest != null && this.Closest.Closest != null) ? this.Closest.Closest.Url : String.Empty;
        }

        public string GetClosestTimestamp()
        {
            return (this.Closest != null && this.Closest.Closest != null) ? this.Closest.Closest.Timestamp : String.Empty;
        }

        [DataMember(Name = "archived_snapshots")]
        public ArchivedSnapshots Closest { get; set; }
    }
}
