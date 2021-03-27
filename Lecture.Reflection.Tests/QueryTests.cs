using Npgsql;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Lecture.Reflection.Tests
{

    public class QueryTests : IDisposable
    {
        private readonly DbConnection _connection;

        public QueryTests()
        {
            _connection = NpgsqlFactory.Instance.CreateConnection();

            _connection.ConnectionString = "host=localhost;port=5432;database=lecture_reflection_db;username=postgres;password=postgres";

            _connection.Open();
        }

        [Fact]
        public void Query_Returns_Results()
        {
            var users = _connection.Query<User>($"select * from users");

            Assert.NotNull(users);

            Assert.Equal(1, users[0].Id);
            Assert.Equal("test1", users[0].Username);
            Assert.Equal("test!", users[0].Password);
        }

        [Fact]
        public void Query_Returns_Results_With_Filter()
        {
            string name = "test2";

            var user = _connection.Query<User>("select * from users where username = @Name", new { Name = name })[0];

            Assert.Equal(2, user.Id);
            Assert.Equal("test2", user.Username);
            Assert.Equal("test!", user.Password);
            Assert.Equal("A1", user.FirstName);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
