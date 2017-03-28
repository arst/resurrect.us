﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Resurrect.Us.Data.Models;

namespace Resurrect.Us.Data.Migrations
{
    [DbContext(typeof(ResurrectRecordsContext))]
    [Migration("20170328184530_Initial")]
    partial class Initial
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

                    b.Property<string>("ResurrectionRecordId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ResurrectionRecordId");

                    b.ToTable("Keyword");
                });

            modelBuilder.Entity("Resurrect.Us.Data.Models.ResurrectionRecord", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AccessCount");

                    b.Property<DateTime>("LastAccess");

                    b.Property<string>("Timestamp");

                    b.Property<string>("Title");

                    b.Property<string>("Url")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ResurrectRecords");
                });

            modelBuilder.Entity("Resurrect.Us.Data.Models.Keyword", b =>
                {
                    b.HasOne("Resurrect.Us.Data.Models.ResurrectionRecord", "ResurrectionRecord")
                        .WithMany("Keywords")
                        .HasForeignKey("ResurrectionRecordId");
                });
        }
    }
}
