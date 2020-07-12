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
        private const string UserRecipientTable = "MessageUserRecipient";
        private const string GroupRecipientTable = "MessageGroupRecipient";
        private const string UserTableName = "[User]";

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
            string insertTable = GetRecipientTableName(recipientType);

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

        public List<MessageOutputModel> GetMessagesToGroup(int groupId)
        {
            return GetMessagesByRecipient(groupId, RecipientType.Group);
        }

        public List<MessageOutputModel> GetMessagesToUser(int userId)
        {
            return GetMessagesByRecipient(userId, RecipientType.User);
        }

        private List<MessageOutputModel> GetMessagesByRecipient(int recipientId, RecipientType recipientType)
        {
            string recipientTable = GetRecipientTableName(recipientType);

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = string.Format(@"
                        SELECT Name AS AuthorName, SenderId, TypeId, CreateDateAndTime, Text, MsgId AS Id
                        FROM {0} UserTable
                            JOIN (
	                            SELECT Message.Id AS MsgId, SenderId, TypeId, CreateDateAndTime, RecipientId, Text 
                                FROM Message 
	                                JOIN {1} RecipTable
	                                    ON Message.Id = RecipTable.MessageId 
	                                    WHERE RecipTable.RecipientId = @recipientId
	                        ) MessageRecords
	                        ON UserTable.Id = MessageRecords.SenderId
                ", UserTableName, recipientTable);

                List<MessageOutputModel> result = db.Query<MessageOutputModel>(sqlQuery, new { recipientId }).ToList();

                return result;
            }
        }

        private string GetRecipientTableName(RecipientType recipientType)
        {
            string recipientTable = string.Empty;

            switch (recipientType)
            {
                case RecipientType.User:
                    recipientTable = UserRecipientTable;
                    break;
                case RecipientType.Group:
                    recipientTable = GroupRecipientTable;
                    break;
                default:
                    break;
            }

            return recipientTable;
        }
    }
}
