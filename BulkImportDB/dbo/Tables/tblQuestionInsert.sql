CREATE TABLE [dbo].[tblQuestionInsert] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [Question]     VARCHAR (MAX) NOT NULL,
    [QuestionType] VARCHAR (MAX) NULL,
    [CreatedOn]    DATETIME      CONSTRAINT [DF_tblQuestionInsert_CreatedOn] DEFAULT (getutcdate()) NULL,
    [CreatedBy]    VARCHAR (50)  NULL
);

