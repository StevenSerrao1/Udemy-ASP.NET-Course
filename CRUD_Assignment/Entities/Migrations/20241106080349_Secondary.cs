using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Secondary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("40739b70-7544-4474-aa0b-464e2c6debd2"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("5b7e18ec-3bb0-4c3d-86c5-0f6f1ad1ee90"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("bcc5788e-04ef-4f1f-a64e-d82146c51b67"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("efcbe1b3-8f18-4e56-b0f3-2f8d42c38243"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("0953ef08-4f88-4e69-869f-e9a15f68e846"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("123b6bed-36d2-495c-96db-4b4325219a42"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("62d41e59-c811-4bbf-b1bb-557f9e0d3b6c"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("8bd19d48-a0e7-4f03-bcb6-054c968d39db"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("be34573f-020e-4615-abe2-efc26a17377c"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("e0422365-6f36-4798-ad50-4f3e6258d878"));

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), "China" },
                    { new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"), "Philippines" },
                    { new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"), "China" },
                    { new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"), "Thailand" },
                    { new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"), "Palestinian Territory" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "CountryId", "DOB", "Gender", "PersonAddress", "PersonEmail", "PersonName", "ReceivesNewsletters" },
                values: new object[,]
                {
                    { new Guid("012107df-862f-4f16-ba94-e5c16886f005"), new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), new DateTime(1990, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "413 Sachtjen Way", "hmosco8@tripod.com", "Hansiain", true },
                    { new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"), new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"), new DateTime(1990, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", "2 Warrior Avenue", "mconachya@va.gov", "Minta", true },
                    { new Guid("29339209-63f5-492f-8459-754943c74abf"), new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), new DateTime(1983, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "57449 Brown Way", "mjarrell6@wisc.edu", "Maddy", true },
                    { new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"), new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"), new DateTime(1988, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "97570 Raven Circle", "mlingfoot5@netvibes.com", "Mitchael", false },
                    { new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"), new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"), new DateTime(1995, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gender", "50467 Holy Cross Crossing", "ttregona4@stumbleupon.com", "Tani", false },
                    { new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"), new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"), new DateTime(1987, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", "9334 Fremont Street", "vklussb@nationalgeographic.com", "Verene", true },
                    { new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"), new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), new DateTime(1998, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", "4 Stuart Drive", "pretchford7@virginia.edu", "Pegeen", true },
                    { new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"), new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"), new DateTime(1989, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", "4 Parkside Point", "mwebsdale0@people.com.cn", "Marguerite", false },
                    { new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"), new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"), new DateTime(1990, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", "6 Morningstar Circle", "ushears1@globo.com", "Ursa", false },
                    { new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"), new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"), new DateTime(1995, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "73 Heath Avenue", "fbowsher2@howstuffworks.com", "Franchot", true },
                    { new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"), new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"), new DateTime(1997, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "484 Clarendon Court", "lwoodwing9@wix.com", "Lombard", false },
                    { new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"), new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), new DateTime(1987, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "83187 Merry Drive", "asarvar3@dropbox.com", "Angie", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("12e15727-d369-49a9-8b13-bc22e9362179"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"));

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryId",
                keyValue: new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("012107df-862f-4f16-ba94-e5c16886f005"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("29339209-63f5-492f-8459-754943c74abf"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"));

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                table: "Countries",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
