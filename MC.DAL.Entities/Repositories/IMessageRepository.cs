using MC.DAL.DataModels.Messages;
using MC.DAL.Enums;
using System.Collections.Generic;

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
