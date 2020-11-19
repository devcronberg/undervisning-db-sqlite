# SQLite demo database

Dette repository indeholder kode til at skabe en simpel SQLite database til brug i undervisning.

## Download

[Klik her for at downloade databasen (people.db)](https://github.com/devcronberg/undervisning-db-sqlite/raw/master/undervisning-db-sqlite/Data/people.db), og placer den i c:\temp.

## ADO

For at arbejde med SQLite i en core applikation skal du fra NuGet hente

```
System.Data.SQLite.Core
```

Hvis førnævnte people.db placeres i c:\temp er connectionstring som følger:

```
Data Source=c:\temp\people.db;
```

og i C# kan du evt benytte følgende kode:

```csharp
private static string databaseFil = "c:\\temp\\people.db";
private static string connectionString = "Data Source=" + databaseFil;
```

så kan du benytte `connectionString` direkte i koden.

Her er et kort eksempel:

```csharp
using (SQLiteConnection cn = new SQLiteConnection(connectionString))
{
    cn.Open();
    using (SQLiteCommand cm = new SQLiteCommand(cn))
    {
        cm.CommandText = "select count(*) from person where height<160";
        cm.CommandType = System.Data.CommandType.Text;
        object resultat = cm.ExecuteScalar();
        Console.WriteLine($"Der er {resultat} under 160 i tabellen");
    }
}
```

## EF

> Bemærk - koden er opdateret til .NET 5 / EF 5

Hvis du skal lege med databasen gennem EF så skab en ny tom Console Application (.NET 5) og tilføj NuGet pakken

```
Microsoft.EntityFrameworkCore.Sqlite
```

Du kan herefter benytte følgende model (forudsætter db er i c:\temp)

```csharp
namespace SQLiteEF
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

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
            return $"{this.PersonId} {this.FirstName} {this.LastName}";
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
            return $"{this.CountryId} {this.Name}";

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
            optionsBuilder.LogTo(Console.WriteLine);
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
    }

}
```

Du kan eventuel se om der er hul igennem med:

```csharp
// people
using (PeopleContext c = new PeopleContext(path))
{
    var res = c.People.Where(i => i.Height > 170 && i.IsHealthy).OrderBy(i => i.LastName).ToList();
    res.ForEach(i => Console.WriteLine(i));
}

// navigation property
using (PeopleContext c = new PeopleContext(path))
{
    var res = c.Countries.Include(i => i.People).ToList();
    res.ForEach(i =>
    {
        Console.WriteLine(i.Name);
        i.People.ForEach(x => Console.WriteLine("\t" + x));
    });
}

// join
using (PeopleContext c = new PeopleContext(path))
{
    var res = from country in c.Countries orderby country.CountryId join person in c.People on country.CountryId equals person.CountryId select new { person.FirstName, person.LastName, country.Name };
    res.ToList().ForEach(i => Console.WriteLine(i.FirstName + " " + i.LastName + " from " + i.Name));
}
```

Hvis du vil vide mere om SQLite/EF/.NET Core så se denne [artikel](https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite).

### WebApi

Se eventuelt [dette](https://github.com/devcronberg/WebApiDemo) repository.

### OData

Se eventuelt [dette](https://github.com/devcronberg/ODataDemo) repository.

### Identity med SqLite

Se eventuelt [dette](https://github.com/devcronberg/aspnetcore22-identity-ef-sqlite) repository.

## Indhold i database

Databasen består af en person tabel med 200 tilfældige personer:

- PersonId (int)
- FirstName (string)
- LastName (string)
- Height (int)
- IsHealthy (bool)
- Gender (int)
- CountryId

og en country tabel med fire lande

- CountryId
- Name

## SQLite browser

Der findes et godt værktøj til SQLite databaser kaldet [SQLite browser](https://sqlitebrowser.org/dl/). Den kan du eventuelt hente hvis du vil se nærmere på design og data.

## Rettigheder

Database og kode kan benyttes frit men smid gerne et link retur til dette repository.
