If Exists(Select Top 1 1 From Information_Schema.Columns Where Table_Name='Users' And Column_Name='CustomerId')
Begin
	DECLARE @ConstraintName nvarchar(200)
	
	SELECT 
	    @ConstraintName = KCU.CONSTRAINT_NAME
	FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 
	INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU
	    ON KCU.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
	    AND KCU.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
	    AND KCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
	WHERE
	    KCU.TABLE_NAME = 'Users' AND
	    KCU.COLUMN_NAME = 'CustomerId'
		
	IF @ConstraintName IS NOT NULL
		EXEC('Alter Table Users drop  CONSTRAINT ' + @ConstraintName)

	Alter Table Users
	Drop Column CustomerId
End

GO

If Exists(Select Top 1 1 From Information_Schema.Columns Where Table_Name='Users' And Column_Name='TermsConditionsAccepted')
Begin
	Alter Table Users
	Drop Column TermsConditionsAccepted
End

GO

-- ValidateUserLoginByUsername @Username = 'admin@test.com', @Password='9pmoJgXzEUSgCQE9JKnazw=='
-- ValidateUserLoginByUsername @Username = 'testcustomer@test.com', @Password='9pmoJgXzEUSgCQE9JKnazw=='
Alter Procedure ValidateUserLoginByUsername
	@Username VarChar(100),
	@Password VarChar(200)
As
Begin
	SET NOCOUNT ON;

	Declare @UserId				BigInt = 0;
	Declare @IsUserLocked		Bit = 0;
	Declare @IsCorrectPassword	Bit = 0;

	--1. Retrieving UserId and Password match based on username
	Select
		@UserId = U.UserId,
		@IsUserLocked = U.IsUserLocked,
		@IsCorrectPassword = Case When U.Passwords = @Password COLLATE Latin1_General_BIN Then 1 Else 0 End
	From Users U
	Where U.EmailAddress = @Username
		And U.IsDeleted = 0

	--2. If Username is exists but Password is incorrect then to increase login retry count and lock status, if user IS NOT already locked
	If @IsCorrectPassword = 0
	Begin
		Update Users
		Set LoginRetryAttempt = LoginRetryAttempt + 1,
			IsUserLocked = Case When (LoginRetryAttempt + 1) >= 5 Then 1 Else 0 End, --Need to replace 5 with max. allowed counts from Config table
			ModifiedOn = GetDate()
		Where EmailAddress = @Username
			And IsUserLocked = 0
			And IsActive = 1
			And IsDeleted = 0
	End
	Else 
	--3. If credentials are correct and User IS NOT locked then to Reset Login Retry Count and Lock status
	Begin
		Update Users
		Set LoginRetryAttempt = 0,
			IsUserLocked = 0,
			ModifiedOn = GetDate()
		Where UserId = @UserId And IsUserLocked = 0
	End

	--4. To return Users details
	Select
		U.UserId,
		U.EmailAddress,
		U.MobileNo,
		U.EmployeeId,
		U.IsActive,
		U.LoginRetryAttempt,
		U.IsUserLocked,
		@IsCorrectPassword as IsCorrectPassword,
		EM.EmployeeName
		--EM.EmployeeCode,
		--EM.RoleId,
		--RM.RoleName,
		--CD.CompanyName,
		--CD.CustomerTypeId,
		--CTM.CustomerTypeName
	From Users U With(NoLock)
	Left Join EmployeeMaster EM With(NoLock)
		On EM.EmployeeId = U.EmployeeId
	--Left Join RoleMaster RM With(NoLock)
	--	On RM.RoleId = EM.RoleId
	--Left Join CustomerMaster CM With(NoLock)
	--	On CM.CustomerId = U.CustomerId
	--Left Join CustomerTypeMaster CTM With(NoLock)
	--	On CTM.CustomerTypeId = CD.CustomerTypeId
	Where U.EmailAddress = @Username
		And U.IsDeleted = 0
End

GO

-- GetProfileDetailsByToken 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6Iis2MHFRcnhQTHV5clBNTzlIRTUwR2c9PSIsIkVtYWlsQWRkcmVzcyI6ImFkbWluQHRlc3QuY29tIiwiTW9iaWxlTm8iOiIxMjM0NTY3ODkwIiwibmJmIjoxNjkzNDY4OTIzLCJleHAiOjE2OTM0NzI1MjMsImlhdCI6MTY5MzQ2ODkyM30.m_8Z57n9_nDFA3XXjTsjFOwNCL8w9U7-GntUrKd0FBM'
Alter Procedure GetProfileDetailsByToken
	@Token VarChar(500)
As
Begin
	Set NoCount On;
	
	Update UsersLoginHistory
	Set LastAccessOn	= GetDate(),
		TokenExpireOn	= (Case When RememberMe = 1 Then DateAdd(day, 1, GetDate()) Else DateAdd(mi, 60, GetDate()) End)
	Where UserToken = @Token;

	Select
		U.UserId,
		U.EmailAddress,
		U.MobileNo,
		U.EmployeeId,
		U.IsActive,
		EM.EmployeeName
		--,
		--
		--EM.EmployeeCode,
		--EM.RoleId,
		--RM.RoleName,
		--CD.CompanyName,
		--CD.CustomerTypeId,
		--CTM.CustomerTypeName
	From UsersLoginHistory LH With(NoLock)
	Inner Join Users U With(NoLock)
		On U.UserId = LH.UserId
	Left Join EmployeeMaster EM With(NoLock)
		On EM.EmployeeId = U.EmployeeId
	--Left Join CustomerDetails CD With(NoLock)
	--	On CD.CustomerId = U.CustomerId
	--Left Join RoleMaster RM With(NoLock)
	--	On RM.RoleId = EM.RoleId
	--Left Join CustomerTypeMaster CTM With(NoLock)
	--	On CTM.CustomerTypeId = CD.CustomerTypeId
	Where LH.UserToken = @Token And IsLoggedIn = 1 And LoggedOutOn Is Null
End

GO

