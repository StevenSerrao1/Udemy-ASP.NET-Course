using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PersonEmail = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ReceivesNewsletters = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("40739b70-7544-4474-aa0b-464e2c6debd2"), "South-Africa" },
                    { new Guid("5b7e18ec-3bb0-4c3d-86c5-0f6f1ad1ee90"), "Australia" },
                    { new Guid("bcc5788e-04ef-4f1f-a64e-d82146c51b67"), "Germany" },
                    { new Guid("efcbe1b3-8f18-4e56-b0f3-2f8d42c38243"), "U.S.A" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "CountryId", "DOB", "Gender", "PersonAddress", "PersonEmail", "PersonName", "ReceivesNewsletters" },
                values: new object[,]
                {
                    { new Guid("0953ef08-4f88-4e69-869f-e9a15f68e846"), new Guid("ee878c05-7fe6-4f23-8c44-a4cd10d410a2"), new DateTime(1941, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "940 Groothuis, Cape Town, Western Cape", "backstroke@gmail.com", "Ryk", true },
                    { new Guid("123b6bed-36d2-495c-96db-4b4325219a42"), new Guid("ee878c05-7fe6-4f23-8c44-a4cd10d410a2"), new DateTime(2000, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "30 Rockefeller Ave, Klerskdorp, North-West", "stevesemail@gmail.com", "Steve", true },
                    { new Guid("62d41e59-c811-4bbf-b1bb-557f9e0d3b6c"), new Guid("641670a5-fed6-44e9-ab25-e3b18c6dc7c8"), new DateTime(1997, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "Verstappen House, Berlin, Germany", "needforspeed@gmail.com", "Max", false },
                    { new Guid("8bd19d48-a0e7-4f03-bcb6-054c968d39db"), new Guid("02ca2b03-63bd-4891-9a59-e92436ca0f33"), new DateTime(1991, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", "14 Rathole Ave., Chicago, Illinois", "fuckoff@gmail.com", "Fiona", false },
                    { new Guid("be34573f-020e-4615-abe2-efc26a17377c"), new Guid("02ca2b03-63bd-4891-9a59-e92436ca0f33"), new DateTime(1960, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "91 Evergreen Terrace, Springfield, Massachusetts", "donuts@gmail.com", "Homer", true },
                    { new Guid("e0422365-6f36-4798-ad50-4f3e6258d878"), new Guid("02ca2b03-63bd-4891-9a59-e92436ca0f33"), new DateTime(1980, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "Dolbow, Quantico, Virginia", "zugzwang@gmail.com", "Spencer", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
