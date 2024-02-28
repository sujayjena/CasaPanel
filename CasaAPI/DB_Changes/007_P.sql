If Object_Id('StateMaster') Is Null
Begin
	Create Table StateMaster
	(
		StateId		Int				Primary Key Identity(1,1),
		StateName	VarChar(100)	Not Null,
		IsActive	Bit				Not Null Default(1),
		IsDeleted	Bit				Not Null Default(0),
		CreatedBy	BigInt			Not Null References Users(UserId),
		CreatedOn	DateTime		Not Null,
		ModifiedBy	BigInt					 References Users(UserId),
		ModifiedOn	DateTime
	)
End

GO

If Object_Id('RegionMaster') Is Null
Begin
	Create Table RegionMaster
	(
		RegionId	Int				Primary Key Identity(1,1),
		StateId		Int				Not Null References StateMaster(StateId),
		RegionName	VarChar(100)	Not Null,
		IsActive	Bit				Not Null Default(1),
		IsDeleted	Bit				Not Null Default(0),
		CreatedBy	BigInt			Not Null References Users(UserId),
		CreatedOn	DateTime		Not Null,
		ModifiedBy	BigInt					 References Users(UserId),
		ModifiedOn	DateTime
	)
End

GO

If Object_Id('DistrictMaster') Is Null
Begin
	Create Table DistrictMaster
	(
		DistrictId		Int				Primary Key Identity(1,1),
		RegionId		Int				Not Null References RegionMaster(RegionId),
		DistrictName	VarChar(100)	Not Null,
		IsActive		Bit				Not Null Default(1),
		IsDeleted		Bit				Not Null Default(0),
		CreatedBy		BigInt			Not Null References Users(UserId),
		CreatedOn		DateTime		Not Null,
		ModifiedBy		BigInt					 References Users(UserId),
		ModifiedOn		DateTime
	)
End

GO

If Object_Id('CityMaster') Is Null
Begin
	Create Table CityMaster
	(
		CityId			Int				Primary Key Identity(1,1),
		DistrictId		Int				Not Null References DistrictMaster(DistrictId),
		CityName		VarChar(100)	Not Null,
		IsActive		Bit				Not Null Default(1),
		IsDeleted		Bit				Not Null Default(0),
		CreatedBy		BigInt			Not Null References Users(UserId),
		CreatedOn		DateTime		Not Null,
		ModifiedBy		BigInt					 References Users(UserId),
		ModifiedOn		DateTime
	)
End

GO

If Object_Id('AreaMaster') Is Null
Begin
	Create Table AreaMaster
	(
		AreaId			Int				Primary Key Identity(1,1),
		CityId			Int				Not Null References CityMaster(CityId),
		AreaName		VarChar(100)	Not Null,
		IsActive		Bit				Not Null Default(1),
		IsDeleted		Bit				Not Null Default(0),
		CreatedBy		BigInt			Not Null References Users(UserId),
		CreatedOn		DateTime		Not Null,
		ModifiedBy		BigInt					 References Users(UserId),
		ModifiedOn		DateTime
	)
End

GO
