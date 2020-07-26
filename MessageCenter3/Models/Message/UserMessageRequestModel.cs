using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Models
{
    public class UserMessageRequestModel
    {
        public int CurrentUserId { get; set; }
        public int CollocutorId { get; set; }
        public int MessageType { get; set; }
        public int Offset { get; set; }
    }
}
