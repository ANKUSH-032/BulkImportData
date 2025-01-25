-- uspQuestionList 'c#'

CREATE PROCEDURE uspQuestionTypeGet

AS
SET NOCOUNT ON;
BEGIN

	
	BEGIN TRY
	SELECT 1 AS Status, 'Data fetched' AS [Message]

	SELECT DISTINCT
	QuestionType
	 
	FROM [dbo].[tblQuestionInsert]
	
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