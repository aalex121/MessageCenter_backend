using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using MessageCenter3.Enums;

namespace MessageCenter3.Models
{
    public class MessageRepository : IMessageRepository
    {
        string _connectionString = null;

        public MessageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddMessage(MessageInputModel message)
        {
            int recipientId = message.RecipientId;
            RecipientType recipientType = (RecipientType)message.RecipientType;
            bool isDraft = message.IsDraft;
            Message newMessage = message;
            newMessage.CreateDateAndTime = DateTime.Now;

            int messageId = AddNewMessage(newMessage, isDraft);
            AddNewMessageReipient(messageId, recipientId, recipientType);
        }

        private int AddNewMessage(Message message, bool isDraft)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string messageInsertQuery =
                        "INSERT INTO Message (SenderId, TypeId, CreateDateAndTime, Text) " +
                        "VALUES(@SenderId, @TypeId, @CreateDateAndTime, @Text);" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

                int messageId = db.Query<int>(messageInsertQuery, message).First();

                if (!isDraft)
                {
                    return messageId;
                }

                var draftInsertValues = new { MessageId = message.Id, IsSent = 0 };

                string draftInsetQuery =
                        "INSERT INTO MessageDraft (MessageId, IsSent) " +
                        "VALUES(@MessageId, @IsSent)";

                db.Query(draftInsetQuery, draftInsertValues);

                return messageId;
            }
        }

        private void AddNewMessageReipient(int messageId, int recipientId, RecipientType recipientType)
        {
            string insertTable = string.Empty;

            switch (recipientType)
            {
                case RecipientType.User:
                    insertTable = "MessageUserRecipient";
                    break;
                case RecipientType.Group:
                    insertTable = "MessageGroupRecipient";
                    break;
                default:
                    break;
            }

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string insertQuery = string.Format("INSERT INTO {0} (MessageId, RecipientId) VALUES(@MessageId, @RecipientId)",
                    insertTable);

                var insertValues = new { MessageId = messageId, RecipientId = recipientId };

                db.Query(insertQuery, insertValues);
            }   
        }

        public List<Message> GetMessagesByAuthor(int userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Message>("SELECT * FROM Message WHERE SenderId = @userId", new { userId }).ToList();
            }
        }

        public List<Message> GetMessagesToGroup(int groupId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Message>(
                    "SELECT * FROM Message Msg" +
                    "JOIN MessageGroupRecipient Mgr" +
                    "ON Msg.Id = Mgr.MessageId" +
                    "WHERE Mgr.RecipientId = @groupId", new { groupId }).ToList();
            }
        }

        public List<Message> GetMessagesToUser(int userId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return db.Query<Message>(
                    "SELECT * FROM Message Msg" +
                    "JOIN MessageUserRecipient Mur" +
                    "ON Msg.Id = Mur.MessageId" +
                    "WHERE Mur.RecipientId = @userId", new { userId }).ToList();
            }
        }
    }
}
