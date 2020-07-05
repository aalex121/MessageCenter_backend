using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter.Models
{
    public class UserData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string RoleId { get; set; }
    }
}
