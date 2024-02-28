-- GetProductDesignById 1
CREATE OR ALTER Procedure [dbo].[GetProductDesignById]
	@Id BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	 PD.ProductDesignId,
	 PD.DesignName,
	 SM.SizeId, SM.SizeName,
	 BM.BrandId, BM.BrandName,
	 CM.CollectionId, CM.CollectionName,
	 CEM.CategoryId, CEM.CategoryName,
	 TM.TypeId, TM.TypeName,
	 PM.PunchId, PM.PunchName,
	 SRM.SurfaceId, SRM.SurfaceName,
	 THM.ThicknessId, THM.ThicknessName,
	 TIM.TileSizeId, TIM.TileSizeName,
	 PD.NoOfTilesPerBox, PD.WeightPerBox,
	 PD.BoxCoverageAreaSqFoot, PD.BoxCoverageAreaSqMeter,
	 PD.IsActive, PD.CreatedOn, PD.ModifiedOn

	 FROM ProductDesigns PD WITH(NOLOCK)  
    INNER JOIN SizeMaster SM WITH(NOLOCK)  
		ON PD.SizeId = SM.SizeId
	INNER JOIN BrandMaster BM WITH(NOLOCK)  
		ON PD.BrandId = BM.BrandId
	INNER JOIN CollectionMaster CM WITH(NOLOCK)  
		ON PD.CollectionId = CM.CollectionId
	INNER JOIN CategoryMaster CEM WITH(NOLOCK)  
		ON PD.CategoryId = CEM.CategoryId
	INNER JOIN TypeMaster TM WITH(NOLOCK)  
		ON PD.TypeId = TM.TypeId
	INNER JOIN PunchMaster PM WITH(NOLOCK)  
		ON PD.PunchId = PM.PunchId
	INNER JOIN SurfaceMaster SRM WITH(NOLOCK)  
		ON PD.SurfaceId = SRM.SurfaceId
	INNER JOIN ThicknessMaster THM WITH(NOLOCK)  
		ON PD.ThicknessId = THM.ThicknessId
	INNER JOIN TileSizeMaster TIM WITH(NOLOCK)  
		ON PD.TileSizeId = TIM.TileSizeId
	WHERE PD.ProductDesignId = @Id
END

GO

--EXEC GetProductDesignFilesByDesignId 1
CREATE OR ALTER  PROCEDURE [dbo].[GetProductDesignFilesByDesignId]
	@ProductDesignId BigInt
As
Begin
	Set NoCount On;

	Select
		pdf.ProductDesignId,
		pdf.ProductDesignFilesId,
		pdf.UploadedFilesName,
		pdf.StoredFilesName,
		pdf.FileType
	From ProductDesignFiles pdf With(NoLock)
	Where pdf.ProductDesignId = @ProductDesignId
		And pdf.IsDeleted = 0
End

GO

