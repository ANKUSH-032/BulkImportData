-- uspQuestionList 's'

CREATE PROCEDURE [dbo].[uspQuestionList]
(
@QuestionType VARCHAR(MAX)
)
AS
SET NOCOUNT ON;
BEGIN

	IF NoT EXISTS(SELECT  1 FROM  [dbo].[tblQuestionInsert] WHERE QuestionType = @QuestionType)
	BEGIN 
	  SELECT 0 AS Status, 'This type is not exist in the system.' AS [Message]
	  return;
	END
	BEGIN TRY
	SELECT 1 AS Status, 'Data fetched' AS [Message]

	SELECT 
	Id
	,Question
	,QuestionType
	,CreatedOn
	,CreatedBy 
	FROM [dbo].[tblQuestionInsert]
	WHERE QuestionType = @QuestionType
END TRY

BEGIN CATCH
DECLARE @ErrorMessage VARCHAR(MAX) = ERROR_Message();
DECLARE @ErrorSeverity INT  = ERROR_Severity();
 DECLARE @ErrorState INT = ERROR_STATE();

        -- Return the error message
        SELECT 
            0 AS Status, 
            @ErrorMessage AS [Message];
			RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH
END