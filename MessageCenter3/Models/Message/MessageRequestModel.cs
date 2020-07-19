using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Models
{
    public class MessageRequestModel
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public int MessageType { get; set; }
    }
}
