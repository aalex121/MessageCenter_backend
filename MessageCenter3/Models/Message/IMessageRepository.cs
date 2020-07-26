using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageCenter3.Enums;

namespace MessageCenter3.Models
{
    public interface IMessageRepository
    {
        List<MessageOutputModel> GetDialogue(UserMessageRequestModel request);

        List<MessageOutputModel> GetGroupMessages(GroupMessageRequestModel request);

        List<MessageOutputModel> GetMessagesByRecipient(int userId, RecipientType recipientType);

        int AddMessage(MessageInputModel message);
    }
}
