CREATE TABLE [dbo].[Message]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SenderId] INT NOT NULL, 
    [TypeId] INT NOT NULL, 
    [CreateDateAndTime] DATETIME NOT NULL, 
    [Text] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Message_ToUser] FOREIGN KEY ([SenderId]) REFERENCES [User]([Id]),
    CONSTRAINT [FK_Message_ToMessageType] FOREIGN KEY ([TypeId]) REFERENCES [MessageType]([Id])
)
