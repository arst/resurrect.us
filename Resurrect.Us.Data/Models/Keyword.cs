using System.ComponentModel.DataAnnotations;

namespace Resurrect.Us.Data.Models
{
    public class Keyword
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public virtual ResurrectionRecord ResurrectionRecord { get; set; }
    }
}