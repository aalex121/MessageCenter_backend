CREATE TABLE [dbo].[ContactUser]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[OwnerId] INT NOT NULL,
	[ContactId] INT NOT NULL,
	CONSTRAINT [FK_ContactUser_ToUser] FOREIGN KEY ([OwnerId]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_ContactUserContact_ToUser] FOREIGN KEY ([ContactId]) REFERENCES [User]([Id])
)
