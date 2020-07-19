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

        public int AddMessage(MessageInputModel message)
        {
            int recipientId = message.RecipientId;
            RecipientType recipientType = (RecipientType)message.RecipientType;
            bool isDraft = message.IsDraft;
            Message newMessage = message;
            newMessage.CreateDateAndTime = DateTime.Now;

            int messageId = AddNewMessage(newMessage, isDraft);
            AddNewMessageReipient(messageId, recipientId, recipientType);

            return messageId;
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
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = string.Empty;

                switch (recipientType)
                {
                    case RecipientType.User:
                        sqlQuery = @"INSERT INTO MessageUserRecipient (MessageId, RecipientId) VALUES(@MessageId, @RecipientId)";
                        break;
                    case RecipientType.Group:
                        sqlQuery = @"INSERT INTO MessageGroupRecipient (MessageId, RecipientId) VALUES(@MessageId, @RecipientId)";
                        break;
                    default:
                        break;
                }

                var insertValues = new { MessageId = messageId, RecipientId = recipientId };

                db.Query(sqlQuery, insertValues);
            }   
        }

        public List<MessageOutputModel> GetMessagesFromTo(int senderId, int recipientId, RecipientType recipientType)
        {
            string sqlQuery = string.Empty;

            switch (recipientType)
            {
                case RecipientType.User:
                    sqlQuery = @"
                        SELECT [User].Id AS CollocutorId, [User].Name AS CollocutorName, 
                            SenderId, TypeId, CreateDateAndTime, Text, Message.Id AS Id 
                            FROM Message 
	                            JOIN MessageUserRecipient RecipTable
	                                ON Message.Id = RecipTable.MessageId
                                JOIN [User]
                                    ON RecipTable.RecipientId = [User].Id
                                WHERE Message.SenderId = @senderId
                                AND RecipTable.RecipientId = @recipientId";
                    break;
                case RecipientType.Group:
                    sqlQuery = @"
                        SELECT [User].Id AS CollocutorId, [User].Name AS CollocutorName, 
                            SenderId, TypeId, CreateDateAndTime, Text, Message.Id AS Id 
                            FROM Message 
	                            JOIN MessageGroupRecipient RecipTable
	                                ON Message.Id = RecipTable.MessageId
                                JOIN [User]
                                    ON RecipTable.RecipientId = [User].Id
                                WHERE Message.SenderId = @senderId
                                AND RecipTable.RecipientId = @recipientId";
                    break;
                default:
                    break;
            }

            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentException("Invalid Recipient Type!");
            }

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<MessageOutputModel> output = db.Query<MessageOutputModel>(sqlQuery,
                        new { senderId, recipientId }).ToList();

                return output;
            }
        }

        public List<MessageOutputModel> GetMessagesByRecipient(int recipientId, RecipientType recipientType)
        {
            string sqlQuery = string.Empty;

            switch (recipientType)
            {
                case RecipientType.User:
                    sqlQuery = @"
                        SELECT [User].Id AS CollocutorId, [User].Name AS CollocutorName, 
                            SenderId, TypeId, CreateDateAndTime, Text, Message.Id AS Id 
                            FROM Message 
	                            JOIN MessageUserRecipient RecipTable
	                                ON Message.Id = RecipTable.MessageId
                                JOIN [User]
                                    ON Message.SenderId = [User].Id
                                WHERE RecipTable.RecipientId = @recipientId";
                    break;
                case RecipientType.Group:
                    sqlQuery = @"
                        [User].Id AS CollocutorId, [User].Name AS CollocutorName, 
                            SenderId, TypeId, CreateDateAndTime, Text, Message.Id AS Id 
                            FROM Message 
	                            JOIN MessageGroupRecipient RecipTable
	                                ON Message.Id = RecipTable.MessageId
                                JOIN [User]
                                    ON Message.SenderId = [User].Id
                                WHERE RecipTable.RecipientId = @recipientId";
                    break;
                default:
                    break;
            }

            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentException("Invalid Recipient Type!");
            }

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<MessageOutputModel> output = db.Query<MessageOutputModel>(sqlQuery,
                        new { recipientId }).ToList();

                return output;
            }
        }

        public List<MessageOutputModel> GetDialogue(int collocutor1, int collocutor2)
        {
            string sqlQuery = @"
                        SELECT [User].Id AS CollocutorId, [User].Name AS CollocutorName, 
                            SenderId, TypeId, CreateDateAndTime, Text, Message.Id AS Id 
                            FROM Message 
	                            JOIN MessageUserRecipient RecipTable
	                                ON Message.Id = RecipTable.MessageId
                                JOIN [User]
                                    ON RecipTable.RecipientId = [User].Id                                
                                WHERE RecipTable.RecipientId IN (@collocutor1, @collocutor2)";

            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                throw new ArgumentException("Invalid Recipient Type!");
            }

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<MessageOutputModel> output = db.Query<MessageOutputModel>(sqlQuery,
                        new { collocutor1, collocutor2 }).ToList();

                return output;
            }
        }
    }
}
