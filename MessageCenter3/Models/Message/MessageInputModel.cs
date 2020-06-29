using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Models
{
    public class MessageInputModel : Message
    {   
        public int RecipientId { get; set; }
        public int RecipientType { get; set; }
        public bool IsDraft { get; set; }
    }
}
