using System;

namespace MC.DAL.Entities
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