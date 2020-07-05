using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter.Models
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
