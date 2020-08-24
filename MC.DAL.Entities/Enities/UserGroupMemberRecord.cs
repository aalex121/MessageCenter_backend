using System;
using System.Collections.Generic;
using System.Text;

namespace MC.DAL.Entities
{
    class UserGroupMemberRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}
