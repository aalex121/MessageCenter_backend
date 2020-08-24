using MC.DAL.Entities;

namespace MC.DAL.DataModels.Messages
{
    public class MessageOutputModel : Message
    {
        public int RecipientId { get; set; }
        public string SenderName { get; set; }
    }
}
