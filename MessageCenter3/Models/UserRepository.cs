using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter.Models
{
    public class UserRepository : IUserRepository
    {
        string _connectionString = null;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<User> GetUsers()
        {   
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<User>("SELECT * FROM Users").ToList();
            }
        }

        public User Get(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<User>("SELECT * FROM Users WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public User GetUserByNameAndPassword(string name, string password)
        {
            var sqlQuery = "SELECT * FROM Users WHERE Name = @name AND Password = @password";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<User>(sqlQuery, new { name, password }).FirstOrDefault();
            }
        }

        public User Create(User user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {   
                var sqlQuery = "INSERT INTO Users (Name, Password, Role) VALUES(@Name, @Password, @Role); SELECT CAST(SCOPE_IDENTITY() as int)";
                int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                user.Id = userId.Value;

                return user;
            }
        }

        public User Update(User user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Users SET Name = @Name, Password = @Password, Role = @Role WHERE Id = @Id";
                db.Execute(sqlQuery, user);

                return Get(user.Id);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Users WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
