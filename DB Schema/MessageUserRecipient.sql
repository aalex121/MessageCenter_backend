CREATE TABLE [dbo].[MessageUserRecipient]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MessageId] INT NOT NULL, 
    [RecipientId] INT NOT NULL, 
    CONSTRAINT [FK_MessageUserRecipient_ToUser] FOREIGN KEY ([RecipientId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_MessageUserRecipient_Message] FOREIGN KEY ([MessageId]) REFERENCES [Message]([Id])
)
