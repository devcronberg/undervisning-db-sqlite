# SQLite demo database

Dette repository indeholder kode til at skabe en simpel SQLite database til brug i undervisning. 

## Download
Klik her for at downloade databasen (people.db). Hvis den placeres i c:\temp er connectionstring som følger:

```
Data Source=c:\temp\people.db;Version=3;
```

og i C# kan du evt benytte følgende kode:

```csharp
private static string databaseFil = "c:\\temp\\people.db";
private static string connectionString = "Data Source=" + databaseFil + ";Version=3;";
```

så kan du benytte ```connectionString``` direkte i koden.

Database og kode kan benyttes frit men smid gerne et link retur til dette repository.