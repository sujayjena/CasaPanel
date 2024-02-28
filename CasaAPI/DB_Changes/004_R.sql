/*
	Description : Get Size from SizeMaster
	EXEC [dbo].[GetSizesList]  
		@PageSize=10,
	    @PageNo=1,
	    @SortBy='',
	    @OrderBy='',
		@ValueForSearch='Emplo',
		@IsActive=NULL,
		@IsExport=NULL
NOTE : PageSize=0 Then all records without pagination will be retried.
*/
CREATE OR ALTER PROCEDURE [dbo].[GetSizesList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@ValueForSearch VARCHAR(100)='',
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @FinalQueryString NVARCHAR(MAX);
	DECLARE @SelectColumns VARCHAR(3000);
	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	IF @IsExport IS NULL AND @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	IF @IsExport IS NULL
	BEGIN
		SET @SelectColumns='SM.SizeId,
							SM.SizeName,
							SM.IsActive,
							CreatorEM.EmployeeName As CreatorName,
							SM.CreatedBy,
							SM.CreatedOn';
	END
	ELSE
	BEGIN
		SET @SelectColumns='SM.SizeName,
							CASE WHEN SM.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,
							CreatorEM.EmployeeName As CreatorName,
							SM.CreatedOn';
	END
	SET @FinalQueryString = N'SELECT
					'+ @SelectColumns +'
				FROM SizeMaster SM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = SM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						AND U.EmployeeId IS NOT NULL
						WHERE (
								@ValueForSearch = '''' OR 
								(SM.SizeName Like ''%''+@ValueForSearch+''%'' OR CreatorEM.EmployeeName Like ''%''+@ValueForSearch+''%''))
								AND (@IsActive IS NULL OR SM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @FinalQueryString,
						N'@ValueForSearch VARCHAR(100),@IsActive BIT',
						@ValueForSearch,@IsActive
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
		@ValueForSearch='',
		@IsActive=NULL,
		@IsExport=NULL
*/

CREATE OR ALTER PROCEDURE [dbo].[GetBrandsList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@ValueForSearch VARCHAR(100)='',
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @FinalQueryString NVARCHAR(MAX);
	DECLARE @SelectColumns VARCHAR(3000);
	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	IF @IsExport IS NULL AND @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END

	IF @IsExport IS NULL
	BEGIN
		SET @SelectColumns='BM.BrandId,
							BM.BrandName,
							BM.IsActive,
							CreatorEM.EmployeeName As CreatorName,
							BM.CreatedBy,
							BM.CreatedOn';
	END
	ELSE
	BEGIN
		SET @SelectColumns='BM.BrandName,
							CASE WHEN SM.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,
							CreatorEM.EmployeeName As CreatorName,
							BM.CreatedOn';
	END
	
	SET @FinalQueryString = N'SELECT
					'+ @SelectColumns +'
				FROM BrandMaster BM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = BM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
							WHERE (
								@ValueForSearch = '''' OR 
								(BM.BrandName Like ''%''+@ValueForSearch+''%'' OR CreatorEM.EmployeeName Like ''%''+@ValueForSearch+''%''))
								AND (@IsActive IS NULL OR BM.IsActive=@IsActive)
				'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @FinalQueryString,
						N'@ValueForSearch VARCHAR(100),@IsActive BIT',
						@ValueForSearch,@IsActive
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
		@ValueForSearch='',
		@IsActive=NULL,
		@IsExport=NULL
*/

CREATE OR ALTER PROCEDURE [dbo].[GetCollectionsList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@ValueForSearch VARCHAR(100)='',
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @FinalQueryString NVARCHAR(MAX);
	DECLARE @SelectColumns VARCHAR(3000);
	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	IF @IsExport IS NULL AND @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END

	IF @IsExport IS NULL
	BEGIN
		SET @SelectColumns='CM.CollectionId,
							CM.CollectionName,
							CM.IsActive,
							CreatorEM.EmployeeName As CreatorName,
							CM.CreatedBy,
							CM.CreatedOn';
	END
	ELSE
	BEGIN
		SET @SelectColumns='CM.CollectionName,
							CASE WHEN CM.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,
							CreatorEM.EmployeeName As CreatorName,
							CM.CreatedOn';
	END
	
	SET @FinalQueryString = N'SELECT
					'+ @SelectColumns +'
				FROM CollectionMaster CM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = CM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
					WHERE (
								@ValueForSearch = '''' OR 
								(CM.CollectionName Like ''%''+@ValueForSearch+''%'' OR CreatorEM.EmployeeName Like ''%''+@ValueForSearch+''%''))
								AND (@IsActive IS NULL OR CM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @FinalQueryString,
						N'@ValueForSearch VARCHAR(100),@IsActive BIT',
						@ValueForSearch,@IsActive
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
		@ValueForSearch='',
		@IsActive=NULL,
		@IsExport=NULL
*/

CREATE OR ALTER PROCEDURE [dbo].[GetCategorysList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@ValueForSearch VARCHAR(100)='',
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @FinalQueryString NVARCHAR(MAX);
	DECLARE @SelectColumns VARCHAR(3000);
	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	IF @IsExport IS NULL AND @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	IF @IsExport IS NULL
	BEGIN
		SET @SelectColumns='CM.CategoryId,
							CM.CategoryName,
							CM.IsActive,
							CreatorEM.EmployeeName As CreatorName,
					        CM.CreatedBy,
							CM.CreatedOn';
	END
	ELSE
	BEGIN
		SET @SelectColumns='CM.CategoryName,
							CASE WHEN CM.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,
							CreatorEM.EmployeeName As CreatorName,
							CM.CreatedOn';
	END

	SET @FinalQueryString = N'SELECT
					'+ @SelectColumns +'
				FROM CategoryMaster CM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = CM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (
						@ValueForSearch = '''' OR 
						(CM.CategoryName Like ''%''+@ValueForSearch+''%'' OR CreatorEM.EmployeeName Like ''%''+@ValueForSearch+''%''))
						AND (@IsActive IS NULL OR CM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @FinalQueryString,
						N'@ValueForSearch VARCHAR(100),@IsActive BIT',
						@ValueForSearch,@IsActive
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
		@ValueForSearch='',
		@IsActive=NULL,
		@IsExport=NULL
*/
CREATE OR ALTER PROCEDURE [dbo].[GetTypesList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@ValueForSearch VARCHAR(100)='',
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @FinalQueryString NVARCHAR(MAX);
	DECLARE @SelectColumns VARCHAR(3000);
	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	IF @IsExport IS NULL AND @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	IF @IsExport IS NULL
	BEGIN
		SET @SelectColumns='TM.TypeId,
							TM.TypeName,
							TM.IsActive,
							CreatorEM.EmployeeName As CreatorName,
							TM.CreatedBy,
							TM.CreatedOn';
	END
	ELSE
	BEGIN
		SET @SelectColumns='TM.TypeName,
							CASE WHEN TM.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,
							CreatorEM.EmployeeName As CreatorName,
							TM.CreatedOn'
	END

	SET @FinalQueryString = N'SELECT
					'+ @SelectColumns +'
				FROM TypeMaster TM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = TM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (
						@ValueForSearch = '''' OR 
						(TM.TypeName Like ''%''+@ValueForSearch+''%'' OR CreatorEM.EmployeeName Like ''%''+@ValueForSearch+''%''))
						AND (@IsActive IS NULL OR TM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @ValueForSearch,
						N'@ValueForSearch VARCHAR(100),@IsActive BIT',
						@ValueForSearch,@IsActive
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
		@ValueForSearch='Emplo',
		@IsActive=NULL,
		@IsExport=NULL
*/
CREATE OR ALTER PROCEDURE [dbo].[GetPunchsList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@ValueForSearch VARCHAR(100)='',
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @FinalQueryString NVARCHAR(MAX);
	DECLARE @SelectColumns VARCHAR(3000);
	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	IF @IsExport IS NULL AND @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	IF @IsExport IS NULL
	BEGIN
		SET @SelectColumns='PM.PunchId,
							PM.PunchName,
							PM.IsActive,
							CreatorEM.EmployeeName As CreatorName,
							PM.CreatedBy,
							PM.CreatedOn';
	END
	ELSE
	BEGIN
		SET @SelectColumns='PM.PunchId,
							CASE WHEN PM.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,
							CreatorEM.EmployeeName As CreatorName,
							PM.CreatedOn';
	END
	SET @FinalQueryString = N'SELECT
					'+ @SelectColumns +'
				FROM PunchMaster PM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = PM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (
						@ValueForSearch = '''' OR 
						(PM.PunchName Like ''%''+@ValueForSearch+''%'' OR CreatorEM.EmployeeName Like ''%''+@ValueForSearch+''%''))
						AND (@IsActive IS NULL OR PM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @FinalQueryString,
						N'@ValueForSearch VARCHAR(100),@IsActive BIT',
						@ValueForSearch,@IsActive
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
		@ValueForSearch='Emplo',
		@IsActive=NULL,
		@IsExport=NULL
*/
CREATE OR ALTER PROCEDURE [dbo].[GetSurfacesList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@ValueForSearch VARCHAR(100)='',
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @FinalQueryString NVARCHAR(MAX);
	DECLARE @SelectColumns VARCHAR(3000);
	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	IF @IsExport IS NULL AND @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	IF @IsExport IS NULL
	BEGIN
		SET @SelectColumns='SM.SurfaceId,
							SM.SurfaceName,
							SM.IsActive,
							CreatorEM.EmployeeName As CreatorName,
							SM.CreatedBy,
							SM.CreatedOn';
	END
	ELSE
	BEGIN
		SET @SelectColumns='SM.SurfaceId,
							CASE WHEN SM.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,
							CreatorEM.EmployeeName As CreatorName,
							SM.CreatedOn';
	END

	SET @FinalQueryString = N'SELECT
					'+ @SelectColumns +'
				FROM SurfaceMaster SM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = SM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (
						@ValueForSearch = '''' OR 
						(SM.SurfaceName Like ''%''+@ValueForSearch+''%'' OR CreatorEM.EmployeeName Like ''%''+@ValueForSearch+''%''))
						AND (@IsActive IS NULL OR SM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @FinalQueryString,
						N'@ValueForSearch VARCHAR(100),@IsActive BIT',
						@ValueForSearch,@IsActive
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
		@ValueForSearch='Emplo',
		@IsActive=NULL,
		@IsExport=NULL
*/
CREATE OR ALTER PROCEDURE [dbo].[GetThicknessesList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@ValueForSearch VARCHAR(100)='',
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @FinalQueryString NVARCHAR(MAX);
	DECLARE @SelectColumns VARCHAR(3000);
	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	IF @IsExport IS NULL AND @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	
	IF @IsExport IS NULL
	BEGIN
		SET @SelectColumns='TM.ThicknessId,
							TM.ThicknessName,
							TM.IsActive,
							CreatorEM.EmployeeName As CreatorName,
							TM.CreatedBy,
							TM.CreatedOn';
	END
	ELSE
	BEGIN
		SET @SelectColumns='TM.ThicknessName,
							CASE WHEN TM.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,
							CreatorEM.EmployeeName As CreatorName,
							TM.CreatedOn';
	END

	SET @FinalQueryString = N'SELECT
						'+ @SelectColumns +'
				FROM ThicknessMaster TM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = TM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (
						@ValueForSearch = '''' OR 
						(TM.ThicknessName Like ''%''+@ValueForSearch+''%'' OR CreatorEM.EmployeeName Like ''%''+@ValueForSearch+''%''))
						AND (@IsActive IS NULL OR TM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @FinalQueryString,
						N'@ValueForSearch VARCHAR(100),@IsActive BIT',
						@ValueForSearch,@IsActive
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
		@ValueForSearch='Emplo',
		@IsActive=NULL,
		@IsExport=NULL
*/
CREATE OR ALTER PROCEDURE [dbo].[GetTileSizesList]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@ValueForSearch VARCHAR(100)='',
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN

 SET NOCOUNT ON;

	DECLARE @FinalQueryString NVARCHAR(MAX);
	DECLARE @SelectColumns VARCHAR(3000);
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

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by CreatedOn DESC ';
		END
	
	IF @IsExport IS NULL
	BEGIN
		SET @SelectColumns='TM.TileSizeId,
							TM.TileSizeName,
							TM.IsActive,
							CreatorEM.EmployeeName As CreatorName,
							TM.CreatedBy,
							TM.CreatedOn';
	END
	ELSE
	BEGIN
		SET @SelectColumns='TM.TileSizeName,
							CASE WHEN TM.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,
							CreatorEM.EmployeeName As CreatorName,
							TM.CreatedOn';
	END
	SET @FinalQueryString = N'SELECT
					'+ @SelectColumns +'
				FROM TileSizeMaster TM WITH(NOLOCK)
				INNER JOIN Users U WITH(NOLOCK)
					ON U.UserId = TM.CreatedBy
				LEFT JOIN EmployeeMaster CreatorEM WITH(NOLOCK)
					ON CreatorEM.EmployeeId = U.EmployeeId
						And U.EmployeeId IS NOT NULL
				WHERE (
						@ValueForSearch = '''' OR 
						(TM.TileSizeName Like ''%''+@ValueForSearch+''%'' OR CreatorEM.EmployeeName Like ''%''+@ValueForSearch+''%''))
						AND (@IsActive IS NULL OR TM.IsActive=@IsActive)
			'+@OrderByQuery+' '+@PaginationQuery+'	'
			
	--PRINT @STR;

	exec sp_executesql @FinalQueryString,
						N'@ValueForSearch VARCHAR(100),@IsActive BIT',
						@ValueForSearch,@IsActive
END