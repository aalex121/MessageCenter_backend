using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageCenter3.Enums;

namespace MessageCenter3.Models
{
    public interface IMessageRepository
    {
        List<MessageOutputModel> GetMessagesFromTo(int senderId, int recipientId, RecipientType recipientType);

        List<MessageOutputModel> GetMessagesByRecipient(int userId, RecipientType recipientType);

        List<MessageOutputModel> GetDialogue(int collocutor1, int collocutor2);

        int AddMessage(MessageInputModel message);
    }
}
