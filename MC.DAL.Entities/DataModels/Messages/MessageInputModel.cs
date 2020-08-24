using MC.DAL.Entities;

namespace MC.DAL.DataModels.Messages
{
    public class MessageInputModel : Message
    {   
        public int RecipientId { get; set; }
        public int RecipientType { get; set; }
        public bool IsDraft { get; set; }
    }
}
