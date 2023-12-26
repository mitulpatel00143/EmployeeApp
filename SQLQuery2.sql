
-- emp count
CREATE or ALTER PROCEDURE [dbo].[stp_emp_GetEmployeeCount]
 AS
BEGIN
	SET NOCOUNT ON;

    SELECT count(1) AS EmpCount FROM [dbo].[EmployeeMaster] 
END
GO


-- viewAll

Create or Alter PROCEDURE [dbo].[stp_emp_GetAllEmployees]
	@SearchTerm NVARCHAR(255) = NULL,
    @SortColumn NVARCHAR(50) = 'EmployeeCode',
    @SortDirection NVARCHAR(4) = 'ASC',
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @StartRow INT = (@PageNumber - 1) * @PageSize + 1;
    DECLARE @EndRow INT = @PageNumber * @PageSize;

    SELECT emp.[Row_Id] ,emp.[EmployeeCode] ,emp.[FirstName] ,emp.[LastName] ,emp.[CountryId] ,emp.[StateId]
			,emp.[CityId] ,con.CountryName ,st.StateName ,ct.CityName ,emp.[EmailAddress] ,emp.[MobileNumber]
			,emp.[PanNumber] ,emp.[PassportNumber]
		  ,CASE WHEN emp.[Gender]=1 THEN 'Male'
				ELSE 'Female' END AS Gender
		  ,CASE WHEN emp.[IsActive]=1 THEN 'Yes'
				ELSE 'No' END AS ActiveStatus 
			 ,emp.[ProfileImage] ,emp.[DateOfBirth] ,emp.[DateOfJoinee] ,emp.[CreatedDate] ,emp.[UpdatedDate] 
			 ,emp.[IsDeleted] ,emp.[DeletedDate]
	FROM [dbo].[EmployeeMaster] emp
    INNER JOIN [dbo].[Country] con ON con.Row_Id = emp.CountryId
    INNER JOIN [dbo].[State] st ON st.Row_Id = emp.StateId
    INNER JOIN [dbo].[City] ct ON ct.Row_Id = emp.CityId
    WHERE (@SearchTerm IS NULL OR emp.FirstName LIKE '%' + @SearchTerm + '%') and IsDeleted = 0
    ORDER BY
        CASE WHEN @SortDirection = 'ASC' THEN
            CASE WHEN @SortColumn = 'EmployeeCode' THEN emp.EmployeeCode
				WHEN @SortColumn = 'FirstName' THEN emp.FirstName
				WHEN @SortColumn = 'LastName' THEN emp.LastName
				WHEN @SortColumn = 'EmailAddress' THEN emp.EmailAddress
				WHEN @SortColumn = 'CountryName' THEN con.CountryName
				WHEN @SortColumn = 'StateName' THEN st.StateName
				WHEN @SortColumn = 'CityName' THEN ct.CityName
				WHEN @SortColumn = 'PanNumber' THEN emp.PanNumber
				WHEN @SortColumn = 'PassportNumber' THEN emp.PassportNumber
				WHEN @SortColumn = 'Gender' THEN CASE WHEN emp.[Gender]=1 THEN 'Male' ELSE 'Female' END
				WHEN @SortColumn = 'ActiveStatus' THEN CASE WHEN emp.[IsActive]=1 THEN 'Yes'ELSE 'No' END
				WHEN @SortColumn = 'ProfileImage' THEN emp.ProfileImage
			END
        END ASC,
        CASE WHEN @SortDirection = 'DESC' THEN
            CASE WHEN @SortColumn = 'EmployeeCode' THEN emp.EmployeeCode 
				WHEN @SortColumn = 'FirstName' THEN emp.FirstName
				WHEN @SortColumn = 'LastName' THEN emp.LastName
				WHEN @SortColumn = 'EmailAddress' THEN emp.EmailAddress
				WHEN @SortColumn = 'CountryName' THEN con.CountryName
				WHEN @SortColumn = 'StateName' THEN st.StateName
				WHEN @SortColumn = 'CityName' THEN ct.CityName
				WHEN @SortColumn = 'PanNumber' THEN emp.PanNumber
				WHEN @SortColumn = 'PassportNumber' THEN emp.PassportNumber
				WHEN @SortColumn = 'Gender' THEN CASE WHEN emp.[Gender]=1 THEN 'Male' ELSE 'Female' END
				WHEN @SortColumn = 'ActiveStatus' THEN CASE WHEN emp.[IsActive]=1 THEN 'Yes'ELSE 'No' END
				WHEN @SortColumn = 'ProfileImage' THEN emp.ProfileImage
			END
        END DESC
    OFFSET @StartRow - 1 ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- employee by code
