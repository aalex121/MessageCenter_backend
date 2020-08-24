CREATE TABLE [dbo].[ContactUserGroup]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[OwnerId] INT NOT NULL,
	[ContactId] INT NOT NULL,
	CONSTRAINT [FK_ContactGroup_ToUser] FOREIGN KEY ([OwnerId]) REFERENCES [User]([Id]),
	CONSTRAINT [FK_ContactGroup_ToUserGroup] FOREIGN KEY ([ContactId]) REFERENCES [UserGroup]([Id])
)