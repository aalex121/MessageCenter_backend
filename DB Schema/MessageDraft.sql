CREATE TABLE [dbo].[MessageDraft]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MessageId] INT NOT NULL, 
    [IsSent] BIT NOT NULL, 
    CONSTRAINT [FK_MessageDraft_ToUser] FOREIGN KEY ([MessageId]) REFERENCES [Message]([id])    
)
