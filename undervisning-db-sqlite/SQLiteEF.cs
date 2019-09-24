namespace SQLiteEF
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    public enum GenderType
    {
        Male,
        Female
    }

    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsHealthy { get; set; }
        public GenderType Gender { get; set; }
        public int Height { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public override string ToString()
        {
            return $"{PersonId} {FirstName} {LastName}";
        }
    }

    
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        
        // Optional
        public List<Person> People { get; set; }

        public override string ToString()
        {
            return $"{CountryId} {Name}";

        }
    }

    public class PeopleContext : DbContext
    {
        private readonly string pathToDb;

        public DbSet<Person> People { get; set; }
        public DbSet<Country> Countries { get; set; }

        public PeopleContext(string pathToDb = @"c:\temp\people.db")
        {
            this.pathToDb = pathToDb;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {           
            optionsBuilder.UseSqlite("Data Source=" + pathToDb);
            // Enable logging to console
            optionsBuilder.UseLoggerFactory(GetLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(e =>
            {
                e.ToTable("Person");
                e.Property(i => i.Gender).HasConversion(x => x.ToString(), x => (GenderType)Enum.Parse(typeof(GenderType), x));
                e.HasOne(p => p.Country).WithMany(b => b.People).HasForeignKey(p => p.CountryId);
            });

            modelBuilder.Entity<Country>(e =>
            {
                e.ToTable("Country");
            });

            base.OnModelCreating(modelBuilder);
        }

        // For logging...
        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddConsole()
                          .AddFilter(DbLoggerCategory.Database.Command.Name,
                                     LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }
    }

}
