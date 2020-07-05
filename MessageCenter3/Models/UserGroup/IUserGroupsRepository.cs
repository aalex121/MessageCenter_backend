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
        UserGroup GetUserGroupById(int groupId);
        List<UserGroup> GetUserGroupsByName(string searchName);
        UserGroup AddUserGroup(UserGroupInputModel newGroup);
        UserGroup RenameUserGroup(UserGroup group);
        bool AddUserToGroup(int userId, int groupId);
        void ExcludeUserFromGroup(int userId, int groupId);
        bool DeleteUserGroup(int groupId);
    }
}
