# SQLite demo database

Dette repository indeholder kode til at skabe en simpel SQLite database til brug i undervisning. 

## Download
[Klik her for at downloade databasen (people.db)](https://github.com/mcronberg/undervisning-db-sqlite/raw/master/db-download/people.db). Hvis den placeres i c:\temp er connectionstring som følger:

```
Data Source=c:\temp\people.db;
```

og i C# kan du evt benytte følgende kode:

```csharp
private static string databaseFil = "c:\\temp\\people.db";
private static string connectionString = "Data Source=" + databaseFil + ";
```

så kan du benytte ```connectionString``` direkte i koden.

## EF

Hvis du skal lege med databasen gennem EF så skab en ny tom Console Application (.NET Core) og tilføje NUGET pakken

```
Microsoft.EntityFrameworkCore.Sqlite
```

Du kan herefter benytte følgende model (forudsætter db er i c:\temp)

```csharp
namespace SQLiteEF
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Table("person")]
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsHealthy { get; set; }
        public int Gender { get; set; }
    }

    public class PeopleContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=c:\\temp\\people.db");
        }
    }

}
```

Hvis du vil vide mere om SQLite/EF/.NET Core så se denne [artikel](https://docs.microsoft.com/en-us/ef/core/get-started/netcore/new-db-sqlite).


## Indhold i database

Databasen består af en enkelte tabel med 200 tilfældige personer:

- PersonId (int)
- FirstName (string)
- LastName (string)
- Height (int)
- IsHealthy (bool)
- Gender (int)

## SQLite browser
Der findes et godt værktøj til SQLite databaser kaldet [SQLite browser](https://sqlitebrowser.org/dl/). Den kan du eventuelt hente hvis du vil se nærmere på design og data.


## Rettigheder
Database og kode kan benyttes frit men smid gerne et link retur til dette repository.
