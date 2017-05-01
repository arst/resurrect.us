using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Resurrect.Us.Data.Models
{
    public class ShortenedUrlRecordRecord
    {
        public ShortenedUrlRecordRecord()
        {
            this.Keywords = new List<Keyword>();
        }

        [Key]
        public long Id { get; set; }
        public string Timestamp { get; set; }
        [Required]
        public string Url { get; set; }
        public DateTime LastAccess { get; set;} 
        public decimal AccessCount { get; set; }
        public string Title { get; set; }
        public  virtual List<Keyword> Keywords { get; set; }

    }
}