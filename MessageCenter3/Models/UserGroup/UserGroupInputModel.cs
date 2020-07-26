using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Models
{
    public class UserGroupInputModel : UserGroup
    {
        public int CreatorId { get; set; }
        public string GroupName { get; set; }
    }
}
