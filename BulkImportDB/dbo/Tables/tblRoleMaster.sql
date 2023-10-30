CREATE TABLE [dbo].[tblRoleMaster] (
    [RoleID]   VARCHAR (50) CONSTRAINT [DF_tblRoleMaster_RoleID] DEFAULT (newid()) NOT NULL,
    [RoleName] VARCHAR (50) NOT NULL
);

