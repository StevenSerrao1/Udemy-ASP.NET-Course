using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class DeletePerson_storedProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_deletePerson = @"
           
            CREATE PROCEDURE [dbo].[DeletePerson]
            @PersonId uniqueidentifier, 
            @DOB datetime2,
            @Gender nvarchar(10),
            @PersonAddress nvarchar(100),
            @PersonEmail nvarchar(40),
            @PersonName nvarchar(40),
            @ReceivesNewsletters bit,
            @CountryId uniqueidentifier

            AS BEGIN
            
            DELETE FROM [dbo].[Persons]
            WHERE PersonId = @PersonId
                AND DOB = @DOB
                AND Gender = @Gender
                AND PersonAddress = @PersonAddress
                AND PersonEmail = @PersonEmail
                AND PersonName = @PersonName
                AND ReceivesNewsletters = @ReceivesNewsletters
                AND CountryId = @CountryId;

            END
           ";

            migrationBuilder.Sql(sp_deletePerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_deletePerson = @"          
            DROP PROCEDURE [dbo].[DeletePerson]
           ";

            migrationBuilder.Sql(sp_deletePerson);
        }
    }
}
