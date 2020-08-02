using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MessageCenter3.Models;

namespace MessageCenter3.DAL.Repositories
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
                        @"INSERT INTO UserGroup (GroupName) 
                            VALUES(@GroupName);
                        SELECT CAST(SCOPE_IDENTITY() as int)";

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
                    @"SELECT * FROM UserGroup 
                        JOIN UserGroupMember member 
                            ON UserGroup.Id = member.GroupId 
                        WHERE member.UserId = @userId";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<UserGroup>(sqlQuery, new { userId }).ToList();
            }
        }

        public List<UserGroup> GetAvailableUserGroups(int userId)
        {
            string sqlQuery =
                    @"SELECT UserGroup.Id AS Id, GroupName FROM UserGroup 
                        JOIN UserGroupMember member 
                            ON UserGroup.Id = member.GroupId 
                        WHERE member.UserId <> @userId
                        GROUP BY UserGroup.Id, GroupName";

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
                    db.Query(
                            @"UPDATE UserGroup 
                                SET GroupName = @GroupName 
                                WHERE Id = @Id", group);
                }
            }

            return group;
        }
                
        public void AddUserToGroup(JoinGroupRequestModel request)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Query(
                        @"INSERT INTO UserGroupMember (UserId, GroupId, IsGroupAdmin)
                            VALUES(@UserId, @GroupId, @IsGroupAdmin)",
                        request);
            }           
        }

        public void ExcludeUserFromGroup(JoinGroupRequestModel request)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Query(
                        @"DELETE FROM UserGroupMember 
                            WHERE UserId = @UserId 
                            AND GroupId = @GroupId",
                        request);
            }
        }

        private IEnumerable<int> GetGroupAdmins(int groupId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<int>(
                    @"SELECT UserId FROM UserGroupMember
                    WHERE GroupId = @groupId
                        AND IsGroupAdmin = 1",
                    new { groupId }
                    );
            }
        }
    }
}
