using System.Collections.Generic;
using MessageCenter3.Enums;
using MessageCenter3.Models;

namespace MessageCenter3.DAL.Repositories
{
    public interface IMessageRepository
    {
        List<MessageOutputModel> GetDialogue(UserMessageRequestModel request);

        List<MessageOutputModel> GetGroupMessages(GroupMessageRequestModel request);

        List<MessageOutputModel> GetMessagesByRecipient(int userId, RecipientType recipientType);

        int AddMessage(MessageInputModel message);
    }
}
