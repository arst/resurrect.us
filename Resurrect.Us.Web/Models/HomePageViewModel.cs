using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resurrect.Us.Web.Models
{
    public class HomePageViewModel
    {
        [Required]
        [Url]
        public string Url { get; set; }
    }
}
