﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskModules.Entities;

#nullable disable

namespace TaskModules.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20240530130604_AddStoredProcedures")]
    partial class AddStoredProcedures
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TaskModules.Entities.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"), 1L, 1);

                    b.Property<string>("DepartmentLogo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DepartmentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentDeptID")
                        .HasColumnType("int");

                    b.HasKey("DepartmentId");

                    b.HasIndex("ParentDeptID");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("TaskModules.Entities.Models.Department", b =>
                {
                    b.HasOne("TaskModules.Entities.Models.Department", "ParentDept")
                        .WithMany("SubDepartments")
                        .HasForeignKey("ParentDeptID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ParentDept");
                });

            modelBuilder.Entity("TaskModules.Entities.Models.Department", b =>
                {
                    b.Navigation("SubDepartments");
                });
#pragma warning restore 612, 618
        }
    }
}