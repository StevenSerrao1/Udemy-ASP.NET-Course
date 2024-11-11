using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class getAllPersons_UpdateTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Persons_CountryId",
                table: "Persons",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Countries_CountryId",
                table: "Persons",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");

            string sp_AlterGetAllPersons = @"
               ALTER PROCEDURE [dbo].[GetAllPersons]
               AS BEGIN
                  SELECT PersonId, PersonName, PersonEmail, DOB, Gender, CountryId, PersonAddress, ReceivesNewsletters, TaxIdentificationNumber
                  FROM [dbo].[Persons];
               END
    ";
            migrationBuilder.Sql(sp_AlterGetAllPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Countries_CountryId",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_CountryId",
                table: "Persons");
        }
    }
}