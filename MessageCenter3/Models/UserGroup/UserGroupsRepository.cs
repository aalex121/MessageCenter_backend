﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Data;

namespace MessageCenter3.Models
{
    public class UserGroupsRepository : IUserGroupsRepository
    {
        string _connectionString = null;

        public UserGroupsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public UserGroup AddUserGroup(UserGroupInputModel newGroup)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                UserGroup newGroupData = newGroup;

                string groupInsertQuery =
                        "INSERT INTO UserGroup (GroupName) " +
                        "VALUES(@GroupName);" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

                newGroup.Id = db.Query<int>(groupInsertQuery, newGroupData).First();

                return newGroup;
            }
        }

        public bool DeleteUserGroup(int groupId)
        {
            bool fGroupExists = GetUserGroupById(groupId) != null;

            if (fGroupExists)
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var id = new { id = groupId };
                    db.Query("DELETE FROM UserGroupMember WHERE GroupId = @id", id);
                    db.Query("DELETE FROM UserGroup WHERE Id = @id", id);
                }
            }

            return fGroupExists;
        }

        public List<UserGroup> GetAllUserGroups()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<UserGroup>("SELECT * FROM UserGroup").ToList();
            }
        }

        public UserGroup GetUserGroupById(int groupId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<UserGroup>("SELECT * FROM UserGroup WHERE Id = @id", new { id = groupId }).FirstOrDefault();
            }
        }

        public List<UserGroup> GetUserGroupsByName(string searchName)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<UserGroup>("SELECT * FROM UserGroup WHERE GroupName LIKE %@GroupName%", 
                        new { GroupName = searchName }).ToList();
            }
        }

        public List<UserGroup> GetUserGroupsByUserId(int userId)
        {
            string sqlQuery =
                    "SELECT * FROM UserGroup group " +
                    "JOIN UserGroupMember member " +
                    "ON group.Id = member.GroupId " +
                    "WHERE member.UserId = @userId ";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<UserGroup>(sqlQuery, new { userId }).ToList();
            }
        }

        public UserGroup RenameUserGroup(UserGroup group)
        {
            if (GetUserGroupById(group.Id) != null)
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    db.Query("UPDATE UserGroup SET GroupName = @GroupName WHERE Id = @Id", group);
                }
            }

            return group;
        }
                
        public bool AddUserToGroup(int userId, int groupId)
        {
            if (GetUserGroupById(groupId) == null)
            {
                return false;
            }

            using (IDbConnection db = new SqlConnection(_connectionString))
            {                
                UserGroupMemberRecord userRecord = db.Query<UserGroupMemberRecord>(
                        @"SELECT * FROM UserGroupMember 
                        WHERE UserId = @userId 
                        AND GroupId = @groupId",
                        new { userId, groupId }).FirstOrDefault();

                if (userRecord != null)
                {
                    return false;
                }

                userRecord.UserId = userId;
                userRecord.GroupId = groupId;

                db.Query("INSERT INTO UserGroupMember (UserId, GroupId) " +
                        "VALUES(@userId, @groupId)", userRecord);
            }

            return true;
        }

        public void ExcludeUserFromGroup(int userId, int groupId)
        {
            UserGroupMemberRecord userRecord = new UserGroupMemberRecord(){ UserId = userId, GroupId = groupId };

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Query(@"DELETE FROM UserGroupMember 
                        WHERE UserId = @userId 
                        AND GroupId = @groupId",
                        userRecord);
            }
        }
    }
}
