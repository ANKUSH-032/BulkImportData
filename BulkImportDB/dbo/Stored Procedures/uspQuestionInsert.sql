            
/*            
 DECLARE @data as bulkQuestionInsert                
INSERT INTO @data            
VALUES ( 'A1 2','c#'),    
('b1v','c1#')            
 exec [uspQuestionInsert] '' , @data            
*/            
CREATE PROCEDURE [dbo].[uspQuestionInsert]              
@CreatedBy VARCHAR(50),              
@BulkQuestionData bulkQuestionInsert READONLY              
             
AS              
BEGIN              
SET NOCOUNT ON;              
DECLARE @Status bit              
DECLARE @Message varchar(500)              
DECLARE @Data varchar(50)            
        
IF EXISTS (SELECT 1 FROM @bulkQuestionData AS [bulk] WHERE EXISTS        
           (SELECT 1 FROM [dbo].[tblQuestionInsert] AS question WHERE question.Question = [bulk].Question         
           AND question.QuestionType = [Bulk].QuestionType         
            
     ) )        
BEGIN        
SELECT 1 AS [Status],  'Duplicate data found' AS Message       
      
SELECT  
        [bulk].Question,  
        [bulk].QuestionType,  
         
        CASE WHEN EXISTS (  
            SELECT 1  
            FROM tblQuestionInsert AS question  
            WHERE question.Question = [bulk].Question     
            AND question.QuestionType = [Bulk].QuestionType      
             
        ) THEN 'Yes' ELSE 'No' END AS IsDuplicate  
    FROM @bulkQuestionData AS [bulk];  
  
    
RETURN         
END         
        
IF EXISTS (SELECT 1 FROM @BulkQuestionData AS [bulk] WHERE EXISTS        
           (SELECT 1 FROM tblQuestionInsert AS question WHERE question.Question = [bulk].Question) )        
BEGIN        
SELECT 0 AS [Status],  'Email alreday Exists in the system' AS Message        
      
SELECT  
         [bulk].Question,  
        [bulk].QuestionType,   
         
        CASE WHEN EXISTS (  
            SELECT 1  
            FROM tblQuestionInsert AS question  
            WHERE question.Question = [bulk].Question    
        ) THEN 'Yes' ELSE 'No' END AS IsDuplicate  
    FROM @BulkQuestionData AS [bulk];  
RETURN         
END         
else

  BEGIN TRY              
   BEGIN              
 INSERT INTO [dbo].tblQuestionInsert              
 (Question,
 QuestionType
 )              
 (SELECT               
 Question,
 QuestionType         
 from @bulkQuestionData)  
 
 SELECT  1 AS [Status], 'Insert Successfully' AS [Message], 1 AS TotalRecords;    

  END              
  END TRY              
           
              
  BEGIN CATCH  
  
 SET @Message = ERROR_MESSAGE();                
                
  DECLARE @ErrorSeverity INT = ERROR_SEVERITY();                
  DECLARE @ErrorState INT = ERROR_STATE();                  
  RAISERROR(@Message, @ErrorSeverity, @ErrorState);              
  END CATCH              
 SELECT @Status [Status], @Message [Message] , 1 [Data]                
END