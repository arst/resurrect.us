using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Models
{
    [DataContract]
    public class ArchivedSnapshots
    {
        [DataMember(Name = "closest")]
        public ArchivedSnapshot Closest { get; set; }
    }
}
