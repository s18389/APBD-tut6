using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.IO;

namespace tutorial6.Services
{
    public class SqlServerDbService : IDbService
    {
        public bool ExistIndexNumber(string index)
        {
            using (var connection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s18389;Integrated Security=True; MultipleActiveResultSets=True;"))
            {
                using (var command = new SqlCommand())
                {
                    command.CommandText = "SELECT COUNT(IndexNumber) AS numberOfStudents FROM Student WHERE IndexNumber = @index;";
                    command.Parameters.AddWithValue("index", index);
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    int numberOfStudents = 0;
                    if (reader.Read())
                    {
                        numberOfStudents = Int32.Parse(reader["numberOfStudents"].ToString());
                    }
                    else
                    {
                        reader.Close();
                        return false;
                    }
                    reader.Close();
                    if (numberOfStudents > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public void SaveLogData(string data)
        {
            string path = @"requestsLog.txt";

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine(data);
                }

            }
            else if (File.Exists(path))
            {
                using (StreamWriter tw = File.AppendText(path))
                {
                    tw.WriteLine(data);
                }
            }

        }
    }
}
