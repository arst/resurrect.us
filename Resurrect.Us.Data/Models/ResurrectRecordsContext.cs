using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resurrect.Us.Data.Models
{
    public class ResurrectRecordsContext : DbContext
    {
        public ResurrectRecordsContext()
        {

        }

        public ResurrectRecordsContext(DbContextOptions<ResurrectRecordsContext> options)
            :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResurrectionRecord>()
                .HasIndex(r => r.Url)
                .HasName("Index_Url");
        }

        public DbSet<ResurrectionRecord> ResurrectRecords { get; set; }
    }
}
