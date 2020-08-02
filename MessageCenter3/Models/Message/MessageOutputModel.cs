using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Models
{
    public class MessageOutputModel : Message
    {
        public int RecipientId { get; set; }
        public string SenderName { get; set; }
    }
}
