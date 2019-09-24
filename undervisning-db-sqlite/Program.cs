using System;
using System.Data.SQLite;
using SQLiteEF;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace undervisning_db_sqlite
{
    class Program
    {
        public static string databaseFil = @"C:\git\undervisning-db-sqlite\db-download\people.db";
        public static string connectionString = "Data Source=" + databaseFil;

        static void Main(string[] args)
        {
            using (PeopleContext c = new PeopleContext())
            {
                var country = c.Countries.Include(i => i.People).FirstOrDefault();
                
            }
           

        
            
        }

        private static void CreateDb()
        {
            SQLiteConnection.CreateFile(databaseFil);
            using (SQLiteConnection cn = new SQLiteConnection(connectionString))
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
    }
}



