using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageCenter3.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TypeId { get; set; }
        public DateTime CreateDateAndTime { get; set; }
        public string Text { get; set; }
    }
}
