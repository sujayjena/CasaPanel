CREATE OR ALTER PROCEDURE [dbo].[GetAreaDetailsById]
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	 SELECT AM.AreaId,AM.CityId,CM.CityName,AM.AreaName,AM.IsActive
			FROM AreaMaster AM WITH(NOLOCK)
			INNER JOIN CityMaster CM WITH(NOLOCK) ON CM.CityId=AM.CityId
	WHERE AreaId = @Id
END

GO

CREATE OR ALTER  PROCEDURE [dbo].[GetAreas]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@AreaName VARCHAR(100)=null,
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX);
	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @IsExport IS NULL AND  @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @AreaName = ISNULL(@AreaName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by AreaId DESC ';
		END
		

	SET @STR = N' WITH CTE AS (
				SELECT
					AM.AreaId,AM.AreaName,AM.IsActive,ST.StateId,ST.StateName, RM.RegionId,RM.RegionName, 
					D.DistrictId,D.DistrictName,CM.CityId, CM.CityName,
					CreatorEM.EmployeeName As CreatorName,
					AM.CreatedBy,
					AM.CreatedOn
				FROM AreaMaster AM WITH(NOLOCK)
				INNER JOIN CityMaster CM WITH(NOLOCK)
					ON CM.CityId=AM.CityId
				INNER JOIN DistrictMaster D WITH(NOLOCK)
					ON CM.DistrictId=D.DistrictId
				INNER JOIN RegionMaster RM  WITH(NOLOCK)
					ON RM.RegionId = D.RegionId
				INNER JOIN StateMaster ST  WITH(NOLOCK)
					ON ST.StateId=RM.StateId
				Inner Join Users U With(NoLock)
					On U.UserId = AM.CreatedBy
				Inner Join EmployeeMaster CreatorEM With(NoLock)
					On CreatorEM.EmployeeId = U.EmployeeId And U.EmployeeId Is Not Null
				WHERE (@AreaName='''' OR AM.AreaName like ''%''+@AreaName+''%'')
					and (@IsActive IS NULL OR AM.IsActive=@IsActive)) 
						SELECT *, (SELECT COUNT(*) FROM CTE) AS TotalRecords
						FROM CTE
			'+@OrderByQuery+' '+@PaginationQuery+'	'

	--PRINT @STR;

	exec sp_executesql @STR,
						N'@AreaName VARCHAR(100),@IsActive BIT',
						@AreaName,@IsActive
END

GO

CREATE OR ALTER  PROCEDURE [dbo].[GetAreasForSelectList]
	@CityId BIGINT,
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		AreaId AS [Value],
		AreaName AS [Text]
	FROM AreaMaster WITH(NOLOCK)
	WHERE CityId = @CityId AND
		(@IsActive IS NULL OR IsActive = @IsActive)
END

GO

CREATE OR ALTER   Procedure [dbo].[GetCities]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@CityName VARCHAR(100)=null,
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)='';

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @IsExport IS NULL AND  @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @CityName = ISNULL(@CityName,'');

	IF @SortBy <> ''
	BEGIN
		SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
	END
	ELSE
	BEGIN
		SET @OrderByQuery='ORDER by CityId DESC ';
	END

	SET @STR = N' WITH CTE AS (
				SELECT
					CM.CityId, CM.CityName, CM.IsActive,
					CM.DistrictId,DM.DistrictName,
					RM.RegionId,RM.RegionName,
					ST.StateId,ST.StateName,
					CreatorEM.EmployeeName As CreatorName,
					CM.CreatedBy,
					CM.CreatedOn
				FROM CityMaster CM WITH(NOLOCK)
				INNER JOIN DistrictMaster DM  WITH(NOLOCK)
					ON DM.DistrictId=CM.DistrictId
				INNER JOIN RegionMaster RM  WITH(NOLOCK)
					ON DM.RegionId=RM.RegionId
				INNER JOIN StateMaster ST  WITH(NOLOCK)
					ON ST.StateId=RM.StateId
				Inner Join Users U With(NoLock)
					On U.UserId = CM.CreatedBy
				INNER Join EmployeeMaster CreatorEM With(NoLock)
					On CreatorEM.EmployeeId = U.EmployeeId And U.EmployeeId Is Not Null
				WHERE (@CityName='''' OR CM.CityName like ''%''+@CityName+''%'')
					And (@IsActive IS NULL OR CM.IsActive=@IsActive)) 
						SELECT *, (SELECT COUNT(*) FROM CTE) AS TotalRecords
						FROM CTE
			'+@OrderByQuery+' '+@PaginationQuery+'	'

	------PRINT @STR;

	exec sp_executesql @STR,
						N'@CityName VARCHAR(100),@IsActive BIT',
						@CityName,@IsActive
END

GO

CREATE OR ALTER PROCEDURE [dbo].[GetCityDetailsById]
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CM.CityId,CM.DistrictId,CM.CityName,DM.DistrictName,CM.IsActive
					FROM CityMaster CM WITH(NOLOCK)
				INNER JOIN DistrictMaster DM  WITH(NOLOCK) ON DM.DistrictId=CM.DistrictId
	WHERE CM.CityId = @Id
END

GO

CREATE OR ALTER   PROCEDURE [dbo].[GetCityForSelectList]
	@DistrictId BIGINT,
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CityId AS [Value], CityName AS [Text]
	FROM CityMaster WITH(NOLOCK)
	WHERE DistrictId = @DistrictId AND
		(@IsActive IS NULL OR IsActive = @IsActive)
END

GO

CREATE OR ALTER PROCEDURE [dbo].[GetDistrictDetailsById]
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT D.DistrictId,D.RegionId,RM.RegionName,D.DistrictName,D.IsActive
					FROM DistrictMaster D WITH(NOLOCK)
				INNER JOIN RegionMaster RM  WITH(NOLOCK) ON RM.RegionId=D.RegionId
	WHERE D.DistrictId = @Id
END

GO

CREATE OR ALTER   Procedure [dbo].[GetDistricts]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@DistrictName VARCHAR(100)=null,
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX)='';

	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	if @IsExport IS NULL AND @PageSize>0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @DistrictName = ISNULL(@DistrictName,'');

	IF @SortBy <> ''
	BEGIN
		SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
	END
	ELSE
	BEGIN
		SET @OrderByQuery='ORDER by DistrictId DESC ';
	END

	SET @STR = N' WITH CTE AS (
				SELECT
					D.DistrictId,D.DistrictName,D.IsActive,
					D.RegionId,RM.RegionName,
					ST.StateId,ST.StateName,
					CreatorEM.EmployeeName As CreatorName,
					D.CreatedBy,
					D.CreatedOn
				FROM DistrictMaster D WITH(NOLOCK)
				INNER JOIN RegionMaster RM  WITH(NOLOCK)
					ON RM.RegionId=D.RegionId
				INNER JOIN StateMaster ST  WITH(NOLOCK)
					ON ST.StateId=RM.StateId
				Inner Join Users U With(NoLock)
					On U.UserId = D.CreatedBy
				INNER Join EmployeeMaster CreatorEM With(NoLock)
					On CreatorEM.EmployeeId = U.EmployeeId And U.EmployeeId Is Not Null
				WHERE (@DistrictName='''' OR D.DistrictName like ''%''+@DistrictName+''%'')
					And (@IsActive IS NULL OR D.IsActive=@IsActive)) 
						SELECT *, (SELECT COUNT(*) FROM CTE) AS TotalRecords
						FROM CTE
			'+@OrderByQuery+' '+@PaginationQuery+'	'

	------PRINT @STR;

	exec sp_executesql @STR,
						N'@DistrictName VARCHAR(100),@IsActive BIT',
						@DistrictName,@IsActive
END

GO

CREATE OR ALTER  PROCEDURE [dbo].[GetDistrictsForSelectList]
	@RegionId BIGINT,
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DistrictId AS [Value], DistrictName AS [Text]
	FROM DistrictMaster WITH(NOLOCK)
	WHERE RegionId = @RegionId AND
		(@IsActive IS NULL OR IsActive = @IsActive)
END

GO

CREATE OR ALTER PROCEDURE [dbo].[GetRegionDetailsById]
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	 SELECT R.RegionId,R.StateId,M.StateName,R.RegionName,R.IsActive
					FROM RegionMaster R WITH(NOLOCK)
					INNER JOIN StateMaster M WITH(NOLOCK) ON M.StateId=R.StateId
	WHERE RegionId = @Id
END

GO

CREATE OR ALTER  Procedure [dbo].[GetRegions]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@RegionName VARCHAR(100)=null,
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @STR NVARCHAR(MAX);
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
	SET @RegionName = ISNULL(@RegionName,'');

	IF @SortBy <> ''
	BEGIN
		SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
	END
	ELSE
	BEGIN
		SET @OrderByQuery='ORDER by RegionId DESC ';
	END
	
	SET @STR = N' WITH CTE AS (
			SELECT
					R.RegionId, R.StateId, M.StateName, R.RegionName, R.IsActive,
					CreatorEM.EmployeeName As CreatorName,
					R.CreatedBy,
					R.CreatedOn
				From RegionMaster R WITH(NOLOCK)
				Inner Join StateMaster M WITH(NOLOCK)
					ON M.StateId=R.StateId
				Inner Join Users U With(NoLock)
					On U.UserId = R.CreatedBy
				INNER Join EmployeeMaster CreatorEM With(NoLock)
					On CreatorEM.EmployeeId = U.EmployeeId And U.EmployeeId Is Not Null
				Where (@RegionName='''' OR R.RegionName like ''%''+@RegionName+''%'')
					and (@IsActive IS NULL OR R.IsActive=@IsActive)) 
						SELECT *, (SELECT COUNT(*) FROM CTE) AS TotalRecords
						FROM CTE
			'+@OrderByQuery+' '+@PaginationQuery+'	'

	-- PRINT @STR;

	exec sp_executesql @STR,
						N'@RegionName VARCHAR(100),@IsActive BIT',
						@RegionName,@IsActive
