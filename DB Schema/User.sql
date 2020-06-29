CREATE TABLE [dbo].[User] (
    [Id]       INT     IDENTITY       NOT NULL,
    [Name]     NVARCHAR (50)  NOT NULL,
    [Password] NVARCHAR (MAX) NOT NULL,
    [RoleId]     INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_User_ToUserRole] FOREIGN KEY ([RoleId]) REFERENCES [UserRole]([Id])
);
