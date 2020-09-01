using System;
using System.Data.SQLite;
using SQLiteEF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Collections.Generic;

namespace undervisning_db_sqlite
{
    class Program
    {

        static void Main(string[] args)
        {

            var path = @"C:\tmp\undervisning-db-sqlite\undervisning-db-sqlite\Data\people.db";

            BrugAfDapper(path);
            BrugAfEF(path);

          


        }

        private static void BrugAfEF(string path)
        {
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
        }

        private static void BrugAfDapper(string path)
        {
            using (var cn = new SQLiteConnection("Data Source=" + path))
            {
                cn.Open();
                List<Person> p = cn.Query<Person>("select * from person").ToList();
                p.ToList().ForEach(i => Console.WriteLine(i.FirstName + " " + i.LastName));
            }

        }
    }
}



