Use CASAPanel;

GO

-- This table will be re-created with exact fields
If Object_Id('EmployeeMaster') Is Null
Begin
	Create Table EmployeeMaster
	(
		EmployeeId		Int Primary Key Identity(1,1),
		EmployeeName	VarChar(100) Not Null,
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL,
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt,
		ModifiedOn		DateTime
	)
End

GO

-- This table will be re-created with exact fields
If Object_Id('CustomerMaster') Is Null
Begin
	Create Table CustomerMaster
	(
		CustomerId Int Primary Key Identity(1,1),
		CustomerName	VarChar(100) Not Null,
		IsActive	Bit NOT NULL Default(1),
		IsDeleted	Bit NOT NULL Default(0),
		CreatedBy	BigInt NOT NULL,
		CreatedOn	DateTime NOT NULL,
		ModifiedBy	BigInt,
		ModifiedOn	DateTime
	)
End

GO

If Object_Id('Users') Is Null
Begin
	Create Table Users
	(
		UserId			BigInt	Primary Key Identity(1,1),
		EmailAddress	VarChar(100) NOT NULL,
		MobileNo		VarChar(15) NOT NULL,
		Passwords		VarChar(200) NOT NULL,
		EmployeeId		Int NULL References EmployeeMaster(EmployeeId),
		CustomerId		Int NULL References CustomerMaster(CustomerId),
		TermsConditionsAccepted Bit NULL,
		LoginRetryAttempt	SmallInt Not Null Default(0),
		IsUserLocked	Bit Not Null Default(0),
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL References Users(UserId),
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt References Users(UserId),
		ModifiedOn		DateTime
	)
End

GO

If Object_Id('UsersLoginHistory') Is Null
Begin
	Create Table UsersLoginHistory
	(
		Id				BigInt	Primary Key Identity(1,1),
		UserId			BigInt	NOT NULL References Users(UserId),
		LoggedInOn		DateTime NOT NULL,
		IsLoggedIn		Bit	NOT NULL,
		UserToken		VarChar(1000) NOT NULL,
		LastAccessOn	DateTime NOT NULL,
		TokenExpireOn	DateTime NOT NULL,
		LoggedOutOn		DateTime,
		IsAutoLogout	Bit,
		DeviceName		VarChar(500) NULL,
		IPAddress		VarChar(30) NULL,
		RememberMe		Bit
	)
End

GO

If Not Exists(Select Top 1 1 From EmployeeMaster)
Begin
	Insert Into EmployeeMaster(EmployeeName,IsActive,IsDeleted,CreatedBy,CreatedOn)
	Values('Superadmin Employee',1,0,1,GetDate())
End

GO

If Not Exists(Select Top 1 1 From Users)
Begin
	Insert Into Users(EmailAddress,MobileNo,Passwords,EmployeeId,CustomerId,TermsConditionsAccepted,LoginRetryAttempt,IsUserLocked,IsActive,IsDeleted,CreatedBy,CreatedOn)
	Values('admin@test.com','1234567890','OzO8tvUkT4EcArzBe63pog==',1,NULL,1,0,0,1,0,1,getdate())
End

GO

-- ValidateUserLoginByUsername @Username = 'admin@test.com', @Password='test'
-- ValidateUserLoginByUsername @Username = 'testcustomer@test.com', @Password='test'
Create Or Alter Procedure ValidateUserLoginByUsername
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
		U.CustomerId,
		U.IsActive,
		U.LoginRetryAttempt,
		U.IsUserLocked,
		@IsCorrectPassword as IsCorrectPassword
		--,
		--EM.EmployeeName,
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
	Left Join CustomerMaster CM With(NoLock)
		On CM.CustomerId = U.CustomerId
	--Left Join CustomerTypeMaster CTM With(NoLock)
	--	On CTM.CustomerTypeId = CD.CustomerTypeId
	Where U.EmailAddress = @Username
		And U.IsDeleted = 0
End

GO

Create Or Alter Procedure SaveUserLoginHistory
	@UserId			BigInt,
	@UserToken		VarChar(2000),
	@TokenExpireOn	DateTime,
	@DeviceName		VarChar(500),
	@IPAddress		VarChar(30),
	@RememberMe		Bit,
	@IsLoggedIn		Bit
As
Begin
	SET NOCOUNT ON;

	-- To auto logout user sessions whose expiry date exceeded current date
	Update UsersLoginHistory
	Set IsLoggedIn = 0,
		LoggedOutOn = GETDATE(),
		IsAutoLogout = 1
	Where UserId = @UserId
		And IPAddress = @IPAddress
		And DeviceName = @DeviceName
		And IsLoggedIn = 1
		And LoggedOutOn Is Null
		And GETDATE() > TokenExpireOn

	-- Login
	If @IsLoggedIn = 1
	Begin
		If Not Exists
		(
			Select Top 1 1 From UsersLoginHistory With(NoLock)
			Where UserId = @UserId
				And UserToken = @UserToken
				And IPAddress = @IPAddress
				And DeviceName = @DeviceName
				And IsLoggedIn = 1
				And LoggedOutOn Is Null
		)
		Begin
			Insert Into UsersLoginHistory
			(
				UserId, LoggedInOn, IsLoggedIn, UserToken, LastAccessOn, TokenExpireOn, DeviceName, IPAddress, RememberMe
			)
			Values
			(
				@UserId, GETDATE(), @IsLoggedIn, @UserToken, GETDATE(), @TokenExpireOn, @DeviceName, @IPAddress, @RememberMe
			)
		End
		Else
		Begin
			Update UsersLoginHistory
			Set LastAccessOn = GETDATE(),
				TokenExpireOn = @TokenExpireOn
			Where UserId = @UserId
				And UserToken = @UserToken
				And IPAddress = @IPAddress
				And DeviceName = @DeviceName
		End
	End
	-- Logout
	Else
	Begin
		Update UsersLoginHistory
		Set LastAccessOn = GETDATE(),
			IsLoggedIn = 0,
			LoggedOutOn = GETDATE(),
			IsAutoLogout = 0
		Where UserId = @UserId
			And UserToken = @UserToken
			And IPAddress = @IPAddress
			And DeviceName = @DeviceName
	End
End

GO

-- GetProfileDetailsByToken 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6Iis2MHFRcnhQTHV5clBNTzlIRTUwR2c9PSIsIkVtYWlsQWRkcmVzcyI6ImFkbWluQHRlc3QuY29tIiwiTW9iaWxlTm8iOiIxMjM0NTY3ODkwIiwibmJmIjoxNjkyNzA3NjY3LCJleHAiOjE2OTI3MTEyNjcsImlhdCI6MTY5MjcwNzY2N30.evD2Pasdp3ovVoSHM-HqaMSl5OiV3PoTOywACXCIsPw'
Create OR Alter Procedure GetProfileDetailsByToken
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
		U.CustomerId,
		U.IsActive
		--,
		--EM.EmployeeName,
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
