using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using static CasaAPI.Models.BloodModel;
using static CasaAPI.Models.BrandModel;
using static CasaAPI.Models.CategoryModel;
using static CasaAPI.Models.CollectionModel;
using static CasaAPI.Models.ContactTypeModel;
using static CasaAPI.Models.GendorModel;
using static CasaAPI.Models.PunchModel;
using static CasaAPI.Models.SurfaceModel;
using static CasaAPI.Models.ThicknessModel;
using static CasaAPI.Models.TileSizeModel;
using static CasaAPI.Models.TileTypeModel;
using static CasaAPI.Models.TypeModel;
using static CasaAPI.Models.WeekCloseModel;
using static CasaAPI.Models.CuttingSizeModel;
using Models;
using Newtonsoft.Json.Linq;

namespace CasaAPI.Repositories
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        private IConfiguration _configuration;
        public AdminRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Size
        public async Task<int> SaveSize(SizeSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SizeId", parameters.SizeId);
            queryParameters.Add("@SizeName", parameters.SizeName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveSizeDetails", queryParameters);
        }
        public async Task<IEnumerable<SizeDetailsResponse>> GetSizesList(SizeSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<SizeDetailsResponse>("GetSizesList", queryParameters);
        }
        public async Task<SizeDetailsResponse?> GetSizeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<SizeDetailsResponse>("GetSizeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<SizeFailToImportValidationErrors>> ImportSizesDetails(List<SizeImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlSizeData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlSizeData", xmlSizeData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<SizeFailToImportValidationErrors>("SaveImportSizeDetails", queryParameters);
        }
        #endregion

        #region Brand
        public async Task<int> SaveBrand(BrandSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BrandId", parameters.BrandId);
            queryParameters.Add("@BrandName", parameters.BrandName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveBrandDetails", queryParameters);
        }
        public async Task<IEnumerable<BrandDetailsResponse>> GetBrandsList(BrandSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<BrandDetailsResponse>("GetBrandsList", queryParameters);
        }
        public async Task<BrandDetailsResponse?> GetBrandDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<BrandDetailsResponse>("GetBrandDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<BrandFailToImportValidationErrors>> ImportBrandsDetails(List<BrandImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlBrandData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<BrandFailToImportValidationErrors>("SaveImportBrandDetails", queryParameters);
        }
        #endregion

        #region Collection
        public async Task<int> SaveCollection(CollectionSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CollectionId", parameters.CollectionId);
            queryParameters.Add("@CollectionName", parameters.CollectionName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveCollectionDetails", queryParameters);
        }
        public async Task<IEnumerable<CollectionDetailsResponse>> GetCollectionsList(CollectionSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<CollectionDetailsResponse>("GetCollectionsList", queryParameters);
        }
        public async Task<CollectionDetailsResponse?> GetCollectionDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<CollectionDetailsResponse>("GetCollectionDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<CollectionFailToImportValidationErrors>> ImportCollectionsDetails(List<CollectionImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlCollectionData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlCollectionData", xmlCollectionData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<CollectionFailToImportValidationErrors>("SaveImportCollectionDetails", queryParameters);
        }
        #endregion

        #region Category
        public async Task<int> SaveCategory(CategorySaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CategoryId", parameters.CategoryId);
            queryParameters.Add("@CategoryName", parameters.CategoryName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveCategoryDetails", queryParameters);
        }
        public async Task<IEnumerable<CategoryDetailsResponse>> GetCategorysList(CategorySearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<CategoryDetailsResponse>("GetCategorysList", queryParameters);
        }
        public async Task<CategoryDetailsResponse?> GetCategoryDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<CategoryDetailsResponse>("GetCategoryDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<CategoryFailToImportValidationErrors>> ImportCategorysDetails(List<CategoryImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlCategoryData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlCategoryData", xmlCategoryData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<CategoryFailToImportValidationErrors>("SaveImportCategoryDetails", queryParameters);
        }
        #endregion

        #region Type
        public async Task<int> SaveType(TypeSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@TypeId", parameters.TypeId);
            queryParameters.Add("@TypeName", parameters.TypeName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveTypeDetails", queryParameters);
        }
        public async Task<IEnumerable<TypeDetailsResponse>> GetTypesList(TypeSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<TypeDetailsResponse>("GetTypesList", queryParameters);
        }
        public async Task<TypeDetailsResponse?> GetTypeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<TypeDetailsResponse>("GetTypeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<TypeFailToImportValidationErrors>> ImportTypesDetails(List<TypeImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlTypeData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlTypeData", xmlTypeData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<TypeFailToImportValidationErrors>("SaveImportTypeDetails", queryParameters);
        }
        #endregion

        #region Punch
        public async Task<int> SavePunch(PunchSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PunchId", parameters.PunchId);
            queryParameters.Add("@PunchName", parameters.PunchName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SavePunchDetails", queryParameters);
        }
        public async Task<IEnumerable<PunchDetailsResponse>> GetPunchsList(PunchSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<PunchDetailsResponse>("GetPunchsList", queryParameters);
        }
        public async Task<PunchDetailsResponse?> GetPunchDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<PunchDetailsResponse>("GetPunchDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<PunchFailToImportValidationErrors>> ImportPunchsDetails(List<PunchImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlPunchData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlPunchData", xmlPunchData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<PunchFailToImportValidationErrors>("SaveImportPunchDetails", queryParameters);
        }
        #endregion

        #region Surface
        public async Task<int> SaveSurface(SurfaceSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SurfaceId", parameters.SurfaceId);
            queryParameters.Add("@SurfaceName", parameters.SurfaceName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveSurfaceDetails", queryParameters);
        }
        public async Task<IEnumerable<SurfaceDetailsResponse>> GetSurfacesList(SurfaceSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<SurfaceDetailsResponse>("GetSurfacesList", queryParameters);
        }
        public async Task<SurfaceDetailsResponse?> GetSurfaceDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<SurfaceDetailsResponse>("GetSurfaceDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<SurfaceFailToImportValidationErrors>> ImportSurfacesDetails(List<SurfaceImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlSurfaceData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlSurfaceData", xmlSurfaceData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<SurfaceFailToImportValidationErrors>("SaveImportSurfaceDetails", queryParameters);
        }
        #endregion

        #region Thickness
        public async Task<int> SaveThickness(ThicknessSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@ThicknessId", parameters.ThicknessId);
            queryParameters.Add("@ThicknessName", parameters.ThicknessName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveThicknessDetails", queryParameters);
        }
        public async Task<IEnumerable<ThicknessDetailsResponse>> GetThicknessesList(ThicknessSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<ThicknessDetailsResponse>("GetThicknessesList", queryParameters);
        }
        public async Task<ThicknessDetailsResponse?> GetThicknessDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<ThicknessDetailsResponse>("GetThicknessDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<ThicknessFailToImportValidationErrors>> ImportThicknessesDetails(List<ThicknessImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlThicknessData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlThicknessData", xmlThicknessData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<ThicknessFailToImportValidationErrors>("SaveImportThicknessDetails", queryParameters);
        }
        #endregion

        #region TileSize
        public async Task<int> SaveTileSize(TileSizeSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@TileSizeId", parameters.TileSizeId);
            queryParameters.Add("@TileSizeName", parameters.TileSizeName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveTileSizeDetails", queryParameters);
        }
        public async Task<IEnumerable<TileSizeDetailsResponse>> GetTileSizesList(TileSizeSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<TileSizeDetailsResponse>("GetTileSizesList", queryParameters);
        }
        public async Task<TileSizeDetailsResponse?> GetTileSizeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<TileSizeDetailsResponse>("GetTileSizeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<TileSizeFailToImportValidationErrors>> ImportTileSizesDetails(List<TileSizeImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlTileSizeData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlTileSizeData", xmlTileSizeData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<TileSizeFailToImportValidationErrors>("SaveImportTileSizeDetails", queryParameters);
        }
        #endregion

        #region Master Data
        public async Task<IEnumerable<SelectListResponse>> GetSizeForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetSizeMasterForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetBrandForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetBrandMasterForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetCollectionForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetCollectionMasterForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetCategoryForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetCategoryMasterForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetTypeForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetTypeMasterForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetPunchForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetPunchMasterForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetSurfaceForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetSurfaceMasterForSelectList", queryParameters);
        }
        public async Task<IEnumerable<SelectListResponse>> GetThicknessForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetThicknessMasterForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetTileForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetTileSizeMasterForSelectList", queryParameters);
        }
        #endregion

        #region Manage Product Design
        public async Task<IEnumerable<ProductDesignResponse>> GetProductDesignList(ProductDesignSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<ProductDesignResponse>("GetProductDesignList", queryParameters);
        }

        public async Task<ProductDesignDetailsResponse?> GetProductDesignDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<ProductDesignDetailsResponse>("GetProductDesignById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<ProductDesignFilesResponse>> GetProductDesignFiles(long productDesignId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@ProductDesignId", productDesignId);

            return await ListByStoredProcedure<ProductDesignFilesResponse>("GetProductDesignFilesByDesignId", queryParameters);
        }

        public async Task<int> SaveProductDesign(ProductDesignsSaveModel parameters)
        {
            string xmlProductDesignFiles;
            DynamicParameters queryParameters = new DynamicParameters();

            xmlProductDesignFiles = ConvertListToXml(parameters?.ProductDesignFiles);

            queryParameters.Add("@ProductDesignId", parameters.ProductDesignId);
            queryParameters.Add("@DesignName", parameters?.DesignName?.ToUpper());
            queryParameters.Add("@SizeId", parameters?.SizeId);
            queryParameters.Add("@BrandId", parameters?.BrandId);
            queryParameters.Add("@CollectionId", parameters?.CollectionId);
            queryParameters.Add("@CategoryId", parameters?.CategoryId);
            queryParameters.Add("@TypeId", parameters?.TypeId);
            queryParameters.Add("@PunchId", parameters?.PunchId);
            queryParameters.Add("@SurfaceId", parameters?.SurfaceId);
            queryParameters.Add("@ThicknessId", parameters?.ThicknessId);
            queryParameters.Add("@TileSizeId", parameters?.TileSizeId);
            queryParameters.Add("@NoOfTilesPerBox", parameters?.NoOfTilesPerBox);
            queryParameters.Add("@WeightPerBox", parameters?.WeightPerBox);
            queryParameters.Add("@BoxCoverageAreaSqFoot", parameters?.BoxCoverageAreaSqFoot);
            queryParameters.Add("@BoxCoverageAreaSqMeter", parameters?.BoxCoverageAreaSqMeter);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@XmlProductDesignFiles", xmlProductDesignFiles);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveProductDesigns", queryParameters);
        }

        public async Task<IEnumerable<ProductDesignValidationErrors>> ImportProductDesignData(List<ImportProductDesign> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlProductDesignData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlProductDesignData", xmlProductDesignData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<ProductDesignValidationErrors>("SaveImportProductDesign", queryParameters);
        }
        #endregion

        #region Manage Box Size
        public async Task<IEnumerable<ManageBoxSizeResponse>> GetManageBoxSizeList(ManageBoxSizeSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<ManageBoxSizeResponse>("GetManageBoxSizeList", queryParameters);
        }
        public async Task<ManageBoxSizeResponse?> GetManageBoxSizeById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<ManageBoxSizeResponse>("GetManageBoxSizeById", queryParameters)).FirstOrDefault();
        }

        public async Task<int> SaveManageBoxSize(ManageBoxSizeModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BoxSizeId", parameters.BoxSizeId);
            queryParameters.Add("@TileSizeId", parameters.TileSizeId);
            queryParameters.Add("@NoOfTilesPerBox", parameters.NoOfTilesPerBox);
            queryParameters.Add("@WeightPerBox", parameters.WeightPerBox);
            queryParameters.Add("@Thickness", parameters.Thickness);
            queryParameters.Add("@BoxCoverageAreaSqFoot", parameters.BoxCoverageAreaSqFoot);
            queryParameters.Add("@BoxCoverageAreaSqMeter", parameters.BoxCoverageAreaSqMeter);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveManageBoxSize", queryParameters);
        }

        public async Task<IEnumerable<ManageBoxSizeValidationErrors>> ImportManageBoxSize(List<ImportManageBoxSize> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlManageBoxSizeData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlManageBoxSizeData", xmlManageBoxSizeData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<ManageBoxSizeValidationErrors>("SaveImportManageBoxSize", queryParameters);
        }
        #endregion

        #region Blood Group
        public async Task<int> SaveBlood(BloodSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BloodId", parameters.BloodId);
            queryParameters.Add("@BloodGroup", parameters.BloodGroup.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveBloodDetails", queryParameters);
        }
        public async Task<IEnumerable<BloodDetailsResponse>> GetBloodsList(BloodSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<BloodDetailsResponse>("GetBloodsList", queryParameters);
        }
        public async Task<BloodDetailsResponse?> GetBloodDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<BloodDetailsResponse>("GetBloodDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<BloodFailToImportValidationErrors>> ImportBloodsDetails(List<BloodImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlBloodData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<BloodFailToImportValidationErrors>("SaveImportBloodDetails", queryParameters);
        }
        public async Task<IEnumerable<SelectListResponse>> GetBloodForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetBloodMasterForSelectList", queryParameters);
        }
        public async Task<IEnumerable<SelectListResponse>> GetSubVendorForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetSubVendorMasterForSelectList", queryParameters);
        }
        public async Task<IEnumerable<SelectListResponse>> GetContactTypeForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetContactTypeForSelectList", queryParameters);
        }
        public async Task<IEnumerable<SelectListResponse>> GetReferralForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetReferralForSelectList", queryParameters);
        }
        #endregion

        #region TitleType
        public async Task<int> SaveTileType(TileTypeSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@TileTypeId", parameters.TileTypeId);
            queryParameters.Add("@TileType", parameters.TileType.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveTileTypeDetails", queryParameters);
        }
        public async Task<IEnumerable<TileTypeDetailsResponse>> GetTileTypesList(TileTypeSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<TileTypeDetailsResponse>("GetTileTypesList", queryParameters);
        }
        public async Task<TileTypeDetailsResponse?> GetTileTypeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<TileTypeDetailsResponse>("GetTileTypeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<TileTypeFailToImportValidationErrors>> ImportTileTypesDetails(List<TileTypeImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlTileTypeData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<TileTypeFailToImportValidationErrors>("SaveImportTileTypeDetails", queryParameters);
        }
        #endregion

        #region ContactType
        public async Task<int> SaveContactType(ContactTypeSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@ContactTypeId", parameters.ContactTypeId);
            queryParameters.Add("@ContactType", parameters.ContactType.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveContactTypeDetails", queryParameters);
        }
        public async Task<IEnumerable<ContactTypeDetailsResponse>> GetContactTypesList(ContactTypeSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<ContactTypeDetailsResponse>("GetContactTypesList", queryParameters);
        }
        public async Task<ContactTypeDetailsResponse?> GetContactTypeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<ContactTypeDetailsResponse>("GetContactTypeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<ContactTypeFailToImportValidationErrors>> ImportContactTypesDetails(List<ContactTypeImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlContactTypeData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<ContactTypeFailToImportValidationErrors>("SaveImportContactTypeDetails", queryParameters);
        }
        #endregion

        #region WeekClose
        public async Task<int> SaveWeekClose(WeekCloseSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@WeekCloseId", parameters.WeekCloseId);
            queryParameters.Add("@WeekClose", parameters.WeekClose.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveWeekCloseDetails", queryParameters);
        }
        public async Task<IEnumerable<WeekCloseDetailsResponse>> GetWeekClosesList(WeekCloseSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<WeekCloseDetailsResponse>("GetWeekClosesList", queryParameters);
        }
        public async Task<WeekCloseDetailsResponse?> GetWeekCloseDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<WeekCloseDetailsResponse>("GetWeekCloseDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<WeekCloseFailToImportValidationErrors>> ImportWeekClosesDetails(List<WeekCloseImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlWeekCloseData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<WeekCloseFailToImportValidationErrors>("SaveImportWeekCloseDetails", queryParameters);
        }
        #endregion

        #region Gendor
        public async Task<int> SaveGendor(GendorSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@GenderId", parameters.GenderId);
            queryParameters.Add("@Gender", parameters.Gender.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveGenderDetails", queryParameters);
        }
        public async Task<IEnumerable<GendorDetailsResponse>> GetGendorsList(GendorSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<GendorDetailsResponse>("GetGendersList", queryParameters);
        }
        public async Task<GendorDetailsResponse?> GetGendorDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<GendorDetailsResponse>("GetGenderDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<GendorFailToImportValidationErrors>> ImportGendorsDetails(List<GendorImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlGenderData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<GendorFailToImportValidationErrors>("SaveImportGenderDetails", queryParameters);
        }
        #endregion

        #region Role
        public async Task<int> SaveRole(RoleSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RoleId", parameters.RoleId);
            queryParameters.Add("@RoleName", parameters.RoleName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveRoleDetails", queryParameters);
        }

        public async Task<IEnumerable<RoleDetailsResponse>> GetRoleList(RoleSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<RoleDetailsResponse>("GetRoleList", queryParameters);
        }

        public async Task<RoleDetailsResponse?> GetRoleDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<RoleDetailsResponse>("GetRoleDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<RoleFailToImportValidationErrors>> ImportRoleDetails(List<RoleImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlRoleData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlRoleData", xmlRoleData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<RoleFailToImportValidationErrors>("SaveImportRoleDetails", queryParameters);
        }

        #endregion

        #region ReportingHierarchy

        public async Task<IEnumerable<SelectListResponse>> GetRoleHierarchyDetailsByRoleId(long roleId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RoleId", roleId);
            return (await ListByStoredProcedure<SelectListResponse>("GetRoleHierarchyDetailsByRoleId", queryParameters));
        }
        public async Task<int> SaveReportingHierarchy(ReportingHierarchySaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@RoleId", parameters.RoleId);
            queryParameters.Add("@ReportingRoleId", parameters.ReportingRoleId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveReportingHierarchyDetails", queryParameters);
        }


        public async Task<IEnumerable<ReportingHierarchyDetailsResponse>> GetReportingHierarchyList(ReportingHierarchySearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<ReportingHierarchyDetailsResponse>("GetReportingHierarchyList", queryParameters);
        }

        public async Task<ReportingHierarchyDetailsResponse?> GetReportingHierarchyDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<ReportingHierarchyDetailsResponse>("GetReportingHierarchyDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<ReportingHierarchyFailToImportValidationErrors>> ImportReportingHierarchyDetails(List<ReportingHierarchyImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlReportingHierarchyData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlReportingHierarchyData", xmlReportingHierarchyData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<ReportingHierarchyFailToImportValidationErrors>("SaveImportReportingHierarchyDetails", queryParameters);
        }


        #endregion

        #region Employee
        public async Task<int> SaveEmployee(EmployeeSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@EmployeeName", parameters?.EmployeeName?.SanitizeValue().ToUpper());
            queryParameters.Add("@EmployeeCode", parameters?.EmployeeCode?.ToUpper());
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@Email", parameters?.Email);
            queryParameters.Add("@Department", parameters?.Department);
            queryParameters.Add("@Role", parameters?.Role);
            queryParameters.Add("@ReportingTo", parameters?.ReportingTo);
            queryParameters.Add("@DateOfBirth", parameters?.DateOfBirth);
            queryParameters.Add("@DateOfJoining", parameters?.DateOfJoining);
            queryParameters.Add("@EmergencycontactNo", parameters?.EmergencycontactNo);
            queryParameters.Add("@BloodGroup", parameters?.BloodGroup);
            queryParameters.Add("@Gender", parameters?.Gender);
            //queryParameters.Add("@MaterialStatus", parameters.MaterialStatus);
            queryParameters.Add("@CompanyNaumber", parameters?.CompanyNaumber);
            queryParameters.Add("@PermanentAddress", parameters?.PermanentAddress?.SanitizeValue());
            queryParameters.Add("@PermanentState", parameters?.PermanentState);
            queryParameters.Add("@PermanentRegion", parameters?.PermanentRegion);
            queryParameters.Add("@PermanentDistrict", parameters?.PermanentDistrict);
            queryParameters.Add("@PermanentCity", parameters?.PermanentCity);
            queryParameters.Add("@PermanentArea", parameters?.PermanentArea);
            queryParameters.Add("@PermanentPinCode", parameters?.PermanentPinCode);
            queryParameters.Add("@IsTemporaryAddressIsSame", parameters?.IsTemporaryAddressIsSame);
            queryParameters.Add("@TemporaryAddress", parameters?.TemporaryAddress?.SanitizeValue());
            queryParameters.Add("@TemporaryState", parameters?.TemporaryState);
            queryParameters.Add("@TemporaryRegion", parameters?.TemporaryRegion);
            queryParameters.Add("@TemporaryDistrict", parameters?.TemporaryDistrict);
            queryParameters.Add("@TemporaryCity", parameters?.TemporaryCity);
            queryParameters.Add("@TemporaryArea", parameters?.TemporaryArea);
            queryParameters.Add("@TemporaryPinCode", parameters?.TemporaryPinCode);
            queryParameters.Add("@EmergencyName", parameters?.EmergencyName?.SanitizeValue().ToUpper());
            queryParameters.Add("@EmergencyNumber", parameters?.EmergencyNumber);
            queryParameters.Add("@EmergencyRelation", parameters?.EmergencyRelation);
            queryParameters.Add("@EmployeePostCompanyName", parameters?.EmployeePostCompanyName?.SanitizeValue().ToUpper());
            queryParameters.Add("@TotalNumberOfExp", parameters?.TotalNumberOfExp);
            queryParameters.Add("@AddharNumber", parameters?.AddharNumber);
            queryParameters.Add("@UploadAddharCard", parameters?.UploadAddharCardURL);
            queryParameters.Add("@PANNumber", parameters?.PANNumber);
            queryParameters.Add("@OtherProof", parameters?.OtherProof);
            queryParameters.Add("@PhotoUpload", parameters?.PhotoUploadURL);
            queryParameters.Add("@Remark", parameters?.Remark);
            queryParameters.Add("@IsWebUser", parameters?.IsWebUser);
            queryParameters.Add("@Password", EncryptDecryptHelper.EncryptString(parameters.Password));
            queryParameters.Add("@IsMobileUser", parameters?.IsMobileUser);
            queryParameters.Add("@ImageUpload", parameters?.ImageUploadURL);
            queryParameters.Add("@IsActive", parameters?.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveEmployeeDetails", queryParameters);
        }

        public async Task<IEnumerable<EmployeeDetailsResponse>> GetEmployeeList(EmployeeSearchParameters parameters)
        {
            EmployeeDetailsResponse List = new EmployeeDetailsResponse();
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<EmployeeDetailsResponse>("GetEmployeeList", queryParameters);
        }

        public async Task<EmployeeDetailsResponse?> GetEmployeeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<EmployeeDetailsResponse>("GetEmployeeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<EmployeeDetailsResponse>> GetEmployeeListByRoleId(long roleId)
        {
            EmployeeDetailsResponse List = new EmployeeDetailsResponse();
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RoleId", roleId);
            return await ListByStoredProcedure<EmployeeDetailsResponse>("GetEmployeeListByRoleId", queryParameters);
        }

        #endregion

        #region Referral
        public async Task<int> SaveReferral(ReferralSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@UniqueNo", RandomDigits(10));
            queryParameters.Add("@ReferralParty", parameters.ReferralParty.SanitizeValue());
            queryParameters.Add("@Address", parameters.Address.SanitizeValue());
            queryParameters.Add("@State", parameters.State);
            queryParameters.Add("@Region", parameters.Region);
            queryParameters.Add("@District", parameters.District);
            queryParameters.Add("@City", parameters.City);
            queryParameters.Add("@Area", parameters.Area);
            queryParameters.Add("@Pincode", parameters.Pincode);
            queryParameters.Add("@Phone", parameters.Phone.SanitizeValue());
            queryParameters.Add("@Mobile", parameters.Mobile.SanitizeValue());
            queryParameters.Add("@GstNo", parameters.GstNo.SanitizeValue());
            queryParameters.Add("@PanNo", parameters.PanNo.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveReferralMasterDetails", queryParameters);
        }

        public async Task<IEnumerable<ReferralDetailsResponse>> GetReferralList(ReferralSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<ReferralDetailsResponse>("GetReferralList", queryParameters);
        }

        public async Task<ReferralDetailsResponse?> GetReferralDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<ReferralDetailsResponse>("GetReferralDetailsById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<ReferralFailToImportValidationErrors>> ImportReferralDetails(List<ReferralImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlReferralData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlReferralData", xmlReferralData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<ReferralFailToImportValidationErrors>("SaveImportReferralDetails", queryParameters);
        }
        #endregion

        #region TDS Nature
        public async Task<int> SaveCuttingSize(CuttingSizeSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CuttingSizeId", parameters.CuttingSizeId);
            queryParameters.Add("@CuttingSize", parameters.CuttingSize.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveCuttingSizeDetails", queryParameters);
        }
        public async Task<IEnumerable<CuttingSizeDetailsResponse>> GetCuttingSizesList(CuttingSizeSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<CuttingSizeDetailsResponse>("GetCuttingSizesList", queryParameters);
        }
        public async Task<CuttingSizeDetailsResponse?> GetCuttingSizeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<CuttingSizeDetailsResponse>("GetCuttingSizeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<CuttingSizeFailToImportValidationErrors>> ImportCuttingSizeDetails(List<CuttingSizeImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlCuttingSizeData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<CuttingSizeFailToImportValidationErrors>("SaveImportCuttingSizeDetails", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetCuttingSizeForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetCuttingSizeMasterForSelectList", queryParameters);
        }
        #endregion

        #region Page
        public async Task<int> SavePage(PageSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageId", parameters.PageId);
            queryParameters.Add("@PageName", parameters.PageName.SanitizeValue().ToUpper());
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@RoleId", parameters.RoleId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SavePageDetails", queryParameters);
        }
        public async Task<IEnumerable<PageDetailsResponse>> GetPageList(PageSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<PageDetailsResponse>("GetPageList", queryParameters);
        }

        public async Task<PageDetailsResponse?> GetPageDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<PageDetailsResponse>("GetPageDetailsById", queryParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<RollPermissionDetailsResponse>> GetRollPermissionDetailsByRoleId(long roleId)
        {
            LinkedList<Dictionary<string, string>> llist = new LinkedList<Dictionary<string, string>>();
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RoleId", roleId);
            return await ListByStoredProcedure<RollPermissionDetailsResponse>("GetRollPermissionDetailsByRoleId", queryParameters);
        }
        public async Task<IEnumerable<RollPermissionDetailsResponse>> GetRollPermissionList(RollPermissionSearchParameters parameters)
        {
            LinkedList<Dictionary<string, string>> llist = new LinkedList<Dictionary<string, string>>();
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@RoleId", parameters.RoleId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<RollPermissionDetailsResponse>("GetRollPermissionList", queryParameters);
        }
        public async Task<int> UpdateRolePermission(List<RolePermissionUpdateParameters> rolePermission)
        {
            var data=0; 
            if (rolePermission != null)
            {
                for (var i = 0; i < rolePermission.Count; i++)
                {             
                DynamicParameters queryParameters = new DynamicParameters();
                    queryParameters.Add("@Id", rolePermission[i].Id);
                    queryParameters.Add("@RoleId", rolePermission[i].RoleId);
                    queryParameters.Add("@PageId", rolePermission[i].PageId);
                    queryParameters.Add("@ViewData", rolePermission[i].ViewData);
                    queryParameters.Add("@AddData", rolePermission[i].AddData);
                    queryParameters.Add("@EditData", rolePermission[i].EditData);
                    queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
                  // SaveByStoredProcedure<int>("UpdateRolePermissionDetails", queryParameters);
                   int value= await ExecuteNonQuery("UpdateRolePermissionDetails", queryParameters);
                    data = data+1;
                }
            }
            return data;

        }

        public async Task<int> UpdateEmployeePermission(List<EmployeePermissionUpdateParameters> employeePermission)
        {
            var data = 0;
            if (employeePermission != null)
            {
                for (var i = 0; i < employeePermission.Count; i++)
                {
                    DynamicParameters queryParameters = new DynamicParameters();
                    queryParameters.Add("@Id", employeePermission[i].Id);
                    queryParameters.Add("@RoleId", employeePermission[i].RoleId);
                    queryParameters.Add("@EmployeeId", employeePermission[i].EmployeeId);
                    queryParameters.Add("@PageId", employeePermission[i].PageId);
                    queryParameters.Add("@ViewData", employeePermission[i].ViewData);
                    queryParameters.Add("@AddData", employeePermission[i].AddData);
                    queryParameters.Add("@EditData", employeePermission[i].EditData);
                    queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
                    //var valuereturn = SaveByStoredProcedure<int>("UpdateEmployeePermissionDetails", queryParameters);
                    int value = await ExecuteNonQuery("UpdateEmployeePermissionDetails", queryParameters);
                    data = data + 1;
                }
            }
            return data;



        }

        public async Task<IEnumerable<EmployeePermissionDetailsResponse>> GetEmployeePermissionDetailsByEmployeeId(long employeeId)
        {
            LinkedList<Dictionary<string, string>> llist = new LinkedList<Dictionary<string, string>>();
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@EmployeeId", employeeId);
            return await ListByStoredProcedure<EmployeePermissionDetailsResponse>("GetEmployeePermissionDetailsByEmployeeId", queryParameters);
            //return (await ListByStoredProcedure<EmployeePermissionDetailsResponse>("GetEmployeePermissionDetailsByEmployeeId", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<EmployeePermissionDetailsResponse>> GetEmployeePermissionList(EmployeePermissionSearchParameters parameters)
        {
            LinkedList<Dictionary<string, string>> llist = new LinkedList<Dictionary<string, string>>();
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@IsExport", parameters.IsExport);

            return await ListByStoredProcedure<EmployeePermissionDetailsResponse>("GetEmployeePermissionList", queryParameters);
        }
        #endregion
    }
}
