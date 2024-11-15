﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241105085656_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryName")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryId = new Guid("efcbe1b3-8f18-4e56-b0f3-2f8d42c38243"),
                            CountryName = "U.S.A"
                        },
                        new
                        {
                            CountryId = new Guid("bcc5788e-04ef-4f1f-a64e-d82146c51b67"),
                            CountryName = "Germany"
                        },
                        new
                        {
                            CountryId = new Guid("5b7e18ec-3bb0-4c3d-86c5-0f6f1ad1ee90"),
                            CountryName = "Australia"
                        },
                        new
                        {
                            CountryId = new Guid("40739b70-7544-4474-aa0b-464e2c6debd2"),
                            CountryName = "South-Africa"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("PersonAddress")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PersonEmail")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("PersonName")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("ReceivesNewsletters")
                        .HasColumnType("bit");

                    b.HasKey("PersonId");

                    b.ToTable("Persons", (string)null);

                    b.HasData(
                        new
                        {
                            PersonId = new Guid("123b6bed-36d2-495c-96db-4b4325219a42"),
                            CountryId = new Guid("ee878c05-7fe6-4f23-8c44-a4cd10d410a2"),
                            DOB = new DateTime(2000, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = "Male",
                            PersonAddress = "30 Rockefeller Ave, Klerskdorp, North-West",
                            PersonEmail = "stevesemail@gmail.com",
                            PersonName = "Steve",
                            ReceivesNewsletters = true
                        },
                        new
                        {
                            PersonId = new Guid("e0422365-6f36-4798-ad50-4f3e6258d878"),
                            CountryId = new Guid("02ca2b03-63bd-4891-9a59-e92436ca0f33"),
                            DOB = new DateTime(1980, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = "Male",
                            PersonAddress = "Dolbow, Quantico, Virginia",
                            PersonEmail = "zugzwang@gmail.com",
                            PersonName = "Spencer",
                            ReceivesNewsletters = false
                        },
                        new
                        {
                            PersonId = new Guid("be34573f-020e-4615-abe2-efc26a17377c"),
                            CountryId = new Guid("02ca2b03-63bd-4891-9a59-e92436ca0f33"),
                            DOB = new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = "Male",
                            PersonAddress = "91 Evergreen Terrace, Springfield, Massachusetts",
                            PersonEmail = "donuts@gmail.com",
                            PersonName = "Homer",
                            ReceivesNewsletters = true
                        },
                        new
                        {
                            PersonId = new Guid("8bd19d48-a0e7-4f03-bcb6-054c968d39db"),
                            CountryId = new Guid("02ca2b03-63bd-4891-9a59-e92436ca0f33"),
                            DOB = new DateTime(1991, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = "Female",
                            PersonAddress = "14 Rathole Ave., Chicago, Illinois",
                            PersonEmail = "fuckoff@gmail.com",
                            PersonName = "Fiona",
                            ReceivesNewsletters = false
                        },
                        new
                        {
                            PersonId = new Guid("0953ef08-4f88-4e69-869f-e9a15f68e846"),
                            CountryId = new Guid("ee878c05-7fe6-4f23-8c44-a4cd10d410a2"),
                            DOB = new DateTime(1941, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = "Male",
                            PersonAddress = "940 Groothuis, Cape Town, Western Cape",
                            PersonEmail = "backstroke@gmail.com",
                            PersonName = "Ryk",
                            ReceivesNewsletters = true
                        },
                        new
                        {
                            PersonId = new Guid("62d41e59-c811-4bbf-b1bb-557f9e0d3b6c"),
                            CountryId = new Guid("641670a5-fed6-44e9-ab25-e3b18c6dc7c8"),
                            DOB = new DateTime(1997, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gender = "Male",
                            PersonAddress = "Verstappen House, Berlin, Germany",
                            PersonEmail = "needforspeed@gmail.com",
                            PersonName = "Max",
                            ReceivesNewsletters = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
