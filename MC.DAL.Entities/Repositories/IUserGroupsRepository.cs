using MC.DAL.DataModels.UserGroups;
using MC.DAL.DataModels.Users;
using MC.DAL.Entities;
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
        JoinAttemptResults AddUserToGroup(JoinGroupRequestModel request);
        void ExcludeUserFromGroup(JoinGroupRequestModel request);
        bool DeleteUserGroup(int groupId);
        List<UserData> GetGroupMemebers(int groupId);
        List<UserData> GetGroupAdmins(int groupId);
    }
}
