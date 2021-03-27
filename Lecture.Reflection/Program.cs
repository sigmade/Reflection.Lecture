using Npgsql;

using System;
using System.Data.Common;

namespace Lecture.Reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            using DbConnection connection = NpgsqlFactory.Instance.CreateConnection();

            connection.ConnectionString = "host=localhost;port=5432;database=lecture_reflection_db;username=postgres;password=postgres";

            connection.Open();

            // connection.Query<User>("select * from Users where id = @id", new { id = 15 })
        }
    }
}
