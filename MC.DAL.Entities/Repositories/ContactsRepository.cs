using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MC.DAL.Entities;
using MC.DAL.DataModels.UserGroups;
using MC.DAL.DataModels.Users;
using MC.DAL.Enities;
using MC.DAL.DataModels.Contacts;
using AutoMapper;

namespace MC.DAL.Repositories
{
    class ContactsRepository
    {
        string _connectionString = null;
        private readonly Mapper _mapper;

        public ContactsRepository(string connectionString)
        {
            _connectionString = connectionString;           
        }

        public List<Contact> GetUserContactsForUser(int userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery =
                        @"SELECT * FROM ContactUser
                            WHERE OwnerId = @userId";

                return db.Query<Contact>(sqlQuery, new { userId }).ToList();
            }
        }

        public List<Contact> GetGroupContactsForUser(int userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery =
                        @"SELECT * FROM ContactUserGroup
                            WHERE OwnerId = @userId";

                return db.Query<Contact>(sqlQuery, new { userId }).ToList();
            }
        }

        public int AddContactUser(NewContactRequest request)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery =
                        @"INSERT INTO ContactUser (OwnerId, ContactId) 
                            VALUES(@OwnerId, @ContactId);
                        SELECT CAST(SCOPE_IDENTITY() as int)";                

                return db.Query<int>(sqlQuery, request).First();
            }
        }

        public int AddContactGroup(NewContactRequest request)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery =
                        @"INSERT INTO ContactUserGroup (OwnerId, ContactId) 
                            VALUES(@OwnerId, @ContactId);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

                return db.Query<int>(sqlQuery, request).First();
            }
        }

        public void RemoveContactUser(int contactId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery =
                        @"DELETE FROM ContactUser
                            WHERE Id = @contactId";

                db.Query<int>(sqlQuery, new { contactId });
            }
        }

        public void RemoveContactGroup(int contactId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery =
                        @"DELETE FROM ContactUserGroup 
                            WHERE Id = @contactId";

                db.Query<int>(sqlQuery, new { contactId });
            }
        }
    }
}
