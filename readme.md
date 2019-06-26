# SQLite demo database

Dette repository indeholder kode til at skabe en simpel SQLite database til brug i undervisning. 

## Download
[Klik her for at downloade databasen (people.db)](https://github.com/mcronberg/undervisning-db-sqlite/raw/master/db-download/people.db). 

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

så kan du benytte ```connectionString``` direkte i koden.

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

Hvis du skal lege med databasen gennem EF så skab en ny tom Console Application (.NET Core) og tilføj NuGet pakkerne

```
Microsoft.EntityFrameworkCore.Sqlite
Microsoft.Extensions.Logging
```

Du kan herefter benytte følgende model (forudsætter db er i c:\temp)

```csharp
namespace SQLiteEF
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;

    [Table("person")]
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsHealthy { get; set; }
        public int Gender { get; set; }
        public int Height { get; set; }

        public override string ToString()
        {
            return $"I'm {FirstName} {LastName} with id {PersonId} born {DateOfBirth.ToShortDateString()}. I'm {(IsHealthy ? "healthy" : "not healthy")}, a {(Gender == 1 ? "woman" : "man")} and {Height} cm.";
        }
        
    }

    public class PeopleContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=c:\\temp\\people.db");
            // Enable logging to console
            // optionsBuilder.UseLoggerFactory(GetLoggerFactory());

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
```

Du kan eventuel se om der er hul igennem med:

```csharp
using (SQLiteEF.PeopleContext c = new SQLiteEF.PeopleContext())
{                
    List<SQLiteEF.Person> lst;
    lst = c.People.Take(5).ToList();
    lst.ForEach(i => Console.WriteLine(i));
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