create or alter proc [dbo].[stp_emp_GetEmployeeByEmployeeCode]
(
	@EmployeeCode varchar(8)
)
as
begin
Declare @RowCount Int = 0
	set @RowCount = (select count(1) from dbo.EmployeeMaster with (NOLOCK)
			where EmployeeCode = @EmployeeCode and IsDeleted = 0)

	If(@RowCount > 0)
		Begin
			select EmployeeCode, FirstName, LastName, CountryId, StateId, CityId, 
					EmailAddress, MobileNumber, PanNumber, PassportNumber, DateOfBirth, DateOfJoinee,
					ProfileImage , Gender, IsActive
					from dbo.EmployeeMaster as emp
					--join dbo.Country as con on emp.CountryId = con.Row_Id
					--join dbo.State as st on emp.CountryId = st.Row_Id
					--join dbo.City as ct on emp.CountryId = ct.Row_Id
					where emp.EmployeeCode = @EmployeeCode
		End

end
GO

-- insert
CREATE or ALTER PROCEDURE [dbo].[stp_emp_InsertEmployee]
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@CountryId INT,
	@StateId INT,
	@CityId INT,
	@EmailAddress VARCHAR(100),
	@MobileNumber VARCHAR(15),
	@PanNumber VARCHAR(12),
	@PassportNumber VARCHAR(20),
	@DateOfBirth DATE,
	@DateOfJoinee DATE,
	@ProfileImage NVARCHAR(150),
	@Gender TINYINT,
	@IsActive BIT
AS
BEGIN

		SET NOCOUNT ON;

		INSERT INTO [dbo].[EmployeeMaster]
			   ( [FirstName], [LastName], [CountryId], [StateId], [CityId], [EmailAddress], [MobileNumber],
					[PanNumber], [PassportNumber], [ProfileImage], [Gender], [IsActive], [DateOfBirth], 
					[DateOfJoinee], [CreatedDate])
		 VALUES
			   (@FirstName, @LastName, @CountryId, @StateId, @CityId, @EmailAddress, @MobileNumber,
					@PanNumber, @PassportNumber, @ProfileImage, @Gender, @IsActive, @DateOfBirth,
					@DateOfJoinee, GETDATE())

END
GO

-- update
CREATE or ALTER PROCEDURE [dbo].[stp_emp_UpdateEmployee]
	@EmployeeCode  VARCHAR(10),
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50),
	@CountryId INT,
	@StateId INT,
	@CityId INT,
	@EmailAddress VARCHAR(100),
	@MobileNumber VARCHAR(15),
	@PanNumber VARCHAR(12),
	@PassportNumber VARCHAR(20),
	@DateOfBirth DATE,
	@DateOfJoinee DATE,
	@ProfileImage NVARCHAR(150),
	@Gender TINYINT,
	@IsActive BIT
