using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MC.DAL.Entities;
using MC.DAL.DataModels.Messages;
using MC.DAL.Enums;
using Microsoft.Data.SqlClient;

namespace MessageCenter3.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
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

        public List<MessageOutputModel> GetDialogue(UserMessageRequestModel request)
        {
            string sqlQuery = sqlQuery = @"
                        SELECT RecipTable.RecipientId, [User].Name AS SenderName, 
                                SenderId, TypeId, CreateDateAndTime, Text, Message.Id AS Id 
                            FROM Message 
	                            JOIN MessageUserRecipient RecipTable
	                                ON Message.Id = RecipTable.MessageId
                                JOIN [User]
                                    ON Message.SenderId = [User].Id
                                WHERE Message.SenderId IN (@CurrentUserId, @CollocutorId)
                                AND RecipTable.RecipientId IN (@CurrentUserId, @CollocutorId)
                                AND Message.TypeId = @MessageType
                        ORDER BY Message.CreateDateAndTime
                        OFFSET @Offset Rows";            

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<MessageOutputModel> output = db.Query<MessageOutputModel>(sqlQuery, request).ToList();

                return output;
            }
        }

        public List<MessageOutputModel> GetGroupMessages(GroupMessageRequestModel request)
        {
            string sqlQuery = @"
                        SELECT RecipTable.RecipientId, [User].Name AS SenderName, 
                                SenderId, TypeId, CreateDateAndTime, Text, Message.Id AS Id 
                            FROM Message 
	                            JOIN MessageGroupRecipient RecipTable
	                                ON Message.Id = RecipTable.MessageId
                                JOIN [User]
                                    ON RecipTable.RecipientId = [User].Id
                                WHERE RecipTable.RecipientId = @GroupId
                                AND Message.TypeId = @MessageType
                        ORDER BY Message.CreateDateAndTime
                        OFFSET @Offset Rows";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<MessageOutputModel> output = db.Query<MessageOutputModel>(sqlQuery, request).ToList();

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
                        SELECT RecipTable.RecipientId, [User].Name AS SenderName, 
                                SenderId, TypeId, CreateDateAndTime, Text, Message.Id AS Id
                            FROM Message 
	                            JOIN MessageUserRecipient RecipTable
	                                ON Message.Id = RecipTable.MessageId
                                JOIN [User]
                                    ON RecipTable.RecipientId = [User].Id
                                WHERE RecipTable.RecipientId = @recipientId";
                    break;
                case RecipientType.Group:
                    sqlQuery = @"
                        SELECT RecipTable.RecipientId, [User].Name AS SenderName, 
                                SenderId, TypeId, CreateDateAndTime, Text, Message.Id AS Id 
                            FROM Message 
	                            JOIN MessageGroupRecipient RecipTable
	                                ON Message.Id = RecipTable.MessageId
                                JOIN [User]
                                    ON RecipTable.RecipientId = [User].Id
                                WHERE RecipTable.RecipientId = @recipientId";
                    break;
                default:
                    break;
            }

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                List<MessageOutputModel> output = db.Query<MessageOutputModel>(sqlQuery,
                        new { recipientId }).ToList();

                return output;
            }
        }
    }
}
