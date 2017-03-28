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
            var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=resurrectus;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            optionsBuilder.UseSqlServer(connection);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<ResurrectionRecord> ResurrectRecords { get; set; }
    }
}
