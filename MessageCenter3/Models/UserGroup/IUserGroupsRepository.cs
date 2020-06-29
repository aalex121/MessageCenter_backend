using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Models
{
    public interface IUserGroupsRepository
    {
        List<UserGroup> GetAllUserGroups();
        List<UserGroup> GetUserGroupsByUser(int userId);
        UserGroup GetUserGroupById(int groupId);
        List<UserGroup> GetUserGroupsByName(string searchName);
        UserGroup AddUserGroup(UserGroup newGroup);
        UserGroup RenameUserGroup(UserGroup group);
        bool DeleteUserGroup(int groupId);
    }
}
