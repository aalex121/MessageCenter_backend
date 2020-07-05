using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Models
{
    public class UserGroupMemberRecord
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}
