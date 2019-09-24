using System;
using System.Data.SQLite;
using SQLiteEF;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace undervisning_db_sqlite
{
    class Program
    {

        static void Main(string[] args)
        {

            var path = @"C:\temp\undervisning-db-sqlite\undervisning-db-sqlite\Data\people.db";

// Get people
using (PeopleContext c = new PeopleContext(path))
{
    var res = c.People.Where(i => i.Height > 170 && i.IsHealthy).OrderBy(i => i.LastName).ToList();
    res.ForEach(i => Console.WriteLine(i));
}

// Get countries with navigation property
using (PeopleContext c = new PeopleContext(path))
{
    var res = c.Countries.Include(i => i.People).ToList();
    res.ForEach(i =>
    {
        Console.WriteLine(i.Name);
        i.People.ForEach(x => Console.WriteLine("\t" + x));
    });
}

// Get countries with join
using (PeopleContext c = new PeopleContext(path))
{
    var res = from country in c.Countries orderby country.CountryId join person in c.People on country.CountryId equals person.CountryId select new { person.FirstName, person.LastName, country.Name };
    res.ToList().ForEach(i => Console.WriteLine(i.FirstName + " " + i.LastName + " from " + i.Name));
}


        }

    }
}



