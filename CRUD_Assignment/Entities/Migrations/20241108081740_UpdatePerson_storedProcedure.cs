using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePerson_storedProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_updatePerson = @"
            CREATE PROCEDURE [dbo].[UpdatePerson]
                @PersonId uniqueidentifier, 
                @DOB datetime2, 
                @Gender nvarchar(10), 
                @PersonAddress nvarchar(100), 
                @PersonEmail nvarchar(40), 
                @PersonName nvarchar(40), 
                @ReceivesNewsletters bit, 
                @CountryId uniqueidentifier
            AS 
            BEGIN
                UPDATE [dbo].[Persons]
                SET DOB = @DOB,
                    Gender = @Gender,
                    PersonAddress = @PersonAddress,
                    PersonEmail = @PersonEmail,
                    PersonName = @PersonName,                   
                    ReceivesNewsletters = @ReceivesNewsletters,
                    CountryId = @CountryId
                WHERE PersonId = @PersonId;
            END";

            migrationBuilder.Sql(sp_updatePerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_updatePerson = @"          
            DROP PROCEDURE [dbo].[UpdatePerson]
            ";

            migrationBuilder.Sql(sp_updatePerson);
        }
    }
}
