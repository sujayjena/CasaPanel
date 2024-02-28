USE [CASAPanel]
GO

If Object_Id('SizeMaster') Is Null
Begin
	CREATE Table SizeMaster
	(
		SizeId		Int Primary Key Identity(1,1),
		SizeName	VarChar(100) Not Null,
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL References Users(UserId),
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt References Users(UserId),
		ModifiedOn		DateTime
	)
End

GO

CREATE OR ALTER PROCEDURE SaveSizeDetails
(
	@SizeId BIGINT,
	@SizeName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	IF ((@SizeId=0 AND 	EXISTS
			(
				SELECT TOP 1 1 
				FROM SizeMaster WITH(NOLOCK) 
				WHERE  SizeName=@SizeName
			)
		 )
		 OR
		(@SizeId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM SizeMaster WITH(NOLOCK) 
				WHERE  SizeName=@SizeName and SizeId<>@SizeId
			)
		))
		BEGIN
			SET @Result=@IsNameExists;
		END
	ELSE
		BEGIN
			IF (@SizeId=0)
			BEGIN
				INSERT INTO SizeMaster
				(
					SizeName, IsActive,IsDeleted,CreatedBy,CreatedOn
				)
				VALUES
				(
					@SizeName, @IsActive,0,@LoggedInUserId,GETDATE()
				)
				
				SET @Result = SCOPE_IDENTITY();
			END
			ELSE IF(@SizeId> 0 and EXISTS(SELECT TOP 1 1 FROM SizeMaster WHERE SizeId=@SizeId))
			BEGIN
				UPDATE SizeMaster
				SET SizeName=@SizeName,
					IsActive=@IsActive,
					ModifiedBy=@LoggedInUserId,
					ModifiedOn=GETDATE()
				WHERE SizeId=@SizeId

				SET @Result = @SizeId;
			END
			ELSE
			BEGIN
				SET @Result=@NoRecordExists
			END
		END
	
	SELECT @Result as Result 
END

GO

/*
	Version : 1.0
	Created Date : 23 AUG 2023
	Execution : EXEC [dbo].[GetSizesList]
	Description : Get Size from SizeMaster
	EXEC [dbo].[GetSizesList]  
		@PageSize=10,
	    @PageNo=1,
	    @SortBy='',
	    @OrderBy='',
		@SizeName='',
		@IsActive=NULL
*/