/*  
 Description : Get Product Design
 EXEC [dbo].[GetProductDesignList]    
  @PageSize=10,  
     @PageNo=1,  
     @SortBy='',  
     @OrderBy='',  
  @ValueForSearch='Emplo',  
  @IsActive=NULL,  
  @IsExport=NULL  
NOTE : PageSize=0 Then all records without pagination will be retried.  
*/  
CREATE OR ALTER PROCEDURE [dbo].[GetProductDesignList]  
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
  SET @SelectColumns='PD.ProductDesignId,  
       PD.DesignName,  
       CASE WHEN PD.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,  
       SM.SizeName, BM.BrandName, CM.CollectionName, CEM.CategoryName, TM.TypeName,
	   PM.PunchName, SRM.SurfaceName, THM.ThicknessName, TIM.TileSizeName,
	   PD.NoOfTilesPerBox, PD.WeightPerBox, PD.BoxCoverageAreaSqFoot,
	   PD.BoxCoverageAreaSqMeter, PD.CreatedBy, PD.CreatedOn';  
 END  
 ELSE  
 BEGIN  
  SET @SelectColumns='PD.DesignName,  
       CASE WHEN PD.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,  
       SM.SizeName, BM.BrandName, CM.CollectionName, CEM.CategoryName, TM.TypeName,
	   PM.PunchName, SRM.SurfaceName, THM.ThicknessName, TIM.TileSizeName,PD.NoOfTilesPerBox, 
	   PD.WeightPerBox, PD.BoxCoverageAreaSqFoot,
	   PD.BoxCoverageAreaSqMeter, PD.CreatedBy, PD.CreatedOn';  
 END  
 SET @FinalQueryString = N' WITH CTE AS (  
     SELECT  
     '+ @SelectColumns +'  
    FROM ProductDesigns PD WITH(NOLOCK)  
    INNER JOIN SizeMaster SM WITH(NOLOCK)  
		ON PD.SizeId = SM.SizeId
	INNER JOIN BrandMaster BM WITH(NOLOCK)  
		ON PD.BrandId = BM.BrandId
	INNER JOIN CollectionMaster CM WITH(NOLOCK)  
		ON PD.CollectionId = CM.CollectionId
	INNER JOIN CategoryMaster CEM WITH(NOLOCK)  
		ON PD.CategoryId = CEM.CategoryId
	INNER JOIN TypeMaster TM WITH(NOLOCK)  
		ON PD.TypeId = TM.TypeId
	INNER JOIN PunchMaster PM WITH(NOLOCK)  
		ON PD.PunchId = PM.PunchId
	INNER JOIN SurfaceMaster SRM WITH(NOLOCK)  
		ON PD.SurfaceId = SRM.SurfaceId
	INNER JOIN ThicknessMaster THM WITH(NOLOCK)  
		ON PD.ThicknessId = THM.ThicknessId
	INNER JOIN TileSizeMaster TIM WITH(NOLOCK)  
		ON PD.TileSizeId = TIM.TileSizeId
      WHERE (  
        @ValueForSearch = '''' OR   
        (PD.DesignName Like ''%''+@ValueForSearch+''%''))  
        AND (@IsActive IS NULL OR PD.IsActive=@IsActive))   
        SELECT *, (SELECT COUNT(*) FROM CTE) AS TotalRecords  
        FROM CTE  
   '+@OrderByQuery+' '+@PaginationQuery+' '  
     
 PRINT @FinalQueryString;  
  
 exec sp_executesql @FinalQueryString,  
      N'@ValueForSearch VARCHAR(100),@IsActive BIT',  
      @ValueForSearch,@IsActive  
END  

GO

/*
	EXEC [dbo].[SaveImportProductDesign ]
	'<ArrayOfImportProductDesign>
  <ImportProductDesign>
    <DesignName>Test Design From File 5</DesignName>
    <SizeName>24 x 24</SizeName>
    <BrandName>Test Brand 1</BrandName>
    <CollectionName>Collection 1</CollectionName>
    <CategoryName>Category 1</CategoryName>
    <TypeName>Type 1</TypeName>
    <PunchName>Punch 1</PunchName>
    <SurfaceName>Surface 1</SurfaceName>
    <ThicknessName>10mm</ThicknessName>
    <TileSizeName>Tile 1</TileSizeName>
    <NoOfTilesPerBox>15</NoOfTilesPerBox>
    <WeightPerBox>10</WeightPerBox>
    <BoxCoverageAreaSqFoot>20.2</BoxCoverageAreaSqFoot>
    <BoxCoverageAreaSqMeter>15.25</BoxCoverageAreaSqMeter>
    <IsActive>Yes</IsActive>
  </ImportProductDesign>
  <ImportProductDesign>
    <DesignName>Test Design From File 6</DesignName>
    <SizeName>30 by 30 Inch</SizeName>
    <BrandName>Test Brand 2</BrandName>
    <CollectionName>Collection 2</CollectionName>
    <CategoryName>Category 2</CategoryName>
    <TypeName>Type 2</TypeName>
    <PunchName>Punch 5</PunchName>
    <SurfaceName>Surface 2</SurfaceName>
    <ThicknessName>20mm</ThicknessName>
    <TileSizeName>Tile 2</TileSizeName>
    <NoOfTilesPerBox>20</NoOfTilesPerBox>
    <WeightPerBox>20</WeightPerBox>
    <BoxCoverageAreaSqFoot>50.2</BoxCoverageAreaSqFoot>
    <BoxCoverageAreaSqMeter>52</BoxCoverageAreaSqMeter>
    <IsActive>Yes</IsActive>
  </ImportProductDesign>
  <ImportProductDesign>
    <DesignName>Test Design From File 7</DesignName>
    <SizeName>12 x 12</SizeName>
    <BrandName>Test Brand 5</BrandName>
    <CollectionName>Collection 3</CollectionName>
    <CategoryName>Category 3</CategoryName>
    <TypeName>Type 3</TypeName>
    <PunchName>Punch 3</PunchName>
    <SurfaceName>Surface 3</SurfaceName>
    <ThicknessName>30mm</ThicknessName>
    <TileSizeName>Tile 3</TileSizeName>
    <NoOfTilesPerBox>23</NoOfTilesPerBox>
    <WeightPerBox>30</WeightPerBox>
    <BoxCoverageAreaSqFoot>62</BoxCoverageAreaSqFoot>
    <BoxCoverageAreaSqMeter>12.2</BoxCoverageAreaSqMeter>
    <IsActive>Yes</IsActive>
  </ImportProductDesign>
  <ImportProductDesign>
    <DesignName>Test Design From File 8</DesignName>
    <SizeName>24 x 25</SizeName>
    <BrandName>Test Brand 1</BrandName>
    <CollectionName>Collection 1</CollectionName>
    <CategoryName>Category 1</CategoryName>
    <TypeName>Type 1</TypeName>
    <PunchName>Punch 1</PunchName>
    <SurfaceName>Surface 1</SurfaceName>
    <ThicknessName>10mm</ThicknessName>
    <TileSizeName>Tile 1</TileSizeName>
    <NoOfTilesPerBox>30</NoOfTilesPerBox>
    <WeightPerBox>41</WeightPerBox>
    <BoxCoverageAreaSqFoot>52.2</BoxCoverageAreaSqFoot>
    <BoxCoverageAreaSqMeter>14.3</BoxCoverageAreaSqMeter>
    <IsActive>Yes</IsActive>
  </ImportProductDesign>
</ArrayOfImportProductDesign>
',1
*/
CREATE OR ALTER PROCEDURE [dbo].[SaveImportProductDesign]
(
	@XmlProductDesignData XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @tempProductDesignData TABLE
	(
		DesignName VARCHAR(100),
		SizeName VARCHAR(100),
		BrandName VARCHAR(100),
		CollectionName VARCHAR(100),
		CategoryName VARCHAR(100),
		TypeName VARCHAR(100),
		PunchName VARCHAR(100),
		SurfaceName VARCHAR(100),
		ThicknessName VARCHAR(100),
		TileSizeName VARCHAR(100),
		NoOfTilesPerBox INT,
		WeightPerBox NUMERIC(6,2),
		BoxCoverageAreaSqFoot NUMERIC(6,2),
		BoxCoverageAreaSqMeter NUMERIC(6,2),
		IsActive VARCHAR(100),
		IsValid BIT,
		ValidationMessage VarChar(Max)
	)

	INSERT INTO @tempProductDesignData
		(
		 DesignName,
		 SizeName,
		 BrandName,
		 CollectionName,
		 CategoryName,
		 TypeName,
		 PunchName,
		 SurfaceName,
		 ThicknessName,
		 TileSizeName,
		 NoOfTilesPerBox,
		 WeightPerBox,
		 BoxCoverageAreaSqFoot,
		 BoxCoverageAreaSqMeter,
		 IsActive,
		 IsValid
		)
	SELECT
		DesignName = T.Item.value('DesignName[1]', 'VARCHAR(100)'),
		SizeName = T.Item.value('SizeName[1]', 'VARCHAR(100)'),
		BrandName = T.Item.value('BrandName[1]', 'VARCHAR(100)'),
		CollectionName = T.Item.value('CollectionName[1]', 'VARCHAR(100)'),
		CategoryName = T.Item.value('CategoryName[1]', 'VARCHAR(100)'),
		TypeName = T.Item.value('TypeName[1]', 'VARCHAR(100)'),
		PunchName = T.Item.value('PunchName[1]', 'VARCHAR(100)'),
		SurfaceName = T.Item.value('SurfaceName[1]', 'VARCHAR(100)'),
		ThicknessName = T.Item.value('ThicknessName[1]', 'VARCHAR(100)'),
		TileSizeName = T.Item.value('TileSizeName[1]', 'VARCHAR(100)'),
		NoOfTilesPerBox = T.Item.value('NoOfTilesPerBox[1]', 'INT'),
		WeightPerBox = T.Item.value('WeightPerBox[1]', 'NUMERIC(6,2)'),
		BoxCoverageAreaSqFoot = T.Item.value('BoxCoverageAreaSqFoot[1]', 'NUMERIC(6,2)'),
		BoxCoverageAreaSqMeter = T.Item.value('BoxCoverageAreaSqMeter[1]', 'NUMERIC(6,2)'),
		IsActive = UPPER(T.Item.value('IsActive[1]', 'VARCHAR(100)')),1 
	FROM @XmlProductDesignData.nodes('/ArrayOfImportProductDesign/ImportProductDesign') AS T(Item)
	
	UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Design Name already exists., '
	FROM @tempProductDesignData T
	INNER JOIN  ProductDesigns PD  
		ON PD.DesignName = T.DesignName
	WHERE T.IsValid=1

		UPDATE T
	SET IsValid = 0,
		ValidationMessage = 'Size Name not exists., '
	FROM @tempProductDesignData T
	LEFT JOIN SizeMaster SM 
	ON LOWER(TRIM(SM.SizeName)) = LOWER(TRIM(T.SizeName))
	WHERE  SM.SizeId IS NULL

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = ISNULL(ValidationMessage,'') +  'Branch Name not exists., '
	FROM @tempProductDesignData T
	LEFT JOIN BrandMaster BM 
		ON LOWER(TRIM(BM.BrandName)) = LOWER(TRIM(T.BrandName))
	WHERE BM.BrandId IS NULL

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = ISNULL(ValidationMessage,'') +  'Collection Name not exists., '
	FROM @tempProductDesignData T
	LEFT JOIN CollectionMaster CM 
		ON LOWER(TRIM(CM.CollectionName)) = LOWER(TRIM(T.CollectionName))
	WHERE CM.CollectionId IS NULL

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = ISNULL(ValidationMessage,'') +  'Category Name not exists., '
	FROM @tempProductDesignData T
	LEFT JOIN CategoryMaster CEM 
		ON LOWER(TRIM(CEM.CategoryName)) = LOWER(TRIM(T.CategoryName))
	WHERE CEM.CategoryId IS NULL

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = ISNULL(ValidationMessage,'') +  'Type Name not exists., '
	FROM @tempProductDesignData T
	LEFT JOIN TypeMaster TM 
		ON LOWER(TRIM(TM.TypeName)) = LOWER(TRIM(T.TypeName))
	WHERE TM.TypeId IS NULL

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = ISNULL(ValidationMessage,'') +  'Punch Name not exists., '
	FROM @tempProductDesignData T
	LEFT JOIN PunchMaster PM 
		ON LOWER(TRIM(PM.PunchName)) = LOWER(TRIM(T.PunchName))
	WHERE PM.PunchId IS NULL

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = ISNULL(ValidationMessage,'') +  'Surface Name not exists., '
	FROM @tempProductDesignData T
	LEFT JOIN SurfaceMaster SM 
		ON LOWER(TRIM(SM.SurfaceName)) = LOWER(TRIM(T.SurfaceName))
	WHERE SM.SurfaceId IS NULL

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = ISNULL(ValidationMessage,'') +  'Thickness Name not exists., '
	FROM @tempProductDesignData T
	LEFT JOIN ThicknessMaster THM 
		ON LOWER(TRIM(THM.ThicknessName)) = LOWER(TRIM(T.ThicknessName))
	WHERE THM.ThicknessId IS NULL

	UPDATE T
	SET IsValid = 0,
		ValidationMessage = ISNULL(ValidationMessage,'') +  'Tile Size Name not exists., '
	FROM @tempProductDesignData T
	LEFT JOIN TileSizeMaster TLM 
		ON LOWER(TRIM(TLM.TileSizeName)) = LOWER(TRIM(T.TileSizeName))
	WHERE TLM.TileSizeId IS NULL

	INSERT INTO ProductDesigns
	(
		DesignName,
		SizeId,
		BrandId,
		CollectionId,
		CategoryId,
		TypeId,
		PunchId,
		SurfaceId,
		ThicknessId,
		TileSizeId,
		NoOfTilesPerBox,
		WeightPerBox,
		BoxCoverageAreaSqFoot,
		BoxCoverageAreaSqMeter,
		IsActive,
		CreatedBy,
		CreatedOn
	)
	SELECT 
		   DesignName, SM.SizeId, BM.BrandId, CM.CollectionId, CEM.CategoryId, TM.TypeId,
		   PM.PunchId, SRM.SurfaceId, TEM.ThicknessId, TIM.TileSizeId,NoOfTilesPerBox,WeightPerBox,
		   BoxCoverageAreaSqFoot, BoxCoverageAreaSqMeter, 
		   CASE WHEN tempd.IsActive='Yes' THEN 1 ELSE 0 END,
		   @LoggedInUserId,
		   GETDATE() 
	FROM @tempProductDesignData tempd
	INNER JOIN SizeMaster SM 
		ON LOWER(TRIM(SM.SizeName)) = LOWER(TRIM(tempd.SizeName))
	INNER JOIN BrandMaster BM 
		ON LOWER(TRIM(BM.BrandName)) = LOWER(TRIM(tempd.BrandName))
	INNER JOIN CollectionMaster CM 
		ON LOWER(TRIM(CM.CollectionName)) = LOWER(TRIM(tempd.CollectionName))
	INNER JOIN CategoryMaster CEM 
		ON LOWER(TRIM(CEM.CategoryName)) = LOWER(TRIM(tempd.CategoryName))
	INNER JOIN TypeMaster TM 
		ON LOWER(TRIM(TM.TypeName)) = LOWER(TRIM(tempd.TypeName))
	INNER JOIN PunchMaster PM 
		ON LOWER(TRIM(PM.PunchName)) = LOWER(TRIM(tempd.PunchName))
	INNER JOIN SurfaceMaster SRM 
		ON LOWER(TRIM(SRM.SurfaceName)) = LOWER(TRIM(tempd.SurfaceName))
	INNER JOIN ThicknessMaster TEM 
		ON LOWER(TRIM(TEM.ThicknessName)) = LOWER(TRIM(tempd.ThicknessName))
	INNER JOIN TileSizeMaster TIM 
		ON LOWER(TRIM(TIM.TileSizeName)) = LOWER(TRIM(tempd.TileSizeName))
	WHERE IsValid = 1
	
	SELECT 
		DesignName,SizeName, BrandName, CollectionName, CategoryName, TypeName, PunchName
		SurfaceName, ThicknessName, TileSizeName, NoOfTilesPerBox, WeightPerBox, BoxCoverageAreaSqFoot, BoxCoverageAreaSqMeter
		IsActive,ValidationMessage
	FROM @tempProductDesignData
	WHERE IsValid = 0;
END

GO

/*
EXEC SaveProductDesigns 1,'Design Name 1',1,1,1,1,1,1,1,1,1,10,50.5,100,120,1,
'<ArrayOfProductDesignFiles xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ProductDesignFiles>
    <ProductDesignFilesId>1</ProductDesignFilesId>
    <UploadedFilesName>TestFile1</UploadedFilesName>
    <StoredFilesName>File_141414</StoredFilesName>
    <FileType>.jpg</FileType>
    <IsDeleted>0</IsDeleted>
  </ProductDesignFiles>
  <ProductDesignFiles>
    <ProductDesignFilesId>2</ProductDesignFilesId>
    <UploadedFilesName>TestFile2</UploadedFilesName>
    <StoredFilesName>File_121212</StoredFilesName>
    <FileType>.jpg</FileType>
    <IsDeleted>0</IsDeleted>
  </ProductDesignFiles>
</ArrayOfProductDesignFiles>',1
*/


CREATE OR ALTER PROCEDURE [dbo].[SaveProductDesigns]
(
	@ProductDesignId BIGINT,
	@DesignName  VARCHAR(100),
	@SizeId INT,
	@BrandId INT,
	@CollectionId INT,
	@CategoryId INT,
	@TypeId INT,
	@PunchId INT,
	@SurfaceId INT,
	@ThicknessId INT,
	@TileSizeId INT,
	@NoOfTilesPerBox INT,
	@WeightPerBox NUMERIC(6,2),
	@BoxCoverageAreaSqFoot NUMERIC(6,2),
	@BoxCoverageAreaSqMeter NUMERIC(6,2),
	@IsActive BIT,
	@XmlProductDesignFiles XML,
	@LoggedInUserId BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	If (
		(@ProductDesignId=0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM ProductDesigns WITH(NOLOCK) 
				WHERE  DesignName=@DesignName
			)
		)
		OR
		(@ProductDesignId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM ProductDesigns WITH(NOLOCK) 
				WHERE  DesignName=@DesignName  and ProductDesignId<>@ProductDesignId
			)
		))
	BEGIN
		SET @Result=@IsNameExists;
	END
	ELSE
	BEGIN
		IF (@ProductDesignId=0)
		BEGIN
			Insert into ProductDesigns
			(
				DesignName,SizeId,BrandId,CollectionId,CategoryId,TypeId,PunchId,SurfaceId,
				ThicknessId,TileSizeId,NoOfTilesPerBox,WeightPerBox,BoxCoverageAreaSqFoot,BoxCoverageAreaSqMeter,IsActive,CreatedBy,CreatedOn
			)
			Values
			(
				@DesignName,@SizeId,@BrandId,@CollectionId,@CategoryId,@TypeId,@PunchId,@SurfaceId, 
				@ThicknessId,@TileSizeId,@NoOfTilesPerBox,@WeightPerBox,@BoxCoverageAreaSqFoot, @BoxCoverageAreaSqMeter,@IsActive,@LoggedInUserId,GETDATE()
			)
			
			SET @Result = SCOPE_IDENTITY();

		END
		ELSE IF(@ProductDesignId> 0 and EXISTS(SELECT TOP 1 1 FROM ProductDesigns WHERE ProductDesignId=@ProductDesignId))
		BEGIN
			UPDATE ProductDesigns
			SET DesignName=@DesignName,
				SizeId=@SizeId,
				BrandId=@BrandId,
				CollectionId=@CollectionId,
				CategoryId=@CategoryId,
				TypeId=@TypeId,
				PunchId=@PunchId,
				SurfaceId=@SurfaceId,
				ThicknessId=@ThicknessId,
				TileSizeId=@TileSizeId,
				NoOfTilesPerBox=@NoOfTilesPerBox,
				WeightPerBox=@WeightPerBox,
				BoxCoverageAreaSqFoot=@BoxCoverageAreaSqFoot,
				BoxCoverageAreaSqMeter=@BoxCoverageAreaSqMeter,
				IsActive=@IsActive,
				ModifiedBy=@LoggedInUserId,
				ModifiedOn=GETDATE()
			WHERE ProductDesignId=@ProductDesignId

			SET @Result = @ProductDesignId

		END
		ELSE
		BEGIN
			SET @Result=@NoRecordExists
		END
	END
	
	-- Start: To add update product design files
		IF @Result > 0 -- @Result is ProductDesignId here
		BEGIN

			DECLARE @tempProductDesignFiles TABLE
			(
				ProductDesignFilesId	BIGINT,
				UploadedFilesName		VARCHAR(300),
				StoredFilesName			VARCHAR(300),
				FileType				VARCHAR(20),
				IsDeleted				BIT
			)

			INSERT INTO @tempProductDesignFiles
			(
				ProductDesignFilesId,UploadedFilesName,StoredFilesName,FileType,IsDeleted
			)
			SELECT
				ProductDesignFilesId	= T.Item.value('ProductDesignFilesId[1]', 'BIGINT'),
				UploadedFilesName		= T.Item.value('UploadedFilesName[1]', 'VARCHAR(300)'),
				StoredFilesName			= T.Item.value('StoredFilesName[1]', 'VARCHAR(300)'),
				FileType				= T.Item.value('FileType[1]', 'VARCHAR(20)'),
				IsDeleted				= T.Item.value('IsDeleted[1]', 'BIT')
			FROM @XmlProductDesignFiles.nodes('/ArrayOfProductDesignFiles/ProductDesignFiles') As T(Item)
			
			-- Insert Into ProductDesignFiles With ProductDesignFilesId = 0
			INSERT INTO ProductDesignFiles
			(
				ProductDesignId,UploadedFilesName,StoredFilesName,FileType,IsDeleted,ModifiedBy, ModifiedOn
			)
			SELECT
				@Result,UploadedFilesName,StoredFilesName,FileType,0,@LoggedInUserId,GETDATE() 
				FROM @tempProductDesignFiles 
				WHERE ProductDesignFilesId = 0
			
			-- To update existing records
			Update pdf
			Set pdf.UploadedFilesName = tpdf.UploadedFilesName,
				pdf.StoredFilesName	  = tpdf.StoredFilesName,
				pdf.FileType		= tpdf.FileType,
				pdf.ModifiedOn		= GETDATE(),
				pdf.ModifiedBy		= @LoggedInUserId
			From ProductDesignFiles pdf WITH(NOLOCK)
			Inner Join @tempProductDesignFiles tpdf
				On pdf.ProductDesignFilesId = tpdf.ProductDesignFilesId
			Where pdf.ProductDesignId = @Result
				And tpdf.ProductDesignFilesId <> 0

			-- To delete removed remarks
			UPDATE pdf
			SET pdf.IsDeleted = 1,
				pdf.ModifiedOn = GETDATE(),
				pdf.ModifiedBy = @LoggedInUserId
			FROM @tempProductDesignFiles tpdf
			LEFT JOIN ProductDesignFiles pdf WITH(NOLOCK)
				ON tpdf.ProductDesignFilesId = pdf.ProductDesignFilesId
			WHERE pdf.ProductDesignId = @Result
				And pdf.ProductDesignId IS NULL
		END

	SELECT @Result as Result
End

GO

CREATE OR ALTER  PROCEDURE [dbo].[GetTileSizeMasterForSelectList]
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TileSizeId AS [Value], TileSizeName AS [Text]
	FROM TileSizeMaster WITH(NOLOCK)
	WHERE @IsActive IS NULL OR IsActive = @IsActive
END

GO

CREATE OR ALTER PROCEDURE [dbo].[GetThicknessMasterForSelectList]
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ThicknessId AS [Value], ThicknessName AS [Text]
	FROM ThicknessMaster WITH(NOLOCK)
	WHERE @IsActive IS NULL OR IsActive = @IsActive
END

GO

CREATE OR ALTER PROCEDURE [dbo].[GetSurfaceMasterForSelectList]
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SurfaceId AS [Value], SurfaceName AS [Text]
	FROM SurfaceMaster WITH(NOLOCK)
	WHERE @IsActive IS NULL OR IsActive = @IsActive
END

GO

CREATE OR ALTER  PROCEDURE [dbo].[GetSizeMasterForSelectList]
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SizeId AS [Value], SizeName AS [Text]
	FROM SizeMaster WITH(NOLOCK)
	WHERE @IsActive IS NULL OR IsActive = @IsActive
END

GO

CREATE OR ALTER  PROCEDURE [dbo].[GetPunchMasterForSelectList]
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT PunchId AS [Value], PunchName AS [Text]
	FROM PunchMaster WITH(NOLOCK)
	WHERE @IsActive IS NULL OR IsActive = @IsActive
END

GO

CREATE OR ALTER  PROCEDURE [dbo].[GetCollectionMasterForSelectList]
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CollectionId AS [Value], CollectionName AS [Text]
	FROM CollectionMaster WITH(NOLOCK)
	WHERE @IsActive IS NULL OR IsActive = @IsActive
END

GO

CREATE OR ALTER  PROCEDURE [dbo].[GetCategoryMasterForSelectList]
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT CategoryId AS [Value], CategoryName AS [Text]
	FROM CategoryMaster WITH(NOLOCK)
	WHERE @IsActive IS NULL OR IsActive = @IsActive
END

GO

CREATE OR ALTER  PROCEDURE [dbo].[GetBrandMasterForSelectList]
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT BrandId AS [Value], BrandName AS [Text]
	FROM BrandMaster WITH(NOLOCK)
	WHERE @IsActive IS NULL OR IsActive = @IsActive
END

GO

-- GetManageBoxSizeById 1  
CREATE OR ALTER Procedure [dbo].[GetManageBoxSizeById]  
 @Id BIGINT  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT  
  MBS.BoxSizeId, MBS.Thickness,
  TIM.TileSizeId, TIM.TileSizeName,  
  MBS.NoOfTilesPerBox, MBS.WeightPerBox,  
  MBS.BoxCoverageAreaSqFoot, MBS.BoxCoverageAreaSqMeter,  
  MBS.IsActive, MBS.CreatedOn, MBS.ModifiedOn  
  
  FROM ManageBoxSize MBS WITH(NOLOCK)    
	INNER JOIN TileSizeMaster TIM WITH(NOLOCK)    
	ON MBS.TileSizeId = TIM.TileSizeId  
	WHERE MBS.BoxSizeId = @Id  
END

GO

/*
Description : Get Manage Box Size
 EXEC [dbo].[GetManageBoxSizeList]      
  @PageSize=10,    
     @PageNo=1,    
     @SortBy='',    
     @OrderBy='',    
  @ValueForSearch='Emplo',    
  @IsActive=NULL,    
  @IsExport=NULL    
NOTE : PageSize=0 Then all records without pagination will be retried.    
*/    
CREATE OR ALTER PROCEDURE [dbo].[GetManageBoxSizeList]    
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
  SET @SelectColumns='MBS.BoxSizeId,    
       TIM.TileSizeName,    
       CASE WHEN MBS.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,    
    MBS.NoOfTilesPerBox, MBS.WeightPerBox, MBS.BoxCoverageAreaSqFoot, MBS.Thickness, 
    MBS.BoxCoverageAreaSqMeter, MBS.CreatedBy, MBS.CreatedOn';    
 END    
 ELSE    
 BEGIN    
  SET @SelectColumns=' TIM.TileSizeName,    
       CASE WHEN MBS.IsActive=1 THEN ''Yes'' ELSE ''No'' END AS IsActive,    
    MBS.NoOfTilesPerBox, MBS.WeightPerBox, MBS.BoxCoverageAreaSqFoot, MBS.Thickness, 
    MBS.BoxCoverageAreaSqMeter, MBS.CreatedBy, MBS.CreatedOn';    
 END    
 SET @FinalQueryString = N' WITH CTE AS (    
     SELECT    
     '+ @SelectColumns +'    
    FROM ManageBoxSize MBS WITH(NOLOCK)    
    INNER JOIN TileSizeMaster TIM WITH(NOLOCK)    
		 ON MBS.TileSizeId = TIM.TileSizeId  
      WHERE (    
        @ValueForSearch = '''' OR     
        (TIM.TileSizeName Like ''%''+@ValueForSearch+''%''))    
        AND (@IsActive IS NULL OR MBS.IsActive=@IsActive))     
        SELECT *, (SELECT COUNT(*) FROM CTE) AS TotalRecords    
        FROM CTE    
   '+@OrderByQuery+' '+@PaginationQuery+' '    
       
 --PRINT @FinalQueryString;    
    
 exec sp_executesql @FinalQueryString,    
      N'@ValueForSearch VARCHAR(100),@IsActive BIT',    
      @ValueForSearch,@IsActive    
END   

GO

CREATE OR ALTER PROCEDURE [dbo].[SaveManageBoxSize]
(
	@BoxSizeId				INT,
	@TileSizeId				INT,
	@NoOfTilesPerBox		INT,
	@WeightPerBox			NUMERIC(6,2),
	@Thickness				NUMERIC(6,2),
	@BoxCoverageAreaSqFoot	NUMERIC(6,2),
	@BoxCoverageAreaSqMeter	NUMERIC(6,2),
	@IsActive				BIT,
	@LoggedInUserId			BIGINT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Result BIGINT=0;
	DECLARE @IsNameExists BIGINT=-2;
	DECLARE @NoRecordExists BIGINT=-1;

	IF ((@BoxSizeId=0 AND EXISTS
			(
				SELECT TOP 1 1 
				FROM ManageBoxSize WITH(NOLOCK) 
				WHERE  TileSizeId=@TileSizeId AND NoOfTilesPerBox = @NoOfTilesPerBox AND WeightPerBox = @WeightPerBox
			)
		 )
		 OR
		(@BoxSizeId>0 AND 
			EXISTS
			(
				SELECT TOP 1 1 
				FROM ManageBoxSize WITH(NOLOCK) 
				WHERE  TileSizeId=@TileSizeId AND NoOfTilesPerBox = @NoOfTilesPerBox AND WeightPerBox = @WeightPerBox AND BoxSizeId<>@BoxSizeId
			)
		))
		BEGIN
			SET @Result=@IsNameExists;
		END
	ELSE
		BEGIN
			IF (@BoxSizeId=0)
			BEGIN
				INSERT INTO ManageBoxSize
				(
					TileSizeId, NoOfTilesPerBox, WeightPerBox, Thickness, BoxCoverageAreaSqFoot, BoxCoverageAreaSqMeter, IsActive,IsDeleted,CreatedBy,CreatedOn
				)
				VALUES
				(
					@TileSizeId, @NoOfTilesPerBox, @WeightPerBox, @Thickness, @BoxCoverageAreaSqFoot, @BoxCoverageAreaSqMeter, @IsActive,0,@LoggedInUserId,GETDATE()
				)
				
				SET @Result = SCOPE_IDENTITY();
			END
			ELSE IF(@BoxSizeId> 0 and EXISTS(SELECT TOP 1 1 FROM ManageBoxSize WHERE BoxSizeId = @BoxSizeId))
			BEGIN
				UPDATE ManageBoxSize
				SET @TileSizeId=@TileSizeId,
					NoOfTilesPerBox = @NoOfTilesPerBox,
					WeightPerBox = @WeightPerBox,
					Thickness = @Thickness,
					BoxCoverageAreaSqFoot = @BoxCoverageAreaSqFoot,
					BoxCoverageAreaSqMeter = @BoxCoverageAreaSqMeter,
					IsActive=@IsActive,
					ModifiedBy=@LoggedInUserId,
					ModifiedOn=GETDATE()
				WHERE BoxSizeId = @BoxSizeId

				SET @Result = @BoxSizeId;
			END
			ELSE
			BEGIN
				SET @Result=@NoRecordExists
			END
		END
	
	SELECT @Result as Result 
END

GO
