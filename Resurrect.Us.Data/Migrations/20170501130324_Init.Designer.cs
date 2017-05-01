using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Resurrect.Us.Data.Models;

namespace Resurrect.Us.Data.Migrations
{
    [DbContext(typeof(ShortenedUrlRecordRecordsContext))]
    [Migration("20170501130324_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Resurrect.Us.Data.Models.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ShortenedUrlRecordId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ShortenedUrlRecordId");

                    b.ToTable("Keyword");
                });

            modelBuilder.Entity("Resurrect.Us.Data.Models.ShortenedUrlRecordRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AccessCount");

                    b.Property<DateTime>("LastAccess");

                    b.Property<string>("Timestamp");

                    b.Property<string>("Title");

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Url")
                        .HasName("Index_Url");

                    b.ToTable("ShortenedUrlRecordRecords");
                });

            modelBuilder.Entity("Resurrect.Us.Data.Models.Keyword", b =>
                {
                    b.HasOne("Resurrect.Us.Data.Models.ShortenedUrlRecordRecord", "ShortenedUrlRecord")
                        .WithMany("Keywords")
                        .HasForeignKey("ShortenedUrlRecordId");
                });
        }
    }
}
