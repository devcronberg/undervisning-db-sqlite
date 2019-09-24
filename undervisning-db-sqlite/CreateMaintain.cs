using SQLiteEF;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace undervisning_db_sqlite
{
    class CreateMaintain
    {
        public static void CreateDb()
        {
            SQLiteConnection.CreateFile("");
            using (SQLiteConnection cn = new SQLiteConnection(""))
            {
                cn.Open();
                using (SQLiteCommand cm = new SQLiteCommand(cn))
                {
                    cm.CommandText = "create table person (PersonId INTEGER PRIMARY KEY AUTOINCREMENT, FirstName VARCHAR(50), LastName VARCHAR(50), Height INTEGER, IsHealthy BOOL, DateOfBirth DATE, Gender INTEGER)";
                    cm.CommandType = System.Data.CommandType.Text;
                    cm.ExecuteNonQuery();
                }

                using (SQLiteCommand cm = new SQLiteCommand(cn))
                {

                    foreach (var person in MCronberg.PersonRepositoryStatic.JustGetPeople(200))
                    {
                        string sql = "insert into person (FirstName, LastName, Height, IsHealthy, DateOfBirth, Gender) ";
                        sql += "values('" + person.FirstName + "', ";
                        sql += "'" + person.LastName + "', ";
                        sql += "" + person.Height + ", ";
                        sql += "" + person.IsHealthy + ", ";
                        sql += "'" + person.DateOfBirth.ToString("yyyy-MM-dd") + "',";
                        sql += "" + Convert.ToInt32(person.Gender) + ")";
                        cm.CommandText = sql;
                        cm.CommandType = System.Data.CommandType.Text;
                        cm.ExecuteNonQuery();
                    }
                }
            }

        }

        public static void SetCountryId(string path) {
            using (PeopleContext c = new PeopleContext(path))
            {
                var people = c.People;
                for (int i = 1; i < people.Count()+1; i++)
                {
                    people.Find(i).CountryId = 1;
                }

                for (int i = 1; i < 75; i++)
                {
                    int r = new Random().Next(1, 201);
                    people.Find(r).CountryId = 2;
                }

                for (int i = 1; i < 75; i++)
                {
                    int r = new Random().Next(1, 201);
                    people.Find(r).CountryId = 3;
                }

                for (int i = 1; i < 75; i++)
                {
                    int r = new Random().Next(1, 201);
                    people.Find(r).CountryId = 4;
                }


                c.SaveChanges();
            }
        }

    }
}