END

GO

CREATE OR ALTER PROCEDURE [dbo].[GetRegionsForSelectList]
	@StateId BIGINT,
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT RegionId AS [Value], RegionName AS [Text]
	FROM RegionMaster WITH(NOLOCK)
	WHERE StateId = @StateId AND
		(@IsActive IS NULL OR IsActive = @IsActive)
END

GO

CREATE OR ALTER PROCEDURE [dbo].[GetStateDetailsById]
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	 SELECT StateId,StateName,IsActive
					FROM StateMaster WITH(NOLOCK)
	WHERE StateId = @Id
END

GO

CREATE OR ALTER  Procedure [dbo].[GetStates]
(
    @PageSize INT,
    @PageNo INT,
    @SortBy VARCHAR(50),
    @OrderBy VARCHAR(4),
	@StateName VARCHAR(100)=null,
	@IsActive BIT,
	@IsExport BIT=NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @STR NVARCHAR(MAX);
	DECLARE @Offset INT, @RowCount INT;
	DECLARE @OrderSortByConcate VARCHAR(100);
	DECLARE @OrderByQuery VARCHAR(1000)='';
	DECLARE @PaginationQuery VARCHAR(100)='';

	If @IsExport IS NULL AND @PageSize > 0
	BEGIN
		SET @Offset = (@PageNo - 1) * @PageSize;
		SET @RowCount = @PageSize * @PageNo;
		SET @PaginationQuery='OFFSET '+CAST(@Offset as VARCHAR(5))+'  ROWS
								 FETCH NEXT '+CAST(@RowCount as VARCHAR(5))+' ROWS ONLY';
	END
	
	SET @OrderSortByConcate= @SortBy + ' ' + @OrderBy;
	SET @StateName = ISNULL(@StateName,'');

	IF @SortBy <> ''
		BEGIN
			SET @OrderByQuery='ORDER by '+@SortBy+' '+@OrderBy+' ';
		END
	ELSE 
		BEGIN
			SET @OrderByQuery='ORDER by StateId DESC ';
		END

	SET @STR = N' WITH CTE AS (
			SELECT
					SM.StateId, SM.StateName, SM.IsActive,
					CreatorEM.EmployeeName As CreatorName,
					SM.CreatedBy,
					SM.CreatedOn
				From StateMaster SM WITH(NOLOCK)
				Inner Join Users U With(NoLock)
					On U.UserId = SM.CreatedBy
				INNER Join EmployeeMaster CreatorEM With(NoLock)
					On CreatorEM.EmployeeId = U.EmployeeId And U.EmployeeId Is Not Null
				Where (@StateName='''' OR SM.StateName like ''%''+@StateName+''%'')
					And (@IsActive IS NULL OR SM.IsActive=@IsActive)) 
						SELECT *, (SELECT COUNT(*) FROM CTE) AS TotalRecords
						FROM CTE
			'+@OrderByQuery+' '+@PaginationQuery+'	'

	-- PRINT @STR;

	Exec sp_executesql @STR,
						N'@StateName VARCHAR(100),@IsActive BIT',
						@StateName,@IsActive
END

GO

CREATE OR ALTER  PROCEDURE [dbo].[GetStatesForSelectList]
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT StateId AS [Value], StateName AS [Text]
	FROM StateMaster WITH(NOLOCK)
	WHERE @IsActive IS NULL OR IsActive = @IsActive
END

GO

Create Or ALTER   Procedure [dbo].[SaveStateDetails]
(
	@StateId BIGINT,
	@StateName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	If (
		(@StateId=0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM StateMaster WITH(NOLOCK) 
				WHERE  StateName=@StateName
			)
		)
		OR
		(@StateId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM StateMaster WITH(NOLOCK) 
				WHERE  StateName=@StateName and StateId<>@StateId
			)
		))
	BEGIN
		SET @Result=@IsNameExists;
	END
	ELSE
	BEGIN
		IF (@StateId=0)
		BEGIN
			Insert into StateMaster(StateName, IsActive,CreatedBy,CreatedOn)
			Values(@StateName, @IsActive,@LoggedInUserId,GETDATE())
			SET @Result = SCOPE_IDENTITY();
		END
		ELSE IF(@StateId> 0 and EXISTS(SELECT TOP 1 1 FROM StateMaster WHERE StateId=@StateId))
		BEGIN
			UPDATE StateMaster
			SET StateName=@StateName,IsActive=@IsActive,ModifiedBy=@LoggedInUserId, ModifiedOn=GETDATE()
			WHERE StateId=@StateId
			SET @Result = @StateId;
		END
		ELSE
		BEGIN
			SET @Result=@NoRecordExists
		END
	END
	
	SELECT @Result as Result
END

GO

CREATE OR ALTER  Procedure [dbo].[SaveAreaDetails]
(
	@AreaId BIGINT,
	@CityId BIGINT,
	@AreaName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	If (
		(@AreaId=0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM AreaMaster WITH(NOLOCK) 
				WHERE  AreaName=@AreaName
			)
		)
		OR
		(@AreaId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM AreaMaster WITH(NOLOCK) 
				WHERE  AreaName=@AreaName and AreaId<>@AreaId
			)
		))
	BEGIN
		SET @Result=@IsNameExists;
	END
	ELSE
	BEGIN
		IF (@AreaId=0)
		BEGIN
			Insert into AreaMaster(CityId, AreaName, IsActive,CreatedBy,CreatedOn)
			Values(@CityId,@AreaName, @IsActive,@LoggedInUserId,GETDATE())
			SET @Result = SCOPE_IDENTITY();
		END
		ELSE IF(@AreaId> 0 and EXISTS(SELECT TOP 1 1 FROM AreaMaster WHERE AreaId=@AreaId))
		BEGIN
			UPDATE AreaMaster
			SET CityId=@CityId, AreaName=@AreaName,IsActive=@IsActive,
				ModifiedBy=@LoggedInUserId, ModifiedOn=GETDATE()
			WHERE AreaId=@AreaId
			SET @Result = @AreaId;
		END
		ELSE
		BEGIN
			SET @Result=@NoRecordExists
		END
	END
	
	SELECT @Result as Result
END

GO

CREATE OR ALTER  Procedure [dbo].[SaveCityDetails]
(
	@CityId BIGINT,
	@DistrictId BIGINT,
	@CityName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	If (
		(@CityId=0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM CityMaster WITH(NOLOCK) 
				WHERE  CityId=@CityId
			)
		)
		OR
		(@CityId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM CityMaster WITH(NOLOCK) 
				WHERE  CityName=@CityName and CityId<>@CityId
			)
		))
	BEGIN
		SET @Result=@IsNameExists;
	END
	ELSE
	BEGIN
		IF (@CityId=0)
		BEGIN
			Insert into CityMaster(DistrictId, CityName, IsActive,CreatedBy,CreatedOn)
			Values(@DistrictId,@CityName, @IsActive,@LoggedInUserId,GETDATE())
			SET @Result = SCOPE_IDENTITY();
		END
		ELSE IF(@CityId> 0 and EXISTS(SELECT TOP 1 1 FROM CityMaster WHERE CityId=@CityId))
		BEGIN
			UPDATE CityMaster
			SET DistrictId=@DistrictId, CityName=@CityName,IsActive=@IsActive,
				ModifiedBy=@LoggedInUserId, ModifiedOn=GETDATE()
			WHERE CityId=@CityId
			SET @Result = @CityId;
		END
		ELSE
		BEGIN
			SET @Result=@NoRecordExists
		END
	END
	
	SELECT @Result as Result
END

GO

CREATE OR ALTER  Procedure [dbo].[SaveDistrictDetails]
(
	@DistrictId BIGINT,
	@RegionId BIGINT,
	@DistrictName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	If (
		(@DistrictId=0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM DistrictMaster WITH(NOLOCK) 
				WHERE  DistrictName=@DistrictName
			)
		)
		OR
		(@DistrictId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM DistrictMaster WITH(NOLOCK) 
				WHERE  DistrictName=@DistrictName and DistrictId<>@DistrictId
			)
		))
	BEGIN
		SET @Result=@IsNameExists;
	END
	ELSE
	BEGIN
		IF (@DistrictId=0)
		BEGIN
			Insert into DistrictMaster(RegionId, DistrictName, IsActive,CreatedBy,CreatedOn)
			Values(@RegionId,@DistrictName, @IsActive,@LoggedInUserId,GETDATE())
			SET @Result = SCOPE_IDENTITY();
		END
		ELSE IF(@DistrictId> 0 and EXISTS(SELECT TOP 1 1 FROM DistrictMaster WHERE DistrictId=@DistrictId))
		BEGIN
			UPDATE DistrictMaster
			SET RegionId=@RegionId, DistrictName=@DistrictName,IsActive=@IsActive,
				ModifiedBy=@LoggedInUserId, ModifiedOn=GETDATE()
			WHERE DistrictId=@DistrictId
			SET @Result = @DistrictId;
		END
		ELSE
		BEGIN
			SET @Result=@NoRecordExists
		END
	END
	
	SELECT @Result as Result
END

GO

CREATE OR ALTER Procedure [dbo].[SaveRegionDetails]
(
	@RegionId BIGINT,
	@StateId BIGINT,
	@RegionName VARCHAR(100),
	@IsActive BIT,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	If (
		(@RegionId=0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM RegionMaster WITH(NOLOCK) 
				WHERE  RegionName=@RegionName
			)
		)
		OR
		(@RegionId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM RegionMaster WITH(NOLOCK) 
				WHERE  RegionName=@RegionName and RegionId<>@RegionId
			)
		))
	BEGIN
		SET @Result=@IsNameExists;
	END
	ELSE
	BEGIN
		IF (@RegionId=0)
		BEGIN
			Insert into RegionMaster(StateId, RegionName, IsActive,CreatedBy,CreatedOn)
			Values(@StateId,@RegionName, @IsActive,@LoggedInUserId,GETDATE())
			SET @Result = SCOPE_IDENTITY();
		END
		ELSE IF(@RegionId> 0 and EXISTS(SELECT TOP 1 1 FROM RegionMaster WHERE RegionId=@RegionId))
		BEGIN
			UPDATE RegionMaster
			SET StateId=@StateId, RegionName=@RegionName,IsActive=@IsActive,
				ModifiedBy=@LoggedInUserId, ModifiedOn=GETDATE()
			WHERE RegionId=@RegionId
			SET @Result = @RegionId;
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
EXEC SaveImportAreaDetails 
'<ArrayOfImportedAreaDetails>
  <ImportedAreaDetails>
    <AreaName>Ring Road11155</AreaName>
	<DistrictName>Test City</DistrictName>
    <DistrictName>Rajkot</DistrictName>
    <RegionName>North</RegionName>
    <StateName>Maharastra</StateName>
    <IsActive>True</IsActive>
  </ImportedAreaDetails>
</ArrayOfImportedAreaDetails>',1
*/
CREATE OR ALTER   Procedure[dbo].[SaveImportAreaDetails]
(
	@XmlAreaData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempAreaDetail TABLE
	(
		StateName VARCHAR(100),
		RegionName VARCHAR(100),
		DistrictName VARCHAR(100),
		CityName VARCHAR(100),
		AreaName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempAreaDetail(StateName,RegionName,DistrictName,CityName,AreaName, IsActive,IsValid)
	SELECT
		StateName = T.Item.value('StateName[1]', 'varchar(100)'),
		RegionName = T.Item.value('RegionName[1]', 'varchar(100)'),
		DistrictName = T.Item.value('DistrictName[1]', 'varchar(100)'),
		CityName = T.Item.value('CityName[1]', 'varchar(100)'),
		AreaName = T.Item.value('AreaName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1
		FROM
	 @XmlAreaData.nodes('/ArrayOfImportedAreaDetails/ImportedAreaDetails') AS T(Item)

	-- 3. Validation of records
	Update @tempAreaDetail
	Set IsValid = 0,
		ValidationMessage = 'Area Name is invalid'
	Where RTRIM(LTRIM(IsNull(AreaName,''))) Not Like '%[a-zA-Z ]%' AND IsValid=1

	Update @tempAreaDetail
	Set IsValid = 0,
		ValidationMessage = 'IsActive value is invalid'
	Where RTRIM(LTRIM(IsNull(IsActive,''))) Not IN ('YES','NO') AND IsValid=1

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Area Name already Exists'
	FROM @tempAreaDetail T
	INNER JOIN  AreaMaster AM ON AM.AreaName = T.AreaName and T.IsValid=1

	

	  UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'State Name is not exists'
	FROM @tempAreaDetail T
	LEFT JOIN StateMaster ST ON ST.StateName=T.StateName
	 WHERE ST.StateName IS NULL AND  T.IsValid=1

	   UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Region Name is not exists'
	FROM @tempAreaDetail T
	LEFT JOIN RegionMaster RM ON RM.RegionName=T.RegionName
	LEFT JOIN StateMaster ST ON ST.StateId=RM.StateId AND ST.StateName=T.StateName
	 WHERE RM.RegionName IS NULL AND T.IsValid=1

	 UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'District Name is not exists'
	 FROM @tempAreaDetail T
	LEFT JOIN DistrictMaster DM ON  T.DistrictName = DM.DistrictName  
	LEFT JOIN RegionMaster RM ON DM.RegionId=RM.RegionId AND T.RegionName=RM.RegionName
	LEFT JOIN StateMaster ST ON ST.StateId=RM.StateId AND ST.StateName=T.StateName
	 WHERE DM.DistrictName IS NULL  and T.IsValid=1

	 UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'City Name is not exists'
	 FROM @tempAreaDetail T
	LEFT JOIN CityMaster CM ON T.CityName = CM.CityName
	LEFT JOIN DistrictMaster DM ON  T.DistrictName = DM.DistrictName  
	LEFT JOIN RegionMaster RM ON DM.RegionId=RM.RegionId AND T.RegionName=RM.RegionName
	LEFT JOIN StateMaster ST ON ST.StateId=RM.StateId AND ST.StateName=T.StateName
	 WHERE CM.CityName IS NULL  and T.IsValid=1

	 Insert into AreaMaster(CityId,AreaName, IsActive,CreatedBy,CreatedOn)
			select CM.CityId,T.AreaName,
			CASE WHEN T.IsActive='YES' THEN 1 ELSE 0 END
			,@LoggedInUserId,GETDATE() 
			from @tempAreaDetail T
			INNER JOIN CityMaster CM ON CM.CityName=T.CityName
			Where IsValid = 1


	-- 5. Returning Invalid records
	Select StateName,RegionName,DistrictName,CityName,AreaName,IsActive,ValidationMessage
	From @tempAreaDetail
	Where IsValid = 0;
END

GO

CREATE OR ALTER Procedure[dbo].[SaveImportCityDetails]
(
	@XmlDistrictData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempCityDetail TABLE
	(
		StateName VARCHAR(100),
		RegionName VARCHAR(100),
		DistrictName VARCHAR(100),
		CityName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempCityDetail(StateName,RegionName,DistrictName,CityName,IsActive,IsValid)
	SELECT
		StateName = T.Item.value('StateName[1]', 'varchar(100)'),
		RegionName = T.Item.value('RegionName[1]', 'varchar(100)'),
		DistrictName = T.Item.value('DistrictName[1]', 'varchar(100)'),
		CityName = T.Item.value('CityName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1
		FROM
	 @XmlDistrictData.nodes('/ArrayOfImportedCityDetails/ImportedCityDetails') AS T(Item)

	
	-- 3. Validation of records
	Update @tempCityDetail
	Set IsValid = 0,
		ValidationMessage = 'City Name is invalid'
	Where RTRIM(LTRIM(IsNull(CityName,''))) Not Like '%[a-zA-Z ]%' AND IsValid=1

	Update @tempCityDetail
	Set IsValid = 0,
		ValidationMessage = 'IsActive value is invalid'
	Where RTRIM(LTRIM(IsNull(IsActive,''))) Not IN ('YES','NO') AND IsValid=1

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'City Name already Exists'
	FROM @tempCityDetail T
	INNER JOIN  CityMaster CM ON CM.CityName = T.CityName and T.IsValid=1

	
	  UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'State Name is not exists'
	FROM @tempCityDetail T
	LEFT JOIN StateMaster ST ON ST.StateName=T.StateName
	 WHERE ST.StateName IS NULL AND  T.IsValid=1

	   UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Region Name is not exists'
	FROM @tempCityDetail T
	LEFT JOIN RegionMaster RM ON RM.RegionName=T.RegionName
	LEFT JOIN StateMaster ST ON ST.StateId=RM.StateId AND ST.StateName=T.StateName
	 WHERE RM.RegionName IS NULL AND T.IsValid=1

	    UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'District Name is not exists'
	FROM @tempCityDetail T
	LEFT JOIN DistrictMaster DM ON DM.DistrictName=T.DistrictName
	LEFT JOIN RegionMaster RM ON RM.RegionName=T.RegionName
	LEFT JOIN StateMaster ST ON ST.StateId=RM.StateId AND ST.StateName=T.StateName
	 WHERE DM.DistrictName IS NULL AND T.IsValid=1

	 Insert into CityMaster(DistrictId,CityName, IsActive,CreatedBy,CreatedOn)
			select DM.DistrictId,T.CityName,
			CASE WHEN T.IsActive='YES' THEN 1 ELSE 0 END
			,@LoggedInUserId,GETDATE() 
			from @tempCityDetail T
			INNER JOIN DistrictMaster DM ON DM.DistrictName = T.DistrictName
			Where IsValid = 1


	-- 5. Returning Invalid records
	Select StateName,RegionName,DistrictName,CityName,IsActive,ValidationMessage
	From @tempCityDetail
	Where IsValid = 0;
END

GO

CREATE OR ALTER  Procedure[dbo].[SaveImportDistrictDetails]
(
	@XmlDistrictData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempDistrictDetail TABLE
	(
		StateName VARCHAR(100),
		RegionName VARCHAR(100),
		DistrictName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempDistrictDetail(StateName,RegionName,DistrictName,IsActive,IsValid)
	SELECT
		StateName = T.Item.value('StateName[1]', 'varchar(100)'),
		RegionName = T.Item.value('RegionName[1]', 'varchar(100)'),
		DistrictName = T.Item.value('DistrictName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1
		FROM
	 @XmlDistrictData.nodes('/ArrayOfImportedDistrictDetails/ImportedDistrictDetails') AS T(Item)

	
	-- 3. Validation of records
	Update @tempDistrictDetail
	Set IsValid = 0,
		ValidationMessage = 'District Name is invalid'
	Where RTRIM(LTRIM(IsNull(DistrictName,''))) Not Like '%[a-zA-Z ]%' AND IsValid=1

	Update @tempDistrictDetail
	Set IsValid = 0,
		ValidationMessage = 'IsActive value is invalid'
	Where RTRIM(LTRIM(IsNull(IsActive,''))) Not IN ('YES','NO') AND IsValid=1

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'District Name already Exists'
	FROM @tempDistrictDetail T
	INNER JOIN  DistrictMaster DM ON DM.DistrictName = T.DistrictName and T.IsValid=1

	
	  UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'State Name is not exists'
	FROM @tempDistrictDetail T
	LEFT JOIN StateMaster ST ON ST.StateName=T.StateName
	 WHERE ST.StateName IS NULL AND  T.IsValid=1

	   UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Region Name is not exists'
	FROM @tempDistrictDetail T
	LEFT JOIN RegionMaster RM ON RM.RegionName=T.RegionName
	LEFT JOIN StateMaster ST ON ST.StateId=RM.StateId AND ST.StateName=T.StateName
	 WHERE RM.RegionName IS NULL AND T.IsValid=1

	 Insert into DistrictMaster(RegionId,DistrictName, IsActive,CreatedBy,CreatedOn)
			select RG.RegionId,T.DistrictName,
			CASE WHEN T.IsActive='YES' THEN 1 ELSE 0 END
			,@LoggedInUserId,GETDATE() 
			from @tempDistrictDetail T
			INNER JOIN RegionMaster RG ON RG.RegionName=T.RegionName
			--INNER JOIN StateMaster ST on ST.StateName=T.StateName
			Where IsValid = 1


	-- 5. Returning Invalid records
	Select StateName,RegionName,DistrictName,IsActive,ValidationMessage
	From @tempDistrictDetail
	Where IsValid = 0;
END

GO

CREATE OR ALTER  Procedure[dbo].[SaveImportRegionDetails]
(
	@XmlRegionData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempRegionDetail TABLE
	(
		StateName VARCHAR(100),
		RegionName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempRegionDetail(StateName,RegionName,IsActive,IsValid)
	SELECT
		StateName = T.Item.value('StateName[1]', 'varchar(100)'),
		RegionName = T.Item.value('RegionName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1
		FROM
	 @XmlRegionData.nodes('/ArrayOfImportedRegionDetails/ImportedRegionDetails') AS T(Item)

	 --3. Validation of records
	Update @tempRegionDetail
	Set IsValid = 0,
		ValidationMessage = 'Region Name is invalid'
	Where RTRIM(LTRIM(IsNull(RegionName,''))) Not Like '%[a-zA-Z ]%' AND IsValid=1

	Update @tempRegionDetail
	Set IsValid = 0,
		ValidationMessage = 'IsActive value is invalid'
	Where RTRIM(LTRIM(IsNull(IsActive,''))) Not IN ('YES','NO') AND IsValid=1

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Region Name already Exists'
	FROM @tempRegionDetail T
	INNER JOIN  RegionMaster PM ON PM.RegionName = T.RegionName and T.IsValid=1

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'State Name is not exists'
	FROM @tempRegionDetail T
	LEFT JOIN StateMaster ST ON ST.StateName = T.StateName
	 WHERE  ST.StateName IS NULL  and T.IsValid=1

	 Insert into RegionMaster(StateId,RegionName, IsActive,CreatedBy,CreatedOn)
			select ST.StateId,RegionName,
			CASE WHEN T.IsActive='YES' THEN 1 ELSE 0 END
			,@LoggedInUserId,GETDATE() from @tempRegionDetail T
			INNER JOIN StateMaster ST on ST.StateName=T.StateName
			Where IsValid = 1


	-- 5. Returning Invalid records
	Select StateName,RegionName,IsActive,ValidationMessage
	From @tempRegionDetail
	Where IsValid = 0;
END

GO

CREATE OR ALTER  Procedure[dbo].[SaveImportStateDetails]
(
	@XmlStateData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @AddressId BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	DECLARE @tempStateDetail TABLE
	(
		StateName VARCHAR(100),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempStateDetail(StateName,IsActive,IsValid)
	SELECT
		StateName = T.Item.value('StateName[1]', 'varchar(100)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1
		FROM
	 @XmlStateData.nodes('/ArrayOfImportedStateDetails/ImportedStateDetails') AS T(Item)

	
	-- 3. Validation of records
	Update @tempStateDetail
	Set IsValid = 0,
		ValidationMessage = 'State Name is invalid'
	Where RTRIM(LTRIM(IsNull(StateName,''))) Not Like '%[a-zA-Z ]%' AND IsValid=1

	Update @tempStateDetail
	Set IsValid = 0,
		ValidationMessage = 'IsActive value is invalid'
	Where RTRIM(LTRIM(IsNull(IsActive,''))) Not IN ('YES','NO') AND IsValid=1

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'State Name already Exists'
	FROM @tempStateDetail T
	INNER JOIN  StateMaster PM ON PM.StateName = T.StateName and T.IsValid=1

	 Insert into StateMaster(StateName, IsActive,CreatedBy,CreatedOn)
			select StateName,
			CASE WHEN IsActive='YES' THEN 1 ELSE 0 END
			,@LoggedInUserId,GETDATE() from @tempStateDetail Where IsValid = 1


	-- 5. Returning Invalid records
	Select StateName,IsActive,ValidationMessage
	From @tempStateDetail
	Where IsValid = 0;
END

GO