AS
BEGIN
	Declare @RowCount Int = 0
	set @RowCount = (select count(1) from dbo.EmployeeMaster with (NOLOCK)
			where EmployeeCode = @EmployeeCode and IsDeleted = 0)

	If(@RowCount > 0)
		Begin
				
			SET NOCOUNT ON;

			UPDATE [dbo].[EmployeeMaster]
				SET  [FirstName] = @FirstName ,
						[LastName] =@LastName ,
						[CountryId] = @CountryId ,
						[StateId] = @StateId ,
						[CityId] = @CityId ,
						[EmailAddress] = @EmailAddress ,
						[MobileNumber] = @MobileNumber ,
						[PanNumber] = @PanNumber ,
						[PassportNumber] = @PassportNumber ,
						[DateOfBirth] = @DateOfBirth ,
						[DateOfJoinee] = @DateOfJoinee ,
						[ProfileImage] = @ProfileImage ,
						[Gender] = @Gender ,
						[IsActive] = @IsActive ,
						[UpdatedDate] = GETDATE() 
				WHERE  EmployeeCode=@EmployeeCode
		End

END
GO

-- delete
CREATE or ALTER PROCEDURE  [dbo].[stp_emp_DeleteEmployee]
	@EmployeeCode varchar(10)
AS
BEGIN	
	Declare @RowCount Int = 0
	set @RowCount = (select count(1) from dbo.EmployeeMaster with (NOLOCK)
			where EmployeeCode = @EmployeeCode and IsDeleted = 0)

	If(@RowCount > 0)
		Begin
			SET NOCOUNT ON;
	
			UPDATE [dbo].[EmployeeMaster]
				SET [IsActive] = 0 ,
					[IsDeleted] = 1 ,
					[DeletedDate] = GETDATE()
				WHERE [EmployeeCode] = @EmployeeCode

		End
	
END
GO

-- unique email
CREATE or ALTER PROCEDURE [dbo].[stp_emp_IsUniqueEmailAddress]
    @EmailAddress NVARCHAR(255),
	@EmployeeCode VARCHAR(10)
AS
BEGIN
    
	SET NOCOUNT ON;
	SELECT CASE WHEN EXISTS (
			SELECT 1 FROM EmployeeMaster
				WHERE EmailAddress = @EmailAddress and EmployeeCode != @EmployeeCode
		) THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS IsUnique;
	
END
GO

-- unique mobile
CREATE or ALTER PROCEDURE [dbo].[stp_emp_IsUniqueMobileNumber]
    @MobileNumber NVARCHAR(20),
	@EmployeeCode VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM EmployeeMaster
			WHERE MobileNumber = @MobileNumber and EmployeeCode != @EmployeeCode
    ) THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS IsUnique;
END
GO

-- unique pan
CREATE or ALTER PROCEDURE [dbo].[stp_emp_IsUniquePanNumber]
    @PanNumber NVARCHAR(20),
	@EmployeeCode VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CASE WHEN EXISTS (
			SELECT 1 FROM EmployeeMaster
				WHERE PanNumber = @PanNumber and EmployeeCode != @EmployeeCode
    ) THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS IsUnique;
END
GO

-- unique passport
CREATE or ALTER PROCEDURE [dbo].[stp_emp_IsUniquePassportNumber]
    @PassportNumber NVARCHAR(20),
	@EmployeeCode VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM EmployeeMaster
			WHERE PassportNumber = @PassportNumber and EmployeeCode != @EmployeeCode
    ) THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS IsUnique;
END
GO


-- countries
CREATE or ALTER PROCEDURE [dbo].[stp_emp_GetCountries]  
AS
BEGIN
	SET NOCOUNT ON; 

	SELECT [Row_Id] ,[CountryName] FROM [dbo].[Country]
   
END
GO

-- state by country
CREATE or ALTER PROCEDURE [dbo].[stp_emp_GetStatesByCountryId]  
	@CountryId INT
AS
BEGIN
	SET NOCOUNT ON; 

	SELECT [Row_Id] ,[CountryId] ,[StateName]
		FROM [dbo].[State]
		WHERE CountryId = @CountryId
   
END
GO

-- cities by state
CREATE or ALTER PROCEDURE [dbo].[stp_emp_GetCitiesByStateId]  
	@StateId INT
AS
BEGIN
	SET NOCOUNT ON; 
	 
	SELECT [Row_Id] ,[StateId] ,[CityName]
		FROM [dbo].[City]
		WHERE StateId = @StateId 
   
END
GO



