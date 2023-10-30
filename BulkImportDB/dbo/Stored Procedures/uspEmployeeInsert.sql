﻿      
/*      
 DECLARE @data as BluckEmployeeInsert          
INSERT INTO @data      
VALUES ( 'Ramesh','Dubey','Dubey',23,'10-06-2023','ankushdubey097@gmail.com','Mumbai','Admin','',''),('Suresh','Dubey','Dubey',23,'10-06-2023','ankushdubey097@gmail.com','Mumbai','Admin','','')      
 exec [uspEmployeeInsert] '' , @data      
*/      
CREATE PROCEDURE [dbo].[uspEmployeeInsert]        
@CreatedBy VARCHAR(50),        
@BluckEmployeeData  BluckEmployeeInsert READONLY        
       
AS        
BEGIN        
SET NOCOUNT ON;        
DECLARE @Status bit        
DECLARE @Message varchar(500)        
DECLARE @Data varchar(50)      
  
IF EXISTS (SELECT 1 FROM @BluckEmployeeData AS [bulk] WHERE EXISTS  
           (SELECT 1 FROM tblEmployees AS Emp WHERE Emp.FirstName = [bulk].FirstName   
           AND Emp.LastName = [Bulk].LastName   
           AND Emp.MiddleName = [bulk].MiddleName  
           AND Emp.Aderess = [bulk].Aderess  
           AND Emp.Age = [bulk].Age  
           --AND Emp.DOB = SUBSTRING([bulk].DOB, 7, 4) + '-' + SUBSTRING([bulk].DOB, 4, 2) + '-' + SUBSTRING([bulk].DOB, 1, 2)
		   AND Emp.DOB = [bulk].DOB
           AND Emp.EmailID = [bulk].EmailID  
           AND Emp.RoleID = [bulk].RoleID  
           AND Emp.Gender = [bulk].Gender
		   ) )  
BEGIN  
SELECT 0 AS [Status],  'Duplicate data found' AS Message 

select FirstName        
 ,MiddleName        
 ,LastName        
 ,Age        
 ,DOB        
 ,EmailID        
 ,Aderess
 ,RoleID    
 ,Gender from @BluckEmployeeData
RETURN   
END   
  
IF EXISTS (SELECT 1 FROM @BluckEmployeeData AS [bulk] WHERE EXISTS  
           (SELECT 1 FROM tblEmployees AS Emp WHERE Emp.EmailID = [bulk].EmailID) )  
BEGIN  
SELECT 0 AS [Status],  'Email alreday Exists in the system' AS Message  

select FirstName        
 ,MiddleName        
 ,LastName        
 ,Age        
 ,DOB        
 ,EmailID        
 ,Aderess
 ,RoleID    
 ,Gender from @BluckEmployeeData
RETURN   
END   
        
  BEGIN TRY        
   BEGIN        
 INSERT INTO [dbo].[tblEmployees]        
 ( FirstName        
 ,MiddleName        
 ,LastName        
 ,Age        
 ,DOB        
 ,EmailID        
 ,Aderess        
 ,CreatedBy        
 ,CreatedOn     
 ,RoleId    
 ,Gender    
 ,Skill    
 )        
 (SELECT         
 FirstName        
 ,MiddleName        
 ,LastName        
 ,Age        
 --,DOB
 ,CONVERT(VARCHAR, CONVERT(DATE, DOB, 101), 110)
 ,EmailID        
 ,Aderess        
 ,@CreatedBy        
 ,GETUTCDATE()    
 ,RoleID    
 ,Gender    
 ,Skill    
 from @BluckEmployeeData)        
 SET @Status = 1        
 SET @Message = 'Insert Successfully'        
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