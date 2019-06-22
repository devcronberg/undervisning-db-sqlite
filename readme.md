# SQLite demo database

Dette repository indeholder kode til at skabe en simpel SQLite database til brug i undervisning. 

## Download
[Klik her for at downloade databasen (people.db)}(https://github.com/mcronberg/undervisning-db-sqlite/raw/master/db-download/people.db). Hvis den placeres i c:\temp er connectionstring som følger:

```
Data Source=c:\temp\people.db;Version=3;
```

og i C# kan du evt benytte følgende kode:

```csharp
private static string databaseFil = "c:\\temp\\people.db";
private static string connectionString = "Data Source=" + databaseFil + ";Version=3;";
```

så kan du benytte ```connectionString``` direkte i koden.

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