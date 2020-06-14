using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter.Models
{
    public interface IUserRepository
    {
        User Create(User user);
        void Delete(int id);
        User Get(int id);
        List<User> GetUsers();
        User Update(User user);
        User GetUserByNameAndPassword(string name, string password);
    }
}
