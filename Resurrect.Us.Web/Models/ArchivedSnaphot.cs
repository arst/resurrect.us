using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Models
{
    [DataContract]
    public class ArchivedSnapshot
    {
        [DataMember(Name = "available")]
        public bool Available { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "timestamp")]
        public string Timestamp { get; set; }
        [DataMember(Name = "status")]
        public int Status { get; set; }
    }
}
