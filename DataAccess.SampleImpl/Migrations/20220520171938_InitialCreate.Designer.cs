﻿// <auto-generated />
using System;
using DataAccess.Database;
using DataAccess.SampleImpl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.SampleImpl.Migrations
{
    [DbContext(typeof(LibraryDataContext))]
    [Migration("20220520171938_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DataAccess.SampleImpl.Dto.BookDto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BookInfoId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BookInfoId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("DataAccess.SampleImpl.Dto.BookInfoDto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DatePublished")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BookInfos");
                });

            modelBuilder.Entity("DataAccess.SampleImpl.Dto.LeaseDto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BorrowerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("LeasedBookId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BorrowerId");

                    b.HasIndex("LeasedBookId");

                    b.ToTable("Leases");
                });

            modelBuilder.Entity("DataAccess.SampleImpl.Dto.ReturnDto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Returns");
                });

            modelBuilder.Entity("DataAccess.SampleImpl.Dto.UserDto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataAccess.SampleImpl.Dto.BookDto", b =>
                {
                    b.HasOne("DataAccess.SampleImpl.Dto.BookInfoDto", "BookInfo")
                        .WithMany()
                        .HasForeignKey("BookInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookInfo");
                });

            modelBuilder.Entity("DataAccess.SampleImpl.Dto.LeaseDto", b =>
                {
                    b.HasOne("DataAccess.SampleImpl.Dto.UserDto", "Borrower")
                        .WithMany()
                        .HasForeignKey("BorrowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.SampleImpl.Dto.BookDto", "LeasedBook")
                        .WithMany()
                        .HasForeignKey("LeasedBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Borrower");

                    b.Navigation("LeasedBook");
                });
#pragma warning restore 612, 618
        }
    }
}