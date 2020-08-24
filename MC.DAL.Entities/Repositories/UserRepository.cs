using Dapper;
using MC.DAL.DataModels.Users;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MessageCenter3.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public const string UserDbName = "[User]";
        string _connectionString = null;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<UserData> GetAllUsers()
        {   
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<UserData>(string.Format("SELECT * FROM {0}", UserDbName)).ToList();
            }
        }

        public UserData GetUserById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = string.Format("SELECT * FROM {0} WHERE Id = @id", UserDbName);
                return db.Query<UserData>(sqlQuery, new { id }).FirstOrDefault();
            }
        }

        public UserData GetUserByName(string name)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = string.Format("SELECT * FROM {0} WHERE Name = @name", UserDbName);
                return db.Query<UserData>(sqlQuery, new { name }).FirstOrDefault();
            }
        }

        public UserData GetUserByNameAndPassword(string name, string password)
        {
            string sqlQuery = string.Format("SELECT * FROM {0} WHERE Name = @name AND Password = @password",
                    UserDbName);

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<UserData>(sqlQuery, new { name, password }).FirstOrDefault();
            }
        }

        public UserData CreateUser(UserData user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {   
                string sqlQuery = string.Format("INSERT INTO {0} " +
                        "(Name, Password, RoleId) " +
                        "VALUES(@Name, @Password, @RoleId); " +
                        "SELECT CAST(SCOPE_IDENTITY() as int)", UserDbName);

                int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                user.Id = userId.Value;

                return user;
            }
        }

        public UserData UpdateUserData(UserData user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = string.Format("UPDATE {0} " +
                        "SET Name = @Name, Password = @Password, RoleId = @RoleId " +
                        "WHERE Id = @Id", UserDbName);

                db.Execute(sqlQuery, user);

                return GetUserById(user.Id);
            }
        }

        public void DeleteUser(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = string.Format("DELETE FROM {0} WHERE Id = @id", UserDbName);
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
