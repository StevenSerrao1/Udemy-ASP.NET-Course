using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class getAllPersons_spUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_AlterGetAllPersons = @"
               ALTER PROCEDURE [dbo].[GetAllPersons]
               AS BEGIN
                  SELECT PersonId, PersonName, PersonEmail, DOB, Gender, CountryId, PersonAddress, ReceivesNewsletters, TIN
                  FROM [dbo].[Persons];
               END
    ";
            migrationBuilder.Sql(sp_AlterGetAllPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_RevertGetAllPersons = @"
           ALTER PROCEDURE [dbo].[GetAllPersons]
           AS BEGIN
              SELECT PersonId, PersonName, PersonEmail, DOB, Gender, CountryId, PersonAddress, ReceivesNewsletters FROM [dbo].[Persons];
           END
    ";
            migrationBuilder.Sql(sp_RevertGetAllPersons);
        }
    }
}
