using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageCenter3.Enums;

namespace MessageCenter3.Models
{
    public interface IMessageRepository
    {
        List<Message> GetMessagesByAuthor(int userId);

        List<MessageOutputModel> GetMessagesToUser(int userId);

        List<MessageOutputModel> GetMessagesToGroup(int groupId);

        void AddMessage(MessageInputModel message);
    }
}
