CREATE TABLE [dbo].[tblEmployees] (
    [FirstName]  VARCHAR (30)  NULL,
    [MiddleName] VARCHAR (30)  NULL,
    [LastName]   VARCHAR (30)  NULL,
    [Age]        INT           NULL,
    [DOB]        VARCHAR (30)  NULL,
    [EmailID]    VARCHAR (50)  NULL,
    [Aderess]    VARCHAR (100) NULL,
    [CreatedBy]  VARCHAR (50)  NULL,
    [CreatedOn]  DATETIME      CONSTRAINT [DF_tblEmployees_CreatedOn] DEFAULT (getutcdate()) NULL,
    [UpdatedBy]  VARCHAR (50)  NULL,
    [UpdateOn]   DATETIME      NULL,
    [DeletedBy]  VARCHAR (50)  NULL,
    [DeletedOn]  DATETIME      NULL,
    [IsDeleted]  BIT           CONSTRAINT [DF_tblEmployees_IsDeleted] DEFAULT ((0)) NULL,
    [RoleID]     VARCHAR (20)  NULL,
    [Gender]     VARCHAR (20)  NULL,
    [Skill]      VARCHAR (50)  NULL
);

