using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using static CasaAPI.Models.BloodModel;
using static CasaAPI.Models.BrandModel;
using static CasaAPI.Models.CategoryModel;
using static CasaAPI.Models.Collection_PanelModel;
using static CasaAPI.Models.ContactTypeModel;
using static CasaAPI.Models.GendorModel;
using static CasaAPI.Models.PunchModel;
using static CasaAPI.Models.SurfaceModel;
using static CasaAPI.Models.ThicknessModel;
using static CasaAPI.Models.TileSizeModel;
using static CasaAPI.Models.TileTypeModel;
using static CasaAPI.Models.TypeModel;
using static CasaAPI.Models.PanelTypeModel;
using static CasaAPI.Models.WeekCloseModel;
using static CasaAPI.Models.CuttingSizeModel;
using Newtonsoft.Json.Linq;
using static CasaAPI.Models.FoldModel;
using static CasaAPI.Models.FlapModel;
using static CasaAPI.Models.TitleGSMModel;
using static CasaAPI.Models.FlapGSMModel;
using static CasaAPI.Models.InnerGSMModel;
using static CasaAPI.Models.TitleProcessModel;

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

        #region Collection Panel
        public async Task<int> SaveCollection_Panel(Collection_PanelSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CollectionId", parameters.CollectionId);
            queryParameters.Add("@CollectionName", parameters.CollectionName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveCollectionDetails_Panel", queryParameters);
        }
        public async Task<IEnumerable<CollectionDetails_PanelResponse>> GetCollectionsList_Panel(Collection_PanelSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<CollectionDetails_PanelResponse>("GetCollectionsList_Panel", queryParameters);
        }
        public async Task<CollectionDetails_PanelResponse?> GetCollectionDetailsById_Panel(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<CollectionDetails_PanelResponse>("GetCollectionDetailsById_Panel", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<Collection_PanelFailToImportValidationErrors>> ImportCollectionsDetails_Panel(List<Collection_PanelImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlCollectionData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlCollectionData", xmlCollectionData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<Collection_PanelFailToImportValidationErrors>("SaveImportCollectionDetails_Panel", queryParameters);
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

        #region Panel Type
        public async Task<int> SavePanelType(PanelTypeSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PanelTypeId", parameters.PanelTypeId);
            queryParameters.Add("@PanelTypeName", parameters.PanelTypeName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SavePanelTypeDetails", queryParameters);
        }
        public async Task<IEnumerable<PanelTypeDetailsResponse>> GetPanelTypesList(PanelTypeSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<PanelTypeDetailsResponse>("GetPanelTypesList", queryParameters);
        }
        public async Task<PanelTypeDetailsResponse?> GetPanelTypeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<PanelTypeDetailsResponse>("GetPanelTypeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<PanelTypeFailToImportValidationErrors>> ImportPanelTypesDetails(List<PanelTypeImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlPanelTypeData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlPanelTypeData", xmlPanelTypeData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<PanelTypeFailToImportValidationErrors>("SaveImportPanelTypeDetails", queryParameters);
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

        public async Task<IEnumerable<SelectListResponse>> GetCollection_PanelForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetCollection_PanelForSelectList", queryParameters);
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

        public async Task<IEnumerable<SelectListResponse>> GetCuttingPlanForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetCuttingPlanForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetProductDesignForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetProductDesignForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetTileSizeForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetTileSizeForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetDriverNameForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetDriverNameForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetFinishNameForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetFinishNameForSelectList", queryParameters);
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
            queryParameters.Add("@FinishName", parameters.FinishName);
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
            queryParameters.Add("@AadharFileName", parameters.AadharFileName.SanitizeValue());
            queryParameters.Add("@AadharSaveFileName", parameters.AadharSaveFileName.SanitizeValue());
            queryParameters.Add("@PanCardFileName", parameters.PanCardFileName.SanitizeValue());
            queryParameters.Add("@PanCardSaveFileName", parameters.PanCardSaveFileName.SanitizeValue());
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

        #region sales

        public async Task<IEnumerable<Users>> GetUsersList()
        {
            return await ListByStoredProcedure<Users>("");
        }

        #region Product API Repository
        public async Task<IEnumerable<ProductResponse>> GetProductsList(SearchProductRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ProductName", parameters.ProductName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            var result = await ListByStoredProcedure<ProductResponse>("GetProducts", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<int> SaveProduct(ProductRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@ProductId", parameters.ProductId);
            queryParameters.Add("@ProductName", parameters.ProductName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveProductDetails", queryParameters);
        }
        public async Task<ProductResponse?> GetProductDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<ProductResponse>("GetProductDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<ProductDataValidationErrors>> ImportProductsDetails(List<ImportedProductDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlProductData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlProductData", xmlProductData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<ProductDataValidationErrors>("SaveImportProductDetails", queryParameters);
        }
        #endregion Product API Repository

        #region Design Type API Repository
        public async Task<IEnumerable<DesignTypeResponse>> GetDesignTypesList(SearchDesignTypeRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@DesignTypeName", parameters.DesignTypeName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            var result = await ListByStoredProcedure<DesignTypeResponse>("GetDesignTypes", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<int> SaveDesignType(DesignTypeRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@DesignTypeId", parameters.DesignTypeId);
            queryParameters.Add("@DesignTypeName", parameters.DesignTypeName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveDesignTypeDetails", queryParameters);
        }

        public async Task<DesignTypeResponse?> GetDesignTypeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<DesignTypeResponse>("GetDesignTypeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<DesignTypeDataValidationErrors>> ImportDesignTypesDetails(List<ImportedDesignTypeDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlDesignTypeData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlDesignTypeData", xmlDesignTypeData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<DesignTypeDataValidationErrors>("SaveImportDesignTypeDetails", queryParameters);
        }
        #endregion Design Type API Repository

        #region Series API Repository
        public async Task<IEnumerable<SeriesResponse>> GetSeriesList(SearchSeriesRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@SeriesName", parameters.SeriesName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            var result = await ListByStoredProcedure<SeriesResponse>("GetSeriess", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<int> SaveSeries(SeriesRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SeriesId", parameters.SeriesId);
            queryParameters.Add("@SeriesName", parameters.SeriesName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveSeriesDetails", queryParameters);
        }

        public async Task<SeriesResponse?> GetSeriesDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<SeriesResponse>("GetSeriesDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<SeriesDataValidationErrors>> ImportSeriesDetails(List<ImportedSeriesDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlSeriesData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlSeriesData", xmlSeriesData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<SeriesDataValidationErrors>("SaveImportSeriesDetails", queryParameters);
        }
        #endregion Series API Repository

        #region Base Design API Repository
        public async Task<IEnumerable<BaseDesignResponse>> GetBaseDesignsList(SearchBaseDesignRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@BaseDesignName", parameters.BaseDesignName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            var result = await ListByStoredProcedure<BaseDesignResponse>("GetBaseDesigns", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<int> SaveBaseDesign(BaseDesignRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BaseDesignId", parameters.BaseDesignId);
            queryParameters.Add("@BaseDesignName", parameters.BaseDesignName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveBaseDesignDetails", queryParameters);
        }
        public async Task<BaseDesignResponse?> GetBaseDesignDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<BaseDesignResponse>("GetBaseDesignDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<BaseDesignDataValidationErrors>> ImportBaseDesignsDetails(List<ImportedBaseDesignDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBaseDesignData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlBaseDesignData", xmlBaseDesignData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<BaseDesignDataValidationErrors>("SaveImportBaseDesignDetails", queryParameters);
        }
        #endregion Base Design API Repository

        #region Customer Type API Repository
        public async Task<IEnumerable<CustomerTypeResponse>> GetCustomerTypesList(SearchCustomerTypeRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@CustomerTypeName", parameters.CustomerTypeName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            var result = await ListByStoredProcedure<CustomerTypeResponse>("GetCustomerTypes", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<int> SaveCustomerType(CustomerTypeRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerTypeId", parameters.CustomerTypeId);
            queryParameters.Add("@CustomerTypeName", parameters.CustomerTypeName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveCustomerTypeDetails", queryParameters);
        }
        public async Task<CustomerTypeResponse?> GetCustomerTypeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<CustomerTypeResponse>("GetCustomerTypeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<CustomerTypeDataValidationErrors>> ImportCustomerTypesDetails(List<ImportedCustomerTypeDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlCustomerTypeData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlCustomerTypeData", xmlCustomerTypeData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<CustomerTypeDataValidationErrors>("SaveImportCustomerTypeDetails", queryParameters);
        }
        #endregion Customer Type API Repository

        #region Leave Type API Repository
        public async Task<IEnumerable<LeaveTypeResponse>> GetLeaveTypesList(SearchLeaveTypeRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@LeaveTypeName", parameters.LeaveTypeName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            var result = await ListByStoredProcedure<LeaveTypeResponse>("GetLeaveTypes", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<int> SaveLeaveType(LeaveTypeRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@LeaveTypeId", parameters.LeaveTypeId);
            queryParameters.Add("@LeaveTypeName", parameters.LeaveTypeName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveLeaveTypeDetails", queryParameters);
        }
        public async Task<LeaveTypeResponse?> GetLeaveTypeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);
            return (await ListByStoredProcedure<LeaveTypeResponse>("GetLeaveTypeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<LeaveTypeDataValidationErrors>> ImportLeaveTypesDetails(List<ImportedLeaveTypeDetails> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlLeaveTypeData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlLeaveTypeData", xmlLeaveTypeData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<LeaveTypeDataValidationErrors>("SaveImportLeaveTypeDetails", queryParameters);
        }
        #endregion Leave Type API Repository

        #region Master Data
        public async Task<IEnumerable<SelectListResponse>> GetCustomerTypesForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<SelectListResponse>("GetCustomerTypesForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetCustomersForSelectList(CustomerSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerTypeId", parameters.CustomerTypeId.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<SelectListResponse>("GetCustomersForSelectList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetStatusMasterForSelectList(string StatusCode)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@StatusCode", StatusCode);

            return await ListByStoredProcedure<SelectListResponse>("GetStatusMasterList", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetReportingToEmployeeForSelectList(ReportingToEmpListParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@RoleId", parameters.RoleId);
            queryParameters.Add("@RegionId", parameters.RegionId.SanitizeValue());

            return await ListByStoredProcedure<SelectListResponse>("GetReportingToEmployeeForSelectList", queryParameters);
        }

        public async Task<IEnumerable<CustomerContactsListForFields>> GetCustomerContactsListForFields(CustomerContactsListRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerId", parameters.CustomerId);
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<CustomerContactsListForFields>("GetCustomerContactsList", queryParameters);
        }
        #endregion

        #region Blood Group Master
        public async Task<int> SaveBloodGroupDetails(BloodGroupRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BloodGroupId", parameters.BloodGroupId);
            queryParameters.Add("@BloodGroup", parameters.BloodGroupName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveBloodGroupDetails", queryParameters);
        }

        public async Task<IEnumerable<BloodGroupResponseModel>> GetBloodGroupList(SearchBloodGroupRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            //queryParameters.Add("@Total", parameters.pagination.Total);
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@BloodGroup", parameters.BloodGroup.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            var result = await ListByStoredProcedure<BloodGroupResponseModel>("GetBloodGroupMasterList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<BloodGroupResponseModel?> GetBloodGroupDetails(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BloodGroupId", id);
            return (await ListByStoredProcedure<BloodGroupResponseModel?>("GetBloodGroupDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region Collection Master
        public async Task<int> SaveCollectionMasterDetails(SaveCollectionRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CollectionId", parameters.CollectionId);
            queryParameters.Add("@CollectionName", parameters.CollectionName.SanitizeValue());
            queryParameters.Add("@CollectionNameId", parameters.CollectionNameId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveCollectionDetails", queryParameters);
        }

        public async Task<IEnumerable<CollectionResponseModel>> GetCollectionMasterList(SearchCollectionRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@CollectionName", parameters.CollectionName.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            var result = await ListByStoredProcedure<CollectionResponseModel>("GetCollectionsList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<CollectionResponseModel?> GetCollectionMasterDetails(int id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CollectionId", id);
            return (await ListByStoredProcedure<CollectionResponseModel?>("GetCollectionDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<CollectionDataValidationErrors>> ImportCollection(List<ImportedCollection> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlCategoryData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlCategoryData", xmlCategoryData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<CollectionDataValidationErrors>("SaveImportCollection", queryParameters);
        }
        #endregion

        #endregion

        #region Fold
        public async Task<int> SaveFold(FoldSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FoldId", parameters.FoldId);
            queryParameters.Add("@FoldName", parameters.FoldName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveFoldDetails", queryParameters);
        }
        public async Task<IEnumerable<FoldDetailsResponse>> GetFoldList(FoldSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<FoldDetailsResponse>("GetFoldList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<FoldDetailsResponse?> GetFoldDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<FoldDetailsResponse>("GetFoldDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<FoldFailToImportValidationErrors>> ImportFoldsDetails(List<FoldImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlFoldData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlFoldData", xmlFoldData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<FoldFailToImportValidationErrors>("SaveImportFoldDetails", queryParameters);
        }
        #endregion

        #region Flap
        public async Task<int> SaveFlap(FlapSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FlapId", parameters.FlapId);
            queryParameters.Add("@FlapName", parameters.FlapName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveFlapDetails", queryParameters);
        }
        public async Task<IEnumerable<FlapDetailsResponse>> GetFlapList(FlapSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<FlapDetailsResponse>("GetFlapList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<FlapDetailsResponse?> GetFlapDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<FlapDetailsResponse>("GetFlapDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<FlapFailToImportValidationErrors>> ImportFlapsDetails(List<FlapImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlFlapData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlFlapData", xmlFlapData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<FlapFailToImportValidationErrors>("SaveImportFlapDetails", queryParameters);
        }
        #endregion

        #region Title GSM
        public async Task<int> SaveTitleGSM(TitleGSMSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@TitleGSMId", parameters.TitleGSMId);
            queryParameters.Add("@TitleGSMName", parameters.TitleGSMName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveTitleGSMDetails", queryParameters);
        }
        public async Task<IEnumerable<TitleGSMDetailsResponse>> GetTitleGSMList(TitleGSMSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<TitleGSMDetailsResponse>("GetTitleGSMList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<TitleGSMDetailsResponse?> GetTitleGSMDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<TitleGSMDetailsResponse>("GetTitleGSMDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<TitleGSMFailToImportValidationErrors>> ImportTitleGSMsDetails(List<TitleGSMImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlTitleGSMData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlTitleGSMData", xmlTitleGSMData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<TitleGSMFailToImportValidationErrors>("SaveImportTitleGSMDetails", queryParameters);
        }
        #endregion

        #region Flap GSM
        public async Task<int> SaveFlapGSM(FlapGSMSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@FlapGSMId", parameters.FlapGSMId);
            queryParameters.Add("@FlapGSMName", parameters.FlapGSMName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveFlapGSMDetails", queryParameters);
        }
        public async Task<IEnumerable<FlapGSMDetailsResponse>> GetFlapGSMList(FlapGSMSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<FlapGSMDetailsResponse>("GetFlapGSMList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<FlapGSMDetailsResponse?> GetFlapGSMDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<FlapGSMDetailsResponse>("GetFlapGSMDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<FlapGSMFailToImportValidationErrors>> ImportFlapGSMsDetails(List<FlapGSMImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlFlapGSMData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlFlapGSMData", xmlFlapGSMData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<FlapGSMFailToImportValidationErrors>("SaveImportFlapGSMDetails", queryParameters);
        }
        #endregion

        #region Inner GSM
        public async Task<int> SaveInnerGSM(InnerGSMSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@InnerGSMId", parameters.InnerGSMId);
            queryParameters.Add("@InnerGSMName", parameters.InnerGSMName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveInnerGSMDetails", queryParameters);
        }
        public async Task<IEnumerable<InnerGSMDetailsResponse>> GetInnerGSMList(InnerGSMSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<InnerGSMDetailsResponse>("GetInnerGSMList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<InnerGSMDetailsResponse?> GetInnerGSMDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<InnerGSMDetailsResponse>("GetInnerGSMDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<InnerGSMFailToImportValidationErrors>> ImportInnerGSMsDetails(List<InnerGSMImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlInnerGSMData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlInnerGSMData", xmlInnerGSMData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<InnerGSMFailToImportValidationErrors>("SaveImportInnerGSMDetails", queryParameters);
        }
        #endregion

        #region Title Process
        public async Task<int> SaveTitleProcess(TitleProcessSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@TitleProcessId", parameters.TitleProcessId);
            queryParameters.Add("@TitleProcessName", parameters.TitleProcessName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveTitleProcessDetails", queryParameters);
        }
        public async Task<IEnumerable<TitleProcessDetailsResponse>> GetTitleProcessList(TitleProcessSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<TitleProcessDetailsResponse>("GetTitleProcessList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<TitleProcessDetailsResponse?> GetTitleProcessDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<TitleProcessDetailsResponse>("GetTitleProcessDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<TitleProcessFailToImportValidationErrors>> ImportTitleProcesssDetails(List<TitleProcessImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlTitleProcessData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlTitleProcessData", xmlTitleProcessData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<TitleProcessFailToImportValidationErrors>("SaveImportTitleProcessDetails", queryParameters);
        }
        #endregion

        #region Expense Type
        public async Task<int> SaveExpenseType(ExpenseTypeRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@ExpenseTypeId", parameters.ExpenseTypeId);
            queryParameters.Add("@ExpenseTypeName", parameters.ExpenseTypeName.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveExpenseType", queryParameters);
        }
        public async Task<IEnumerable<ExpenseTypeResponse>> GetExpenseTypeList(SearchExpenseTypeRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@Total", parameters.pagination.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<ExpenseTypeResponse>("GetExpenseTypeList", queryParameters);
            parameters.pagination.Total = queryParameters.Get<int>("Total");

            return result;
        }
        public async Task<ExpenseTypeResponse?> GetExpenseTypeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<ExpenseTypeResponse>("GetExpenseTypeDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion
    }
}
