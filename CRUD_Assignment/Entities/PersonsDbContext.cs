using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.IO;
using Microsoft.Identity.Client;
using Microsoft.Data.SqlClient;

namespace Entities
{
    public class PersonsDbContext : DbContext
    {
        public PersonsDbContext(DbContextOptions<PersonsDbContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Assign/Map Entity types to tables with string value names
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            //Seed to Countries
            string countriesJson = File.ReadAllText("countries.json");
            List<Country> countries = JsonSerializer.Deserialize<List<Country>>(countriesJson)!;

            foreach (Country country in countries!)
                modelBuilder.Entity<Country>().HasData(country);


            //Seed to Persons
            string personsJson = File.ReadAllText("persons.json");
            List<Person> persons = JsonSerializer.Deserialize<List<Person>>(personsJson)!;

            foreach (Person person in persons!) 
            {
                modelBuilder.Entity<Person>().HasData(person);
            }
                
            // Print values for verification - not necessary but nice
            foreach (var person in persons!)
            {
                Console.WriteLine($"PersonName: {person.PersonName}, PersonId: {person.PersonId}, CountryId: {person.CountryId}");
            }

            //
            // F L U E N T API
            //

            // Add property configuration for TIN column
            modelBuilder.Entity<Person>()
                .Property(prop => prop.TIN)
                .HasColumnName("TaxIdentificationNumber")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("ABC12345");

            // Add unique value constraint to TIN column for each person / p = person
            // modelBuilder.Entity<Person>()
            // .HasIndex(p => p.TIN).IsUnique();

            // Associate a check constraint with TIN property
            modelBuilder.Entity<Person>()
                .ToTable(t => t.HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8"));

            // T A B L E RELATIONS
            //modelBuilder.Entity<Person>(entity => {
            //    entity.HasOne<Country>(c => c.Country)
            //    .WithMany(p => p.Persons)
            //    .HasForeignKey(c => c.CountryId);
            //});
                
        }

        #region Stored Procedure Methods
        // Create a method to call the stored procedure - GET ALL PEOPLE FROM DB
        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        // Create a method to call the stored procedure - INSERT PERSON INTO DB
        public int sp_AddPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonId", person.PersonId),
                new SqlParameter("@DOB", person.DOB),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@PersonAddress", person.PersonAddress),
                new SqlParameter("@PersonEmail", person.PersonEmail),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@ReceivesNewsletters", person.ReceivesNewsletters),
                new SqlParameter("@CountryId", person.CountryId),
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[AddPerson] @PersonId, @DOB, @Gender, @PersonAddress, @PersonEmail, @PersonName, @ReceivesNewsletters, @CountryId", parameters);
        }

        // Create a method to call the stored procedure - DELETE PERSON FROM DB
        public int sp_DeletePerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonId", person.PersonId),
                new SqlParameter("@DOB", person.DOB),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@PersonAddress", person.PersonAddress),
                new SqlParameter("@PersonEmail", person.PersonEmail),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@ReceivesNewsletters", person.ReceivesNewsletters),
                new SqlParameter("@CountryId", person.CountryId),
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[DeletePerson] @PersonId, @DOB, @Gender, @PersonAddress, @PersonEmail, @PersonName, @ReceivesNewsletters, @CountryId", parameters);
        }

        // Create a method to call the stored procedure - UPDATE PERSON FROM DB
        public int sp_UpdatePerson(Guid personId, DateTime? dob, string? gender,
        string? personAddress, string? personEmail, string? personName, bool receivesNewsletters, Guid? countryId)
        {
            var parameters = new[]
            {
            new SqlParameter("@PersonId", personId),
            new SqlParameter("@DOB", dob.HasValue ? dob.Value : (object)DBNull.Value),
            new SqlParameter("@Gender", gender ?? (object)DBNull.Value),
            new SqlParameter("@PersonAddress", personAddress ?? (object)DBNull.Value),
            new SqlParameter("@PersonEmail", personEmail ?? (object)DBNull.Value),
            new SqlParameter("@PersonName", personName ?? (object)DBNull.Value),
            new SqlParameter("@ReceivesNewsletters", receivesNewsletters),
            new SqlParameter("@CountryId", countryId  ?? (object)DBNull.Value)
        };

            return Database.ExecuteSqlRaw(
                "EXECUTE [dbo].[UpdatePerson] @PersonId, @DOB, @Gender, @PersonAddress, @PersonEmail, @PersonName, @ReceivesNewsletters, @CountryId",
                parameters.ToArray()
            );
        }
        #endregion
    }
}