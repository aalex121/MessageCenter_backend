using MC.DAL.DataModels.Users;
using System.Collections.Generic;

namespace MessageCenter3.DAL.Repositories
{
    public interface IUserRepository
    {
        UserData CreateUser(UserData user);
        void DeleteUser(int id);
        UserData GetUserById(int id);
        UserData GetUserByName(string name);
        List<UserData> GetAllUsers();
        UserData UpdateUserData(UserData user);
        UserData GetUserByNameAndPassword(string name, string password);
    }
}
