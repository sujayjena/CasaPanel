using CasaAPI.Models;
using static CasaAPI.Models.BloodModel;
using static CasaAPI.Models.BrandModel;
using static CasaAPI.Models.CategoryModel;
using static CasaAPI.Models.Collection_PanelModel;
using static CasaAPI.Models.ContactTypeModel;
using static CasaAPI.Models.CuttingSizeModel;
using static CasaAPI.Models.FlapGSMModel;
using static CasaAPI.Models.FlapModel;
using static CasaAPI.Models.FoldModel;
using static CasaAPI.Models.GendorModel;
using static CasaAPI.Models.PanelTypeModel;
using static CasaAPI.Models.PunchModel;
using static CasaAPI.Models.SurfaceModel;
using static CasaAPI.Models.ThicknessModel;
using static CasaAPI.Models.TileSizeModel;
using static CasaAPI.Models.TileTypeModel;
using static CasaAPI.Models.TitleGSMModel;
using static CasaAPI.Models.TypeModel;
using static CasaAPI.Models.WeekCloseModel;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        #region Size
        Task<int> SaveSize(SizeSaveParameters parameters);
        Task<IEnumerable<SizeDetailsResponse>> GetSizesList(SizeSearchParameters parameters);
        Task<SizeDetailsResponse?> GetSizeDetailsById(long id);
        Task<IEnumerable<SizeFailToImportValidationErrors>> ImportSizesDetails(List<SizeImportSaveParameters> parameters);
        #endregion

        #region Brand
        Task<int> SaveBrand(BrandSaveParameters parameters);
        Task<IEnumerable<BrandDetailsResponse>> GetBrandsList(BrandSearchParameters parameters);
        Task<BrandDetailsResponse?> GetBrandDetailsById(long id);
        Task<IEnumerable<BrandFailToImportValidationErrors>> ImportBrandsDetails(List<BrandImportSaveParameters> parameters);

        #endregion

        #region Collection Panel
        Task<int> SaveCollection_Panel(Collection_PanelSaveParameters parameters);
        Task<IEnumerable<CollectionDetails_PanelResponse>> GetCollectionsList_Panel(Collection_PanelSearchParameters parameters);
        Task<CollectionDetails_PanelResponse?> GetCollectionDetailsById_Panel(long id);
        Task<IEnumerable<Collection_PanelFailToImportValidationErrors>> ImportCollectionsDetails_Panel(List<Collection_PanelImportSaveParameters> parameters);

        #endregion

        #region Category
        Task<int> SaveCategory(CategorySaveParameters parameters);
        Task<IEnumerable<CategoryDetailsResponse>> GetCategorysList(CategorySearchParameters parameters);
        Task<CategoryDetailsResponse?> GetCategoryDetailsById(long id);
        Task<IEnumerable<CategoryFailToImportValidationErrors>> ImportCategorysDetails(List<CategoryImportSaveParameters> parameters);

        #endregion

        #region Type
        Task<int> SaveType(TypeSaveParameters parameters);
        Task<IEnumerable<TypeDetailsResponse>> GetTypesList(TypeSearchParameters parameters);
        Task<TypeDetailsResponse?> GetTypeDetailsById(long id);
        Task<IEnumerable<TypeFailToImportValidationErrors>> ImportTypesDetails(List<TypeImportSaveParameters> parameters);

        #endregion

        #region Panel Type
        Task<int> SavePanelType(PanelTypeSaveParameters parameters);
        Task<IEnumerable<PanelTypeDetailsResponse>> GetPanelTypesList(PanelTypeSearchParameters parameters);
        Task<PanelTypeDetailsResponse?> GetPanelTypeDetailsById(long id);
        Task<IEnumerable<PanelTypeFailToImportValidationErrors>> ImportPanelTypesDetails(List<PanelTypeImportSaveParameters> parameters);

        #endregion

        #region Punch
        Task<int> SavePunch(PunchSaveParameters parameters);
        Task<IEnumerable<PunchDetailsResponse>> GetPunchsList(PunchSearchParameters parameters);
        Task<PunchDetailsResponse?> GetPunchDetailsById(long id);
        Task<IEnumerable<PunchFailToImportValidationErrors>> ImportPunchsDetails(List<PunchImportSaveParameters> parameters);

        #endregion

        #region Surface
        Task<int> SaveSurface(SurfaceSaveParameters parameters);
        Task<IEnumerable<SurfaceDetailsResponse>> GetSurfacesList(SurfaceSearchParameters parameters);
        Task<SurfaceDetailsResponse?> GetSurfaceDetailsById(long id);
        Task<IEnumerable<SurfaceFailToImportValidationErrors>> ImportSurfacesDetails(List<SurfaceImportSaveParameters> parameters);

        #endregion

        #region Thickness
        Task<int> SaveThickness(ThicknessSaveParameters parameters);
        Task<IEnumerable<ThicknessDetailsResponse>> GetThicknessesList(ThicknessSearchParameters parameters);
        Task<ThicknessDetailsResponse?> GetThicknessDetailsById(long id);
        Task<IEnumerable<ThicknessFailToImportValidationErrors>> ImportThicknessesDetails(List<ThicknessImportSaveParameters> parameters);

        #endregion

        #region TileSize
        Task<int> SaveTileSize(TileSizeSaveParameters parameters);
        Task<IEnumerable<TileSizeDetailsResponse>> GetTileSizesList(TileSizeSearchParameters parameters);
        Task<TileSizeDetailsResponse?> GetTileSizeDetailsById(long id);
        Task<IEnumerable<TileSizeFailToImportValidationErrors>> ImportTileSizesDetails(List<TileSizeImportSaveParameters> parameters);

        #endregion

        #region Master Data
        Task<IEnumerable<SelectListResponse>> GetSizeForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetBrandForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetCollection_PanelForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetCollectionForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetCategoryForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetTypeForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetPunchForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetSurfaceForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetThicknessForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetTileForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetCuttingPlanForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetProductDesignForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetTileSizeForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetDriverNameForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetFinishNameForSelectList(CommonSelectListRequestModel parameters);
        #endregion

        #region Manage Product Design
        Task<IEnumerable<ProductDesignResponse>> GetProductDesignList(ProductDesignSearchParameters request);
        Task<ProductDesignDetailsResponse?> GetProductDesignDetailsById(long id);
        Task<IEnumerable<ProductDesignFilesResponse>> GetProductDesignFiles(long productDesignId);
        Task<int> SaveProductDesign(ProductDesignsSaveModel parameters);
        Task<IEnumerable<ProductDesignValidationErrors>> ImportProductDesignData(List<ImportProductDesign> parameters);
        #endregion

        #region Manage Box Size
        Task<IEnumerable<ManageBoxSizeResponse>> GetManageBoxSizeList(ManageBoxSizeSearchParameters parameters);
        Task<ManageBoxSizeResponse?> GetManageBoxSizeById(long id);
        Task<int> SaveManageBoxSize(ManageBoxSizeModel parameters);
        Task<IEnumerable<ManageBoxSizeValidationErrors>> ImportManageBoxSize(List<ImportManageBoxSize> parameters);
        #endregion

        Task<IEnumerable<SelectListResponse>> GetSubVendorForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetContactTypeForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetReferralForSelectList(CommonSelectListRequestModel parameters);

        #region TileType
        Task<int> SaveTileType(TileTypeSaveParameters parameters);
        Task<IEnumerable<TileTypeDetailsResponse>> GetTileTypesList(TileTypeSearchParameters parameters);
        Task<TileTypeDetailsResponse?> GetTileTypeDetailsById(long id);
        Task<IEnumerable<TileTypeFailToImportValidationErrors>> ImportTileTypesDetails(List<TileTypeImportSaveParameters> parameters);

        #endregion

        #region ContactType
        Task<int> SaveContactType(ContactTypeSaveParameters parameters);
        Task<IEnumerable<ContactTypeDetailsResponse>> GetContactTypesList(ContactTypeSearchParameters parameters);
        Task<ContactTypeDetailsResponse?> GetContactTypeDetailsById(long id);
        Task<IEnumerable<ContactTypeFailToImportValidationErrors>> ImportContactTypesDetails(List<ContactTypeImportSaveParameters> parameters);

        #endregion

        #region WeekClose
        Task<int> SaveWeekClose(WeekCloseSaveParameters parameters);
        Task<IEnumerable<WeekCloseDetailsResponse>> GetWeekClosesList(WeekCloseSearchParameters parameters);
        Task<WeekCloseDetailsResponse?> GetWeekCloseDetailsById(long id);
        Task<IEnumerable<WeekCloseFailToImportValidationErrors>> ImportWeekClosesDetails(List<WeekCloseImportSaveParameters> parameters);

        #endregion

        #region Gendor
        Task<int> SaveGendor(GendorSaveParameters parameters);
        Task<IEnumerable<GendorDetailsResponse>> GetGendorsList(GendorSearchParameters parameters);
        Task<GendorDetailsResponse?> GetGendorDetailsById(long id);
        Task<IEnumerable<GendorFailToImportValidationErrors>> ImportGendorsDetails(List<GendorImportSaveParameters> parameters);

        #endregion

        #region Cutting Size
        Task<int> SaveCuttingSize(CuttingSizeSaveParameters request);
        Task<IEnumerable<CuttingSizeDetailsResponse>> GetCuttingSizesList(CuttingSizeSearchParameters request);
        Task<CuttingSizeDetailsResponse?> GetCuttingSizeDetailsById(long id);
        Task<IEnumerable<CuttingSizeFailToImportValidationErrors>> ImportCuttingSizeDetails(List<CuttingSizeImportSaveParameters> request);
        Task<IEnumerable<SelectListResponse>> GetCuttingSizeForSelectList(CommonSelectListRequestModel parameters);
        #endregion

        #region Referral
        Task<int> SaveReferral(ReferralSaveParameters parameters);
        Task<IEnumerable<ReferralDetailsResponse>> GetReferralList(ReferralSearchParameters parameters);
        Task<ReferralDetailsResponse?> GetReferralDetailsById(long id);

        Task<IEnumerable<ReferralFailToImportValidationErrors>> ImportReferralDetails(List<ReferralImportSaveParameters> parameters);
        #endregion

        #region Page

        Task<int> SavePage(PageSaveParameters parameters);
        Task<IEnumerable<PageDetailsResponse>> GetPageList(PageSearchParameters parameters);
        Task<PageDetailsResponse?> GetPageDetailsById(long id);
        Task<IEnumerable<RollPermissionDetailsResponse>> GetRollPermissionDetailsByRoleId(long roleId);
        Task<IEnumerable<RollPermissionDetailsResponse>> GetRollPermissionList(RollPermissionSearchParameters parameters);
        Task<int> UpdateRolePermission(List<RolePermissionUpdateParameters> rolePermission);

        Task<int> UpdateEmployeePermission(List<EmployeePermissionUpdateParameters> employeePermission);
        Task<IEnumerable<EmployeePermissionDetailsResponse>> GetEmployeePermissionDetailsByEmployeeId(long employeeId);

        Task<IEnumerable<EmployeePermissionDetailsResponse>> GetEmployeePermissionList(EmployeePermissionSearchParameters parameters);
        #endregion

        #region sales
        Task<IEnumerable<Users>> GetUsersList();

        #region Product Repository Interface
        Task<IEnumerable<ProductResponse>> GetProductsList(SearchProductRequest request);
        Task<int> SaveProduct(ProductRequest productRequest);
        Task<ProductResponse?> GetProductDetailsById(long id);
        Task<IEnumerable<ProductDataValidationErrors>> ImportProductsDetails(List<ImportedProductDetails> parameters);


        #endregion Product Repository Interface

        #region Design Type Repository Interface
        Task<IEnumerable<DesignTypeResponse>> GetDesignTypesList(SearchDesignTypeRequest request);
        Task<int> SaveDesignType(DesignTypeRequest designTypeRequest);
        Task<DesignTypeResponse?> GetDesignTypeDetailsById(long id);
        Task<IEnumerable<DesignTypeDataValidationErrors>> ImportDesignTypesDetails(List<ImportedDesignTypeDetails> parameters);
        #endregion Design Type Repository Interface

        #region Series Repository Interface
        Task<IEnumerable<SeriesResponse>> GetSeriesList(SearchSeriesRequest request);
        Task<int> SaveSeries(SeriesRequest seriesRequest);
        Task<SeriesResponse?> GetSeriesDetailsById(long id);
        Task<IEnumerable<SeriesDataValidationErrors>> ImportSeriesDetails(List<ImportedSeriesDetails> parameters);
        #endregion Series Repository Interface

        #region Base Design Repository Interface
        Task<IEnumerable<BaseDesignResponse>> GetBaseDesignsList(SearchBaseDesignRequest request);
        Task<int> SaveBaseDesign(BaseDesignRequest baseDesignRequest);
        Task<BaseDesignResponse?> GetBaseDesignDetailsById(long id);
        Task<IEnumerable<BaseDesignDataValidationErrors>> ImportBaseDesignsDetails(List<ImportedBaseDesignDetails> parameters);
        #endregion Base Design Repository Interface

        #region Customer Type Repository Interface
        Task<IEnumerable<CustomerTypeResponse>> GetCustomerTypesList(SearchCustomerTypeRequest request);
        Task<int> SaveCustomerType(CustomerTypeRequest customerTypeRequest);
        Task<CustomerTypeResponse?> GetCustomerTypeDetailsById(long id);
        Task<IEnumerable<CustomerTypeDataValidationErrors>> ImportCustomerTypesDetails(List<ImportedCustomerTypeDetails> parameters);
        #endregion  Customer Type Repository Interface

        #region Leave Type Repository Interface
        Task<IEnumerable<LeaveTypeResponse>> GetLeaveTypesList(SearchLeaveTypeRequest request);
        Task<int> SaveLeaveType(LeaveTypeRequest customTypeRequest);
        Task<LeaveTypeResponse?> GetLeaveTypeDetailsById(long id);
        Task<IEnumerable<LeaveTypeDataValidationErrors>> ImportLeaveTypesDetails(List<ImportedLeaveTypeDetails> parameters);
        #endregion  Leave Type Repository Interface

        #region Master Data
        Task<IEnumerable<SelectListResponse>> GetCustomerTypesForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetCustomersForSelectList(CustomerSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetStatusMasterForSelectList(string StatusCode);
        Task<IEnumerable<SelectListResponse>> GetReportingToEmployeeForSelectList(ReportingToEmpListParameters parameters);
        Task<IEnumerable<CustomerContactsListForFields>> GetCustomerContactsListForFields(CustomerContactsListRequest parameters);
        #endregion

        #region Blood Group Master
        Task<int> SaveBloodGroupDetails(BloodGroupRequestModel parameters);
        Task<IEnumerable<BloodGroupResponseModel>> GetBloodGroupList(SearchBloodGroupRequestModel parameters);
        Task<BloodGroupResponseModel?> GetBloodGroupDetails(long id);
        #endregion

        #region Collection Master
        Task<int> SaveCollectionMasterDetails(SaveCollectionRequestModel parameters);
        Task<IEnumerable<CollectionResponseModel>> GetCollectionMasterList(SearchCollectionRequestModel parameters);
        Task<CollectionResponseModel?> GetCollectionMasterDetails(int id);
        Task<IEnumerable<CollectionDataValidationErrors>> ImportCollection(List<ImportedCollection> parameters);
        #endregion

        #endregion

        #region Fold
        Task<int> SaveFold(FoldSaveParameters parameters);
        Task<IEnumerable<FoldDetailsResponse>> GetFoldList(FoldSearchParameters parameters);
        Task<FoldDetailsResponse?> GetFoldDetailsById(long id);
        Task<IEnumerable<FoldFailToImportValidationErrors>> ImportFoldsDetails(List<FoldImportSaveParameters> parameters);

        #endregion

        #region Flap
        Task<int> SaveFlap(FlapSaveParameters parameters);
        Task<IEnumerable<FlapDetailsResponse>> GetFlapList(FlapSearchParameters parameters);
        Task<FlapDetailsResponse?> GetFlapDetailsById(long id);
        Task<IEnumerable<FlapFailToImportValidationErrors>> ImportFlapsDetails(List<FlapImportSaveParameters> parameters);

        #endregion

        #region Title GSM
        Task<int> SaveTitleGSM(TitleGSMSaveParameters parameters);
        Task<IEnumerable<TitleGSMDetailsResponse>> GetTitleGSMList(TitleGSMSearchParameters parameters);
        Task<TitleGSMDetailsResponse?> GetTitleGSMDetailsById(long id);
        Task<IEnumerable<TitleGSMFailToImportValidationErrors>> ImportTitleGSMsDetails(List<TitleGSMImportSaveParameters> parameters);

        #endregion

        #region Flap GSM
        Task<int> SaveFlapGSM(FlapGSMSaveParameters parameters);
        Task<IEnumerable<FlapGSMDetailsResponse>> GetFlapGSMList(FlapGSMSearchParameters parameters);
        Task<FlapGSMDetailsResponse?> GetFlapGSMDetailsById(long id);
        Task<IEnumerable<FlapGSMFailToImportValidationErrors>> ImportFlapGSMsDetails(List<FlapGSMImportSaveParameters> parameters);

        #endregion
    }
}
