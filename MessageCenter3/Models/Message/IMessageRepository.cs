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

        List<Message> GetMessagesToUser(int userId);

        List<Message> GetMessagesToGroup(int groupId);

        void AddMessage(MessageInputModel message);
    }
}
