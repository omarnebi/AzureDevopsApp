﻿// <auto-generated />
using System;
using AzureDevopsApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AzureDevopsApp.Migrations
{
    [DbContext(typeof(AzureDevopsDBContext))]
    partial class AzureDevopsDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AzureDevopsApp.Models.ProjectInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastUpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceControlType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateTypeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Visibility")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProjectInfos");
                });
#pragma warning restore 612, 618
        }
    }
}
