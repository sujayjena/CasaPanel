CREATE OR ALTER PROCEDURE [dbo].[SaveImportSizeDetails]
(
	@XmlSizeData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempSizeDetail TABLE
	(
		SizeName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempSizeDetail
		(
		 SizeName,
		 IsActive,
		 IsValid
		)
	SELECT
		SizeName = T.Item.value('SizeName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlSizeData.nodes('/ArrayOfSizeImportSaveParameters/SizeImportSaveParameters') AS T(Item)

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Size Name already Exists'
	FROM @tempSizeDetail T
	INNER JOIN  SizeMaster SM  
		ON SM.SizeName = T.SizeName 
	WHERE T.IsValid=1

	INSERT INTO SizeMaster
	(
		SizeName,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   SizeName,
		   CASE WHEN IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempSizeDetail 
	WHERE IsValid = 1
	
	SELECT SizeName,IsActive,ValidationMessage
	FROM @tempSizeDetail
	WHERE IsValid = 0;
END

GO
CREATE OR ALTER PROCEDURE [dbo].[SaveImportBrandDetails]
(
	@XmlBrandData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempBrandDetail TABLE
	(
		BrandName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempBrandDetail
		(
		 BrandName,
		 IsActive,
		 IsValid
		)
	SELECT
		BrandName = T.Item.value('BrandName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlBrandData.nodes('/ArrayOfBrandImportSaveParameters/BrandImportSaveParameters') AS T(Item)

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Brand Name already Exists'
	FROM @tempBrandDetail T
	INNER JOIN  BrandMaster BM  
		ON BM.BrandName = T.BrandName 
	WHERE T.IsValid=1

	INSERT INTO BrandMaster
	(
		BrandName,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   BrandName,
		   CASE WHEN IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempBrandDetail 
	WHERE IsValid = 1
	
	SELECT BrandName,IsActive,ValidationMessage
	FROM @tempBrandDetail
	WHERE IsValid = 0;
END

GO

CREATE OR ALTER PROCEDURE [dbo].[SaveImportCollectionDetails]
(
	@XmlCollectionData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempCollectionDetail TABLE
	(
		CollectionName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempCollectionDetail
		(
		 CollectionName,
		 IsActive,
		 IsValid
		)
	SELECT
		CollectionName = T.Item.value('CollectionName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlCollectionData.nodes('/ArrayOfCollectionImportSaveParameters/CollectionImportSaveParameters') AS T(Item)

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Collection Name already Exists'
	FROM @tempCollectionDetail T
	INNER JOIN  CollectionMaster CM  
		ON CM.CollectionName = T.CollectionName 
	WHERE T.IsValid=1

	INSERT INTO CollectionMaster
	(
		CollectionName,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   CollectionName,
		   CASE WHEN IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempCollectionDetail 
	WHERE IsValid = 1
	
	SELECT CollectionName,IsActive,ValidationMessage
	FROM @tempCollectionDetail
	WHERE IsValid = 0;
END

GO
CREATE OR ALTER PROCEDURE [dbo].[SaveImportCategoryDetails]
(
	@XmlCategoryData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempCategoryDetail TABLE
	(
		CategoryName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempCategoryDetail
		(
		 CategoryName,
		 IsActive,
		 IsValid
		)
	SELECT
		CategoryName = T.Item.value('CategoryName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlCategoryData.nodes('/ArrayOfCategoryImportSaveParameters/CategoryImportSaveParameters') AS T(Item)

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Category Name already Exists'
	FROM @tempCategoryDetail T
	INNER JOIN CategoryMaster CM  
		ON CM.CategoryName = T.CategoryName 
	WHERE T.IsValid=1

	INSERT INTO CategoryMaster
	(
		CategoryName,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   CategoryName,
		   CASE WHEN IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempCategoryDetail 
	WHERE IsValid = 1
	
	SELECT CategoryName,IsActive,ValidationMessage
	FROM @tempCategoryDetail
	WHERE IsValid = 0;
END

GO

CREATE OR ALTER PROCEDURE [dbo].[SaveImportTypeDetails]
(
	@XmlTypeData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempTypeDetail TABLE
	(
		TypeName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempTypeDetail
		(
		 TypeName,
		 IsActive,
		 IsValid
		)
	SELECT
		TypeName = T.Item.value('TypeName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlTypeData.nodes('/ArrayOfTypeImportSaveParameters/TypeImportSaveParameters') AS T(Item)

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Type Name already Exists'
	FROM @tempTypeDetail T
	INNER JOIN TypeMaster TM  
		ON TM.TypeName = T.TypeName 
	WHERE T.IsValid=1

	INSERT INTO TypeMaster
	(
		TypeName,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   TypeName,
		   CASE WHEN IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempTypeDetail 
	WHERE IsValid = 1
	
	SELECT TypeName,IsActive,ValidationMessage
	FROM @tempTypeDetail
	WHERE IsValid = 0;
END

GO

CREATE OR ALTER PROCEDURE [dbo].[SaveImportPunchDetails]
(
	@XmlPunchData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempPunchDetail TABLE
	(
		PunchName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempPunchDetail
		(
		 PunchName,
		 IsActive,
		 IsValid
		)
	SELECT
		PunchName = T.Item.value('PunchName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlPunchData.nodes('/ArrayOfPunchImportSaveParameters/PunchImportSaveParameters') AS T(Item)

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Punch Name already Exists'
	FROM @tempPunchDetail T
	INNER JOIN PunchMaster PM  
		ON PM.PunchName = T.PunchName 
	WHERE T.IsValid=1

	INSERT INTO PunchMaster
	(
		PunchName,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   PunchName,
		   CASE WHEN IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempPunchDetail 
	WHERE IsValid = 1
	
	SELECT PunchName,IsActive,ValidationMessage
	FROM @tempPunchDetail
	WHERE IsValid = 0;
END

GO

CREATE OR ALTER PROCEDURE [dbo].[SaveImportSurfaceDetails]
(
	@XmlSurfaceData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempSurfaceDetail TABLE
	(
		SurfaceName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempSurfaceDetail
		(
		 SurfaceName,
		 IsActive,
		 IsValid
		)
	SELECT
		SurfaceName = T.Item.value('SurfaceName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlSurfaceData.nodes('/ArrayOfSurfaceImportSaveParameters/SurfaceImportSaveParameters') AS T(Item)

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Surface Name already Exists'
	FROM @tempSurfaceDetail T
	INNER JOIN SurfaceMaster SM  
		ON SM.SurfaceName = T.SurfaceName 
	WHERE T.IsValid=1

	INSERT INTO SurfaceMaster
	(
		SurfaceName,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   SurfaceName,
		   CASE WHEN IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempSurfaceDetail 
	WHERE IsValid = 1
	
	SELECT SurfaceName,IsActive,ValidationMessage
	FROM @tempSurfaceDetail
	WHERE IsValid = 0;
END

GO

CREATE OR ALTER PROCEDURE [dbo].[SaveImportThicknessDetails]
(
	@XmlThicknessData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempThicknessDetail TABLE
	(
		ThicknessName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempThicknessDetail
		(
		 ThicknessName,
		 IsActive,
		 IsValid
		)
	SELECT
		ThicknessName = T.Item.value('ThicknessName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlThicknessData.nodes('/ArrayOfThicknessImportSaveParameters/ThicknessImportSaveParameters') AS T(Item)

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Thickness Name already Exists'
	FROM @tempThicknessDetail T
	INNER JOIN ThicknessMaster TM  
		ON TM.ThicknessName = T.ThicknessName 
	WHERE T.IsValid=1

	INSERT INTO ThicknessMaster
	(
		ThicknessName,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   ThicknessName,
		   CASE WHEN IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempThicknessDetail 
	WHERE IsValid = 1
	
	SELECT ThicknessName,IsActive,ValidationMessage
	FROM @tempThicknessDetail
	WHERE IsValid = 0;
END

GO

CREATE OR ALTER PROCEDURE [dbo].[SaveImportTileSizeDetails]
(
	@XmlTileSizeData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempTileSizeDetail TABLE
	(
		TileSizeName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempTileSizeDetail
		(
		 TileSizeName,
		 IsActive,
		 IsValid
		)
	SELECT
		TileSizeName = T.Item.value('TileSizeName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlTileSizeData.nodes('/ArrayOfTileSizeImportSaveParameters/TileSizeImportSaveParameters') AS T(Item)

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'TileSize Name already Exists'
	FROM @tempTileSizeDetail T
	INNER JOIN TileSizeMaster TM  
		ON TM.TileSizeName = T.TileSizeName 
	WHERE T.IsValid=1

	INSERT INTO TileSizeMaster
	(
		TileSizeName,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   TileSizeName,
		   CASE WHEN IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempTileSizeDetail 
	WHERE IsValid = 1
	
	SELECT TileSizeName,IsActive,ValidationMessage
	FROM @tempTileSizeDetail
	WHERE IsValid = 0;
END

Go
CREATE OR ALTER   PROCEDURE [dbo].[SaveImportRoleDetails]
(
	@XmlRoleData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempRoleDetail TABLE
	(
		RoleName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempRoleDetail
		(
		 RoleName,
		 IsActive,
		 IsValid
		)
	SELECT
		RoleName = T.Item.value('RoleName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlRoleData.nodes('/ArrayOfRoleImportSaveParameters/RoleImportSaveParameters') AS T(Item)

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Role Name already Exists'
	FROM @tempRoleDetail T
	INNER JOIN  RoleMaster RM  
		ON RM.RoleName = T.RoleName 
	WHERE T.IsValid=1

	INSERT INTO RoleMaster
	(
		RoleName,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   RoleName,
		   CASE WHEN IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempRoleDetail 
	WHERE IsValid = 1
	
	SELECT RoleName,IsActive,ValidationMessage
	FROM @tempRoleDetail
	WHERE IsValid = 0;
END
Go
CREATE OR ALTER  PROCEDURE [dbo].[SaveImportReportingHierarchyDetails]
(
	@XmlReportingHierarchyData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @tempReportingHierarchyData TABLE
	(
		RoleName VARCHAR(100),
		ReportingRoleName VARCHAR(100),		
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)		
	)

	INSERT INTO @tempReportingHierarchyData
		(
		 RoleName,
		 ReportingRoleName,
		 IsActive,
		 IsValid
		)
	SELECT
		RoleName = T.Item.value('RoleName[1]', 'VARCHAR(100)'),
		ReportingRoleName = T.Item.value('ReportingRoleName[1]', 'VARCHAR(100)'),		
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlReportingHierarchyData.nodes('/ArrayOfReportingHierarchyImportSaveParameters/ReportingHierarchyImportSaveParameters') AS T(Item)
	
	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Role Name already exists., '
	FROM @tempReportingHierarchyData T
	INNER JOIN  RoleMaster RM  
		ON LOWER(TRIM(RM.RoleName)) = LOWER(TRIM(T.RoleName))
	WHERE T.IsValid=1

		UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Reporting Role Name not exists., '
	FROM @tempReportingHierarchyData T
	LEFT JOIN RoleMaster RM 
	ON LOWER(TRIM(RM.RoleName)) = LOWER(TRIM(T.ReportingRoleName))
	WHERE  RM.RoleId IS NULL

	

	INSERT INTO ReportingHierarchyMaster
	(
		RoleId,
		ReportingRoleId,		
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		 RM.RoleId, (select Top 1 RoleId from RoleMaster where LOWER(TRIM(RoleName))=LOWER(TRIM(tempd.ReportingRoleName))) ,
		   
		   CASE WHEN tempd.IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempReportingHierarchyData tempd
	INNER JOIN RoleMaster RM 
		ON LOWER(TRIM(RM.RoleName)) = LOWER(TRIM(tempd.RoleName))
	
	WHERE IsValid = 1
	
	SELECT 
		RoleName,ReportingRoleName,
		IsActive,ValidationMessage
	FROM @tempReportingHierarchyData
	WHERE IsValid = 0;
END



