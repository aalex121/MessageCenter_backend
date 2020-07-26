using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Models
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
