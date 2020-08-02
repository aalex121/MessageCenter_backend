using MessageCenter3.Models;
using System.Collections.Generic;

namespace MessageCenter3.DAL.Repositories
{
    public interface IUserGroupsRepository
    {
        List<UserGroup> GetAllUserGroups();
        List<UserGroup> GetUserGroupsByUserId(int userId);        
        List<UserGroup> GetAvailableUserGroups(int userId);
        UserGroup GetUserGroupById(int groupId);
        List<UserGroup> GetUserGroupsByName(string searchName);
        UserGroup AddUserGroup(UserGroupInputModel newGroup);
        UserGroup RenameUserGroup(UserGroup group);
        void AddUserToGroup(JoinGroupRequestModel request);
        void ExcludeUserFromGroup(JoinGroupRequestModel request);
        bool DeleteUserGroup(int groupId);
    }
}
