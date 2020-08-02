CREATE TABLE [dbo].[UserGroupMember]
(
	[Id] INT NOT NULL IDENTITY, 
    [UserId] INT NOT NULL, 
    [GroupId] INT NOT NULL, 
    [IsGroupAdmin] BIT NOT NULL, 
    PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_UserGroupMember_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_UserGroupMember_ToUserGroup] FOREIGN KEY ([GroupId]) REFERENCES [UserGroup]([Id]) 
)
