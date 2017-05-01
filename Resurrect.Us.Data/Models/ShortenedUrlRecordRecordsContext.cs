using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resurrect.Us.Data.Models
{
    public class ShortenedUrlRecordRecordsContext : DbContext
    {
        public ShortenedUrlRecordRecordsContext(DbContextOptions<ShortenedUrlRecordRecordsContext> options)
            :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedUrlRecordRecord>()
                .HasIndex(r => r.Url)
                .HasName("Index_Url");
        }

        public DbSet<ShortenedUrlRecordRecord> ShortenedUrlRecordRecords { get; set; }
    }
}