CREATE OR ALTER PROCEDURE [dbo].[GetSizesList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@SizeName VARCHAR(100)=null,
	@IsActive BIT
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @SizeName = ISNULL(@SizeName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	SET @STR = N'SELECT
					SM.SizeId,SM.SizeName,SM.IsActive,CreatorEM.EmployeeName As CreatorName,
					SM.CreatedBy,SM.CreatedOn
				FROM SizeMaster SM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = SM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (@SizeName='''' OR SM.SizeName LIKE ''%''+@SizeName+''%'')
					AND (@IsActive IS NULL OR SM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @STR,
						N'@SizeName VARCHAR(100),@IsActive BIT',
						@SizeName,@IsActive
END
GO

CREATE OR ALTER PROCEDURE GetSizeDetailsById
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		SM.SizeId,
		SM.SizeName,
		SM.IsActive,
		CreatorEM.EmployeeName As CreatorName,
		SM.CreatedBy,
		SM.CreatedOn
	FROM SizeMaster SM WITH(NOLOCK)
	INNER JOIN Users U WITH(NOLOCK)
	 	ON U.UserId = SM.CreatedBy
	LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
		ON CreatorEM.EmployeeId = U.EmployeeId
		  And U.EmployeeId IS NOT NULL
	WHERE SizeId = @Id
END

GO

If Object_Id('BrandMaster') Is Null
Begin
	CREATE Table BrandMaster
	(
		BrandId		Int Primary Key Identity(1,1),
		BrandName	VarChar(100) Not Null,
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL References Users(UserId),
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt References Users(UserId),
		ModifiedOn		DateTime
	)
End

GO

CREATE OR ALTER PROCEDURE SaveBrandDetails
(
	@BrandId BIGINT,
	@BrandName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	IF ((@BrandId=0 AND 	EXISTS
			(
				SELECT TOP 1 1 
				FROM BrandMaster WITH(NOLOCK) 
				WHERE  BrandName=@BrandName
			)
		 )
		 OR
		(@BrandId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM BrandMaster WITH(NOLOCK) 
				WHERE  BrandName=@BrandName and BrandId<>@BrandId
			)
		))
		BEGIN
			SET @Result=@IsNameExists;
		END
	ELSE
		BEGIN
			IF (@BrandId=0)
			BEGIN
				INSERT INTO BrandMaster
				(
					BrandName, IsActive,IsDeleted,CreatedBy,CreatedOn
				)
				VALUES
				(
					@BrandName, @IsActive,0,@LoggedInUserId,GETDATE()
				)
				
				SET @Result = SCOPE_IDENTITY();
			END
			ELSE IF(@BrandId> 0 and EXISTS(SELECT TOP 1 1 FROM BrandMaster WHERE BrandId=@BrandId))
			BEGIN
				UPDATE BrandMaster
				SET BrandName=@BrandName,
					IsActive=@IsActive,
					ModifiedBy=@LoggedInUserId,
					ModifiedOn=GETDATE()
				WHERE BrandId=@BrandId

				SET @Result = @BrandId;
			END
			ELSE
			BEGIN
				SET @Result=@NoRecordExists
			END
		END
	
	SELECT @Result as Result 
END

GO

/*
	Version : 1.0
	Created Date : 23 AUG 2023
	Execution : EXEC [dbo].[GetBrandsList]
	Description : Get Brand from BrandMaster
	EXEC [dbo].[GetBrandsList]  
		@PageSize=10,
	    @PageNo=1,
	    @SortBy='',
	    @OrderBy='',
		@BrandName='',
		@IsActive=NULL
*/

CREATE OR ALTER PROCEDURE [dbo].[GetBrandsList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@BrandName VARCHAR(100)=null,
	@IsActive BIT
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @BrandName = ISNULL(@BrandName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	SET @STR = N'SELECT
					BM.BrandId,BM.BrandName,BM.IsActive,CreatorEM.EmployeeName As CreatorName,
					BM.CreatedBy,BM.CreatedOn
				FROM BrandMaster BM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = BM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (@BrandName='''' OR BM.BrandName LIKE ''%''+@BrandName+''%'')
					AND (@IsActive IS NULL OR BM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @STR,
						N'@BrandName VARCHAR(100),@IsActive BIT',
						@BrandName,@IsActive
END
GO

CREATE OR ALTER PROCEDURE GetBrandDetailsById
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		BM.BrandId,
		BM.BrandName,
		BM.IsActive,
		CreatorEM.EmployeeName As CreatorName,
		BM.CreatedBy,
		BM.CreatedOn
	FROM BrandMaster BM WITH(NOLOCK)
	INNER JOIN Users U WITH(NOLOCK)
	 	ON U.UserId = BM.CreatedBy
	LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
		ON CreatorEM.EmployeeId = U.EmployeeId
		  And U.EmployeeId IS NOT NULL
	WHERE BrandId = @Id
END


GO
If Object_Id('CollectionMaster') Is Null
Begin
	CREATE Table CollectionMaster
	(
		CollectionId		Int Primary Key Identity(1,1),
		CollectionName	VarChar(100) Not Null,
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL References Users(UserId),
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt References Users(UserId),
		ModifiedOn		DateTime
	)
End

GO

CREATE OR ALTER PROCEDURE SaveCollectionDetails
(
	@CollectionId BIGINT,
	@CollectionName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	IF ((@CollectionId=0 AND 	EXISTS
			(
				SELECT TOP 1 1 
				FROM CollectionMaster WITH(NOLOCK) 
				WHERE  CollectionName=@CollectionName
			)
		 )
		 OR
		(@CollectionId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM CollectionMaster WITH(NOLOCK) 
				WHERE  CollectionName=@CollectionName and CollectionId<>@CollectionId
			)
		))
		BEGIN
			SET @Result=@IsNameExists;
		END
	ELSE
		BEGIN
			IF (@CollectionId=0)
			BEGIN
				INSERT INTO CollectionMaster
				(
					CollectionName, IsActive,IsDeleted,CreatedBy,CreatedOn
				)
				VALUES
				(
					@CollectionName, @IsActive,0,@LoggedInUserId,GETDATE()
				)
				
				SET @Result = SCOPE_IDENTITY();
			END
			ELSE IF(@CollectionId> 0 and EXISTS(SELECT TOP 1 1 FROM CollectionMaster WHERE CollectionId=@CollectionId))
			BEGIN
				UPDATE CollectionMaster
				SET CollectionName=@CollectionName,
					IsActive=@IsActive,
					ModifiedBy=@LoggedInUserId,
					ModifiedOn=GETDATE()
				WHERE CollectionId=@CollectionId

				SET @Result = @CollectionId;
			END
			ELSE
			BEGIN
				SET @Result=@NoRecordExists
			END
		END
	
	SELECT @Result as Result 
END

GO

/*
	Version : 1.0
	Created Date : 23 AUG 2023
	Execution : EXEC [dbo].[GetCollectionsList]
	Description : Get Collection from CollectionMaster
	EXEC [dbo].[GetCollectionsList]  
		@PageSize=10,
	    @PageNo=1,
	    @SortBy='',
	    @OrderBy='',
		@CollectionName='',
		@IsActive=NULL
*/

CREATE OR ALTER PROCEDURE [dbo].[GetCollectionsList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@CollectionName VARCHAR(100)=null,
	@IsActive BIT
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @CollectionName = ISNULL(@CollectionName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	SET @STR = N'SELECT
					CM.CollectionId,CM.CollectionName,CM.IsActive,CreatorEM.EmployeeName As CreatorName,
					CM.CreatedBy,CM.CreatedOn
				FROM CollectionMaster CM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = CM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (@CollectionName='''' OR CM.CollectionName LIKE ''%''+@CollectionName+''%'')
					AND (@IsActive IS NULL OR CM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @STR,
						N'@CollectionName VARCHAR(100),@IsActive BIT',
						@CollectionName,@IsActive
END
GO

CREATE OR ALTER PROCEDURE GetCollectionDetailsById
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		CM.CollectionId,
		CM.CollectionName,
		CM.IsActive,
		CreatorEM.EmployeeName As CreatorName,
		CM.CreatedBy,
		CM.CreatedOn
	FROM CollectionMaster CM WITH(NOLOCK)
	INNER JOIN Users U WITH(NOLOCK)
	 	ON U.UserId = CM.CreatedBy
	LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
		ON CreatorEM.EmployeeId = U.EmployeeId
		  And U.EmployeeId IS NOT NULL
	WHERE CollectionId = @Id
END

GO

USE [CASAPanel]
GO
If Object_Id('CategoryMaster') Is Null
Begin
	CREATE Table CategoryMaster
	(
		CategoryId		Int Primary Key Identity(1,1),
		CategoryName	VarChar(100) Not Null,
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL References Users(UserId),
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt References Users(UserId),
		ModifiedOn		DateTime
	)
End

GO

CREATE OR ALTER PROCEDURE SaveCategoryDetails
(
	@CategoryId BIGINT,
	@CategoryName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	IF ((@CategoryId=0 AND 	EXISTS
			(
				SELECT TOP 1 1 
				FROM CategoryMaster WITH(NOLOCK) 
				WHERE  CategoryName=@CategoryName
			)
		 )
		 OR
		(@CategoryId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM CategoryMaster WITH(NOLOCK) 
				WHERE  CategoryName=@CategoryName and CategoryId<>@CategoryId
			)
		))
		BEGIN
			SET @Result=@IsNameExists;
		END
	ELSE
		BEGIN
			IF (@CategoryId=0)
			BEGIN
				INSERT INTO CategoryMaster
				(
					CategoryName, IsActive,IsDeleted,CreatedBy,CreatedOn
				)
				VALUES
				(
					@CategoryName, @IsActive,0,@LoggedInUserId,GETDATE()
				)
				
				SET @Result = SCOPE_IDENTITY();
			END
			ELSE IF(@CategoryId> 0 and EXISTS(SELECT TOP 1 1 FROM CategoryMaster WHERE CategoryId=@CategoryId))
			BEGIN
				UPDATE CategoryMaster
				SET CategoryName=@CategoryName,
					IsActive=@IsActive,
					ModifiedBy=@LoggedInUserId,
					ModifiedOn=GETDATE()
				WHERE CategoryId=@CategoryId

				SET @Result = @CategoryId;
			END
			ELSE
			BEGIN
				SET @Result=@NoRecordExists
			END
		END
	
	SELECT @Result as Result 
END

GO

/*
	Version : 1.0
	Created Date : 23 AUG 2023
	Execution : EXEC [dbo].[GetCategorysList]
	Description : Get Category from CategoryMaster
	EXEC [dbo].[GetCategorysList]  
		@PageSize=10,
	    @PageNo=1,
	    @SortBy='',
	    @OrderBy='',
		@CategoryName='',
		@IsActive=NULL
*/

CREATE OR ALTER PROCEDURE [dbo].[GetCategorysList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@CategoryName VARCHAR(100)=null,
	@IsActive BIT
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @CategoryName = ISNULL(@CategoryName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	SET @STR = N'SELECT
					CM.CategoryId,CM.CategoryName,CM.IsActive,CreatorEM.EmployeeName As CreatorName,
					CM.CreatedBy,CM.CreatedOn
				FROM CategoryMaster CM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = CM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (@CategoryName='''' OR CM.CategoryName LIKE ''%''+@CategoryName+''%'')
					AND (@IsActive IS NULL OR CM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @STR,
						N'@CategoryName VARCHAR(100),@IsActive BIT',
						@CategoryName,@IsActive
END
GO

CREATE OR ALTER PROCEDURE GetCategoryDetailsById
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		CM.CategoryId,
		CM.CategoryName,
		CM.IsActive,
		CreatorEM.EmployeeName As CreatorName,
		CM.CreatedBy,
		CM.CreatedOn
	FROM CategoryMaster CM WITH(NOLOCK)
	INNER JOIN Users U WITH(NOLOCK)
	 	ON U.UserId = CM.CreatedBy
	LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
		ON CreatorEM.EmployeeId = U.EmployeeId
		  And U.EmployeeId IS NOT NULL
	WHERE CategoryId = @Id
END
GO
If Object_Id('TypeMaster') Is Null
Begin
	CREATE Table TypeMaster
	(
		TypeId		Int Primary Key Identity(1,1),
		TypeName	VarChar(100) Not Null,
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL References Users(UserId),
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt References Users(UserId),
		ModifiedOn		DateTime
	)
End
GO
CREATE OR ALTER PROCEDURE SaveTypeDetails
(
	@TypeId BIGINT,
	@TypeName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	IF ((@TypeId=0 AND 	EXISTS
			(
				SELECT TOP 1 1 
				FROM TypeMaster WITH(NOLOCK) 
				WHERE  TypeName=@TypeName
			)
		 )
		 OR
		(@TypeId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM TypeMaster WITH(NOLOCK) 
				WHERE  TypeName=@TypeName and TypeId<>@TypeId
			)
		))
		BEGIN
			SET @Result=@IsNameExists;
		END
	ELSE
		BEGIN
			IF (@TypeId=0)
			BEGIN
				INSERT INTO TypeMaster
				(
					TypeName, IsActive,IsDeleted,CreatedBy,CreatedOn
				)
				VALUES
				(
					@TypeName, @IsActive,0,@LoggedInUserId,GETDATE()
				)
				
				SET @Result = SCOPE_IDENTITY();
			END
			ELSE IF(@TypeId> 0 and EXISTS(SELECT TOP 1 1 FROM TypeMaster WHERE TypeId=@TypeId))
			BEGIN
				UPDATE TypeMaster
				SET TypeName=@TypeName,
					IsActive=@IsActive,
					ModifiedBy=@LoggedInUserId,
					ModifiedOn=GETDATE()
				WHERE TypeId=@TypeId

				SET @Result = @TypeId;
			END
			ELSE
			BEGIN
				SET @Result=@NoRecordExists
			END
		END
	
	SELECT @Result as Result 
END
GO
/*
	Version : 1.0
	Created Date : 23 AUG 2023
	Execution : EXEC [dbo].[GetTypesList]
	Description : Get Type from TypeMaster
	EXEC [dbo].[GetTypesList]  
		@PageSize=10,
	    @PageNo=1,
	    @SortBy='',
	    @OrderBy='',
		@TypeName='',
		@IsActive=NULL
*/
CREATE OR ALTER PROCEDURE [dbo].[GetTypesList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@TypeName VARCHAR(100)=null,
	@IsActive BIT
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @TypeName = ISNULL(@TypeName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	SET @STR = N'SELECT
					TM.TypeId,TM.TypeName,TM.IsActive,CreatorEM.EmployeeName As CreatorName,
					TM.CreatedBy,TM.CreatedOn
				FROM TypeMaster TM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = TM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (@TypeName='''' OR TM.TypeName LIKE ''%''+@TypeName+''%'')
					AND (@IsActive IS NULL OR CM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @STR,
						N'@TypeName VARCHAR(100),@IsActive BIT',
						@TypeName,@IsActive
END
GO

CREATE OR ALTER PROCEDURE GetTypeDetailsById
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		TM.TypeId,
		TM.TypeName,
		TM.IsActive,
		CreatorEM.EmployeeName As CreatorName,
		TM.CreatedBy,
		TM.CreatedOn
	FROM TypeMaster TM WITH(NOLOCK)
	INNER JOIN Users U WITH(NOLOCK)
	 	ON U.UserId = TM.CreatedBy
	LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
		ON CreatorEM.EmployeeId = U.EmployeeId
		  And U.EmployeeId IS NOT NULL
	WHERE TypeId = @Id
END
GO
If Object_Id('PunchMaster') Is Null
Begin
	CREATE Table PunchMaster
	(
		PunchId		Int Primary Key Identity(1,1),
		PunchName	VarChar(100) Not Null,
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL References Users(UserId),
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt References Users(UserId),
		ModifiedOn		DateTime
	)
End
GO
CREATE OR ALTER PROCEDURE SavePunchDetails
(
	@PunchId BIGINT,
	@PunchName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	IF ((@PunchId=0 AND 	EXISTS
			(
				SELECT TOP 1 1 
				FROM PunchMaster WITH(NOLOCK) 
				WHERE  PunchName=@PunchName
			)
		 )
		 OR
		(@PunchId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM PunchMaster WITH(NOLOCK) 
				WHERE  PunchName=@PunchName and PunchId<>@PunchId
			)
		))
		BEGIN
			SET @Result=@IsNameExists;
		END
	ELSE
		BEGIN
			IF (@PunchId=0)
			BEGIN
				INSERT INTO PunchMaster
				(
					PunchName, IsActive,IsDeleted,CreatedBy,CreatedOn
				)
				VALUES
				(
					@PunchName, @IsActive,0,@LoggedInUserId,GETDATE()
				)
				
				SET @Result = SCOPE_IDENTITY();
			END
			ELSE IF(@PunchId> 0 and EXISTS(SELECT TOP 1 1 FROM PunchMaster WHERE PunchId=@PunchId))
			BEGIN
				UPDATE PunchMaster
				SET PunchName=@PunchName,
					IsActive=@IsActive,
					ModifiedBy=@LoggedInUserId,
					ModifiedOn=GETDATE()
				WHERE PunchId=@PunchId

				SET @Result = @PunchId;
			END
			ELSE
			BEGIN
				SET @Result=@NoRecordExists
			END
		END
	
	SELECT @Result as Result 
END
GO
/*
	Version : 1.0
	Created Date : 23 AUG 2023
	Execution : EXEC [dbo].[GetPunchsList]
	Description : Get Punch from PunchMaster
	EXEC [dbo].[GetPunchsList]  
		@PageSize=10,
	    @PageNo=1,
	    @SortBy='',
	    @OrderBy='',
		@PunchName='',
		@IsActive=NULL
*/
CREATE OR ALTER PROCEDURE [dbo].[GetPunchsList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@PunchName VARCHAR(100)=null,
	@IsActive BIT
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @PunchName = ISNULL(@PunchName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	SET @STR = N'SELECT
					PM.PunchId,PM.PunchName,PM.IsActive,CreatorEM.EmployeeName As CreatorName,
					PM.CreatedBy,PM.CreatedOn
				FROM PunchMaster PM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = PM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (@PunchName='''' OR PM.PunchName LIKE ''%''+@PunchName+''%'')
					AND (@IsActive IS NULL OR PM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @STR,
						N'@PunchName VARCHAR(100),@IsActive BIT',
						@PunchName,@IsActive
END
GO

CREATE OR ALTER PROCEDURE GetPunchDetailsById
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		PM.PunchId,
		PM.PunchName,
		PM.IsActive,
		CreatorEM.EmployeeName As CreatorName,
		PM.CreatedBy,
		PM.CreatedOn
	FROM PunchMaster PM WITH(NOLOCK)
	INNER JOIN Users U WITH(NOLOCK)
	 	ON U.UserId = PM.CreatedBy
	LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
		ON CreatorEM.EmployeeId = U.EmployeeId
		  And U.EmployeeId IS NOT NULL
	WHERE PunchId = @Id
END
GO
If Object_Id('SurfaceMaster') Is Null
Begin
	CREATE Table SurfaceMaster
	(
		SurfaceId		Int Primary Key Identity(1,1),
		SurfaceName	VarChar(100) Not Null,
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL References Users(UserId),
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt References Users(UserId),
		ModifiedOn		DateTime
	)
End
GO
CREATE OR ALTER PROCEDURE SaveSurfaceDetails
(
	@SurfaceId BIGINT,
	@SurfaceName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	IF ((@SurfaceId=0 AND 	EXISTS
			(
				SELECT TOP 1 1 
				FROM SurfaceMaster WITH(NOLOCK) 
				WHERE  SurfaceName=@SurfaceName
			)
		 )
		 OR
		(@SurfaceId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM SurfaceMaster WITH(NOLOCK) 
				WHERE  SurfaceName=@SurfaceName and SurfaceId<>@SurfaceId
			)
		))
		BEGIN
			SET @Result=@IsNameExists;
		END
	ELSE
		BEGIN
			IF (@SurfaceId=0)
			BEGIN
				INSERT INTO SurfaceMaster
				(
					SurfaceName, IsActive,IsDeleted,CreatedBy,CreatedOn
				)
				VALUES
				(
					@SurfaceName, @IsActive,0,@LoggedInUserId,GETDATE()
				)
				
				SET @Result = SCOPE_IDENTITY();
			END
			ELSE IF(@SurfaceId> 0 and EXISTS(SELECT TOP 1 1 FROM SurfaceMaster WHERE SurfaceId=@SurfaceId))
			BEGIN
				UPDATE SurfaceMaster
				SET SurfaceName=@SurfaceName,
					IsActive=@IsActive,
					ModifiedBy=@LoggedInUserId,
					ModifiedOn=GETDATE()
				WHERE SurfaceId=@SurfaceId

				SET @Result = @SurfaceId;
			END
			ELSE
			BEGIN
				SET @Result=@NoRecordExists
			END
		END
	
	SELECT @Result as Result 
END
GO
/*
	Version : 1.0
	Created Date : 23 AUG 2023
	Execution : EXEC [dbo].[GetSurfacesList]
	Description : Get Surface from SurfaceMaster
	EXEC [dbo].[GetSurfacesList]  
		@PageSize=10,
	    @PageNo=1,
	    @SortBy='',
	    @OrderBy='',
		@SurfaceName='',
		@IsActive=NULL
*/
CREATE OR ALTER PROCEDURE [dbo].[GetSurfacesList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@SurfaceName VARCHAR(100)=null,
	@IsActive BIT
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @SurfaceName = ISNULL(@SurfaceName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	SET @STR = N'SELECT
					SM.SurfaceId,SM.SurfaceName,SM.IsActive,CreatorEM.EmployeeName As CreatorName,
					SM.CreatedBy,SM.CreatedOn
				FROM SurfaceMaster SM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = SM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (@SurfaceName='''' OR SM.SurfaceName LIKE ''%''+@SurfaceName+''%'')
					AND (@IsActive IS NULL OR SM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @STR,
						N'@SurfaceName VARCHAR(100),@IsActive BIT',
						@SurfaceName,@IsActive
END
GO

CREATE OR ALTER PROCEDURE GetSurfaceDetailsById
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		SM.SurfaceId,
		SM.SurfaceName,
		SM.IsActive,
		CreatorEM.EmployeeName As CreatorName,
		SM.CreatedBy,
		SM.CreatedOn
	FROM SurfaceMaster SM WITH(NOLOCK)
	INNER JOIN Users U WITH(NOLOCK)
	 	ON U.UserId = SM.CreatedBy
	LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
		ON CreatorEM.EmployeeId = U.EmployeeId
		  And U.EmployeeId IS NOT NULL
	WHERE SurfaceId = @Id
END
GO
If Object_Id('ThicknessMaster') Is Null
Begin
	CREATE Table ThicknessMaster
	(
		ThicknessId		Int Primary Key Identity(1,1),
		ThicknessName	VarChar(100) Not Null,
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL References Users(UserId),
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt References Users(UserId),
		ModifiedOn		DateTime
	)
End
GO
CREATE OR ALTER PROCEDURE SaveThicknessDetails
(
	@ThicknessId BIGINT,
	@ThicknessName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	IF ((@ThicknessId=0 AND 	EXISTS
			(
				SELECT TOP 1 1 
				FROM ThicknessMaster WITH(NOLOCK) 
				WHERE  ThicknessName=@ThicknessName
			)
		 )
		 OR
		(@ThicknessId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM ThicknessMaster WITH(NOLOCK) 
				WHERE  ThicknessName=@ThicknessName and ThicknessId<>@ThicknessId
			)
		))
		BEGIN
			SET @Result=@IsNameExists;
		END
	ELSE
		BEGIN
			IF (@ThicknessId=0)
			BEGIN
				INSERT INTO ThicknessMaster
				(
					ThicknessName, IsActive,IsDeleted,CreatedBy,CreatedOn
				)
				VALUES
				(
					@ThicknessName, @IsActive,0,@LoggedInUserId,GETDATE()
				)
				
				SET @Result = SCOPE_IDENTITY();
			END
			ELSE IF(@ThicknessId> 0 and EXISTS(SELECT TOP 1 1 FROM ThicknessMaster WHERE ThicknessId=@ThicknessId))
			BEGIN
				UPDATE ThicknessMaster
				SET ThicknessName=@ThicknessName,
					IsActive=@IsActive,
					ModifiedBy=@LoggedInUserId,
					ModifiedOn=GETDATE()
				WHERE ThicknessId=@ThicknessId

				SET @Result = @ThicknessId;
			END
			ELSE
			BEGIN
				SET @Result=@NoRecordExists
			END
		END
	
	SELECT @Result as Result 
END
GO
/*
	Version : 1.0
	Created Date : 23 AUG 2023
	Execution : EXEC [dbo].[GetThicknessesList]
	Description : Get Thickness from ThicknessMaster
	EXEC [dbo].[GetThicknessesList]  
		@PageSize=10,
	    @PageNo=1,
	    @SortBy='',
	    @OrderBy='',
		@ThicknessName='',
		@IsActive=NULL
*/
CREATE OR ALTER PROCEDURE [dbo].[GetThicknessesList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@ThicknessName VARCHAR(100)=null,
	@IsActive BIT
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @ThicknessName = ISNULL(@ThicknessName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	SET @STR = N'SELECT
					TM.ThicknessId,TM.ThicknessName,TM.IsActive,CreatorEM.EmployeeName As CreatorName,
					TM.CreatedBy,TM.CreatedOn
				FROM ThicknessMaster TM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = TM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (@ThicknessName='''' OR TM.ThicknessName LIKE ''%''+@ThicknessName+''%'')
					AND (@IsActive IS NULL OR TM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @STR,
						N'@ThicknessName VARCHAR(100),@IsActive BIT',
						@ThicknessName,@IsActive
END
GO

CREATE OR ALTER PROCEDURE GetThicknessDetailsById
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		TM.ThicknessId,
		TM.ThicknessName,
		TM.IsActive,
		CreatorEM.EmployeeName As CreatorName,
		TM.CreatedBy,
		TM.CreatedOn
	FROM ThicknessMaster TM WITH(NOLOCK)
	INNER JOIN Users U WITH(NOLOCK)
	 	ON U.UserId = TM.CreatedBy
	LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
		ON CreatorEM.EmployeeId = U.EmployeeId
		  And U.EmployeeId IS NOT NULL
	WHERE ThicknessId = @Id
END
GO
If Object_Id('TileSizeMaster') Is Null
Begin
	CREATE Table TileSizeMaster
	(
		TileSizeId		Int Primary Key Identity(1,1),
		TileSizeName	VarChar(100) Not Null,
		IsActive		Bit NOT NULL Default(1),
		IsDeleted		Bit NOT NULL Default(0),
		CreatedBy		BigInt NOT NULL References Users(UserId),
		CreatedOn		DateTime NOT NULL,
		ModifiedBy		BigInt References Users(UserId),
		ModifiedOn		DateTime
	)
End
GO
CREATE OR ALTER PROCEDURE SaveTileSizeDetails
(
	@TileSizeId BIGINT,
	@TileSizeName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	IF ((@TileSizeId=0 AND 	EXISTS
			(
				SELECT TOP 1 1 
				FROM TileSizeMaster WITH(NOLOCK) 
				WHERE  TileSizeName=@TileSizeName
			)
		 )
		 OR
		(@TileSizeId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM TileSizeMaster WITH(NOLOCK) 
				WHERE  TileSizeName=@TileSizeName and TileSizeId<>@TileSizeId
			)
		))
		BEGIN
			SET @Result=@IsNameExists;
		END
	ELSE
		BEGIN
			IF (@TileSizeId=0)
			BEGIN
				INSERT INTO TileSizeMaster
				(
					TileSizeName, IsActive,IsDeleted,CreatedBy,CreatedOn
				)
				VALUES
				(
					@TileSizeName, @IsActive,0,@LoggedInUserId,GETDATE()
				)
				
				SET @Result = SCOPE_IDENTITY();
			END
			ELSE IF(@TileSizeId> 0 and EXISTS(SELECT TOP 1 1 FROM TileSizeMaster WHERE TileSizeId=@TileSizeId))
			BEGIN
				UPDATE TileSizeMaster
				SET TileSizeName=@TileSizeName,
					IsActive=@IsActive,
					ModifiedBy=@LoggedInUserId,
					ModifiedOn=GETDATE()
				WHERE TileSizeId=@TileSizeId

				SET @Result = @TileSizeId;
			END
			ELSE
			BEGIN
				SET @Result=@NoRecordExists
			END
		END
	
	SELECT @Result as Result 
END
GO
/*
	Version : 1.0
	Created Date : 23 AUG 2023
	Execution : EXEC [dbo].[GetTileSizesList]
	Description : Get TileSize from TileSizeMaster
	EXEC [dbo].[GetTileSizesList]  
		@PageSize=10,
	    @PageNo=1,
	    @SortBy='',
	    @OrderBy='',
		@TileSizeName='',
		@IsActive=NULL
*/
CREATE OR ALTER PROCEDURE [dbo].[GetTileSizesList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@TileSizeName VARCHAR(100)=null,
	@IsActive BIT
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @TileSizeName = ISNULL(@TileSizeName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	SET @STR = N'SELECT
					TM.TileSizeId,TM.TileSizeName,TM.IsActive,CreatorEM.EmployeeName As CreatorName,
					TM.CreatedBy,TM.CreatedOn
				FROM TileSizeMaster TM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = TM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (@TileSizeName='''' OR TM.TileSizeName LIKE ''%''+@TileSizeName+''%'')
					AND (@IsActive IS NULL OR TM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @STR,
						N'@TileSizeName VARCHAR(100),@IsActive BIT',
						@TileSizeName,@IsActive
END
GO

CREATE OR ALTER PROCEDURE GetTileSizeDetailsById
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		TM.TileSizeId,
		TM.TileSizeName,
		TM.IsActive,
		CreatorEM.EmployeeName As CreatorName,
		TM.CreatedBy,
		TM.CreatedOn
	FROM TileSizeMaster TM WITH(NOLOCK)
	INNER JOIN Users U WITH(NOLOCK)
	 	ON U.UserId = TM.CreatedBy
	LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
		ON CreatorEM.EmployeeId = U.EmployeeId
		  And U.EmployeeId IS NOT NULL
	WHERE TileSizeId = @Id
END

GO
