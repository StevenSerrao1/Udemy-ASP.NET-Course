using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddPerson_storedProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           string sp_addPerson = @"
           
            CREATE PROCEDURE [dbo].[AddPerson](@PersonId uniqueidentifier, @DOB datetime2, @Gender nvarchar(10), @PersonAddress nvarchar(100), @PersonEmail nvarchar(40), @PersonName nvarchar(40), @ReceivesNewsletters bit, @CountryId uniqueidentifier)
            AS BEGIN
            
            INSERT INTO [dbo].[Persons](PersonId, PersonName, PersonEmail, CountryId, DOB, PersonAddress, Gender, ReceivesNewsletters) VALUES (@PersonId, @PersonName, @PersonEmail, @CountryId, @DOB, @PersonAddress, @Gender, @ReceivesNewsletters)
            END
           ";

            migrationBuilder.Sql(sp_addPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_addPerson = @"          
            DROP PROCEDURE [dbo].[AddPerson]
           ";

            migrationBuilder.Sql(sp_addPerson);
        }
    }
}
