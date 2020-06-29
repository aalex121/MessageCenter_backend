CREATE TABLE [dbo].[MessageGroupRecipient]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MessageId] INT NOT NULL, 
    [RecipientId] INT NOT NULL, 
    CONSTRAINT [FK_MessageGroupRecipient_ToUserGroup] FOREIGN KEY ([RecipientId]) REFERENCES [UserGroup]([Id]), 
    CONSTRAINT [FK_MessageGroupRecipient_ToMessage] FOREIGN KEY ([MessageId]) REFERENCES [Message]([Id])
)
