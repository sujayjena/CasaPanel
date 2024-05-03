using CasaAPI.Models;
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
using static CasaAPI.Models.WeekCloseModel;
using static CasaAPI.Models.CuttingSizeModel;
using static CasaAPI.Models.PanelTypeModel;
using static CasaAPI.Models.FoldModel;
using static CasaAPI.Models.FlapModel;
using static CasaAPI.Models.TitleGSMModel;
using static CasaAPI.Models.FlapGSMModel;
using static CasaAPI.Models.InnerGSMModel;
using static CasaAPI.Models.TitleProcessModel;

namespace CasaAPI.Interfaces.Services
{
    public interface IAdminService
    {
        #region Size
        Task<int> SaveSize(SizeSaveParameters request);
        Task<IEnumerable<SizeDetailsResponse>> GetSizesList(SizeSearchParameters request);
        Task<SizeDetailsResponse?> GetSizeDetailsById(long id);
        Task<IEnumerable<SizeFailToImportValidationErrors>> ImportSizesDetails(List<SizeImportSaveParameters> request);
        #endregion

        #region Brand
        Task<int> SaveBrand(BrandSaveParameters request);
        Task<IEnumerable<BrandDetailsResponse>> GetBrandsList(BrandSearchParameters request);
        Task<BrandDetailsResponse?> GetBrandDetailsById(long id);
        Task<IEnumerable<BrandFailToImportValidationErrors>> ImportBrandsDetails(List<BrandImportSaveParameters> request);

        #endregion

        #region Collection Panel
        Task<int> SaveCollection_Panel(Collection_PanelSaveParameters request);
        Task<IEnumerable<CollectionDetails_PanelResponse>> GetCollectionsList_Panel(Collection_PanelSearchParameters request);
        Task<CollectionDetails_PanelResponse?> GetCollectionDetailsById_Panel(long id);
        Task<IEnumerable<Collection_PanelFailToImportValidationErrors>> ImportCollectionsDetails_Panel(List<Collection_PanelImportSaveParameters> request);

        #endregion

        #region Category
        Task<int> SaveCategory(CategorySaveParameters request);
        Task<IEnumerable<CategoryDetailsResponse>> GetCategorysList(CategorySearchParameters request);
        Task<CategoryDetailsResponse?> GetCategoryDetailsById(long id);
        Task<IEnumerable<CategoryFailToImportValidationErrors>> ImportCategorysDetails(List<CategoryImportSaveParameters> request);

        #endregion

        #region Type
        Task<int> SaveType(TypeSaveParameters request);
        Task<IEnumerable<TypeDetailsResponse>> GetTypesList(TypeSearchParameters request);
        Task<TypeDetailsResponse?> GetTypeDetailsById(long id);
        Task<IEnumerable<TypeFailToImportValidationErrors>> ImportTypesDetails(List<TypeImportSaveParameters> request);

        #endregion

        #region Panel Type
        Task<int> SavePanelType(PanelTypeSaveParameters request);
        Task<IEnumerable<PanelTypeDetailsResponse>> GetPanelTypesList(PanelTypeSearchParameters request);
        Task<PanelTypeDetailsResponse?> GetPanelTypeDetailsById(long id);
        Task<IEnumerable<PanelTypeFailToImportValidationErrors>> ImportPanelTypesDetails(List<PanelTypeImportSaveParameters> request);

        #endregion

        #region Punch
        Task<int> SavePunch(PunchSaveParameters request);
        Task<IEnumerable<PunchDetailsResponse>> GetPunchsList(PunchSearchParameters request);
        Task<PunchDetailsResponse?> GetPunchDetailsById(long id);
        Task<IEnumerable<PunchFailToImportValidationErrors>> ImportPunchsDetails(List<PunchImportSaveParameters> request);

        #endregion

        #region Surface
        Task<int> SaveSurface(SurfaceSaveParameters request);
        Task<IEnumerable<SurfaceDetailsResponse>> GetSurfacesList(SurfaceSearchParameters request);
        Task<SurfaceDetailsResponse?> GetSurfaceDetailsById(long id);
        Task<IEnumerable<SurfaceFailToImportValidationErrors>> ImportSurfacesDetails(List<SurfaceImportSaveParameters> request);

        #endregion

        #region Thickness
        Task<int> SaveThickness(ThicknessSaveParameters request);
        Task<IEnumerable<ThicknessDetailsResponse>> GetThicknessesList(ThicknessSearchParameters request);
        Task<ThicknessDetailsResponse?> GetThicknessDetailsById(long id);
        Task<IEnumerable<ThicknessFailToImportValidationErrors>> ImportThicknessesDetails(List<ThicknessImportSaveParameters> request);

        #endregion

        #region TileSize
        Task<int> SaveTileSize(TileSizeSaveParameters request);
        Task<IEnumerable<TileSizeDetailsResponse>> GetTileSizesList(TileSizeSearchParameters request);
        Task<TileSizeDetailsResponse?> GetTileSizeDetailsById(long id);
        Task<IEnumerable<TileSizeFailToImportValidationErrors>> ImportTileSizesDetails(List<TileSizeImportSaveParameters> request);

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
        Task<IEnumerable<SelectListResponse>> GetSubVendorForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetContactTypeForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetReferralForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetCuttingPlanForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetProductDesignForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetTileSizeForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetDriverNameForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetFinishNameForSelectList(CommonSelectListRequestModel parameters);
        #endregion

        #region Product Design
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

        #region TileType
        Task<int> SaveTileType(TileTypeSaveParameters request);
        Task<IEnumerable<TileTypeDetailsResponse>> GetTileTypesList(TileTypeSearchParameters request);
        Task<TileTypeDetailsResponse?> GetTileTypeDetailsById(long id);
        Task<IEnumerable<TileTypeFailToImportValidationErrors>> ImportTileTypesDetails(List<TileTypeImportSaveParameters> request);

        #endregion

        #region ContactType
        Task<int> SaveContactType(ContactTypeSaveParameters request);
        Task<IEnumerable<ContactTypeDetailsResponse>> GetContactTypesList(ContactTypeSearchParameters request);
        Task<ContactTypeDetailsResponse?> GetContactTypeDetailsById(long id);
        Task<IEnumerable<ContactTypeFailToImportValidationErrors>> ImportContactTypesDetails(List<ContactTypeImportSaveParameters> request);

        #endregion

        #region WeekClose
        Task<int> SaveWeekClose(WeekCloseSaveParameters request);
        Task<IEnumerable<WeekCloseDetailsResponse>> GetWeekClosesList(WeekCloseSearchParameters request);
        Task<WeekCloseDetailsResponse?> GetWeekCloseDetailsById(long id);
        Task<IEnumerable<WeekCloseFailToImportValidationErrors>> ImportWeekClosesDetails(List<WeekCloseImportSaveParameters> request);

        #endregion

        #region Gendor
        Task<int> SaveGendor(GendorSaveParameters request);
        Task<IEnumerable<GendorDetailsResponse>> GetGendorsList(GendorSearchParameters request);
        Task<GendorDetailsResponse?> GetGendorDetailsById(long id);
        Task<IEnumerable<GendorFailToImportValidationErrors>> ImportGendorsDetails(List<GendorImportSaveParameters> request);

        #endregion

        #region Cutting Size
        Task<int> SaveCuttingSize(CuttingSizeSaveParameters request);
        Task<IEnumerable<CuttingSizeDetailsResponse>> GetCuttingSizesList(CuttingSizeSearchParameters request);
        Task<CuttingSizeDetailsResponse?> GetCuttingSizeDetailsById(long id);
        Task<IEnumerable<CuttingSizeFailToImportValidationErrors>> ImportCuttingSizeDetails(List<CuttingSizeImportSaveParameters> request);
        Task<IEnumerable<SelectListResponse>> GetCuttingSizeForSelectList(CommonSelectListRequestModel parameters);
        #endregion

        #region Referral

        string RandomDigits(int length);
        Task<int> SaveReferral(ReferralSaveParameters request);
        Task<IEnumerable<ReferralDetailsResponse>> GetReferralList(ReferralSearchParameters request);
        Task<ReferralDetailsResponse?> GetReferralDetailsById(long id);
        Task<IEnumerable<ReferralFailToImportValidationErrors>> ImportReferralDetails(List<ReferralImportSaveParameters> parameters);
        #endregion

        #region RoleAccess
        Task<int> SavePage(PageSaveParameters request);
        Task<IEnumerable<PageDetailsResponse>> GetPageList(PageSearchParameters request);
        Task<PageDetailsResponse?> GetPageDetailsById(long id);
        Task<IEnumerable<RollPermissionDetailsResponse>> GetRollPermissionDetailsByRoleId(long roleId);
        Task<IEnumerable<RollPermissionDetailsResponse>> GetRollPermissionList(RollPermissionSearchParameters request);
        Task<int> UpdateRolePermission(List<RolePermissionUpdateParameters> rolePermission);
        Task<int> UpdateEmployeePermission(List<EmployeePermissionUpdateParameters> employeePermission);

        Task<IEnumerable<EmployeePermissionDetailsResponse>> GetEmployeePermissionDetailsByEmployeeId(long employeeId);
        Task<IEnumerable<EmployeePermissionDetailsResponse>> GetEmployeePermissionList(EmployeePermissionSearchParameters request);
        #endregion

        #region sales
        Task<IEnumerable<Users>> GetUsersList();

        #region Product API Service Interface
        Task<IEnumerable<ProductResponse>> GetProductsList(SearchProductRequest request);
        Task<int> SaveProduct(ProductRequest productRequest);
        Task<ProductResponse?> GetProductDetailsById(long id);
        Task<IEnumerable<ProductDataValidationErrors>> ImportProductsDetails(List<ImportedProductDetails> request);

        #endregion Product API Service Interface

        #region Design Type API Service Interface
        Task<IEnumerable<DesignTypeResponse>> GetDesignTypesList(SearchDesignTypeRequest request);

        Task<int> SaveDesignType(DesignTypeRequest designTypeRequest);
        Task<DesignTypeResponse?> GetDesignTypeDetailsById(long id);
        Task<IEnumerable<DesignTypeDataValidationErrors>> ImportDesignTypesDetails(List<ImportedDesignTypeDetails> request);

        #endregion Design Type API Service Interface

        #region Series API Service Interface
        Task<IEnumerable<SeriesResponse>> GetSeriesList(SearchSeriesRequest request);
        Task<int> SaveSeries(SeriesRequest seriesRequest);
        Task<SeriesResponse?> GetSeriesDetailsById(long id);
        Task<IEnumerable<SeriesDataValidationErrors>> ImportSeriesDetails(List<ImportedSeriesDetails> request);

        #endregion Design Type API Service Interface

        #region Base Design API Service Interface
        Task<IEnumerable<BaseDesignResponse>> GetBaseDesignsList(SearchBaseDesignRequest request);
        Task<int> SaveBaseDesign(BaseDesignRequest seriesRequest);
        Task<BaseDesignResponse?> GetBaseDesignDetailsById(long id);
        Task<IEnumerable<BaseDesignDataValidationErrors>> ImportBaseDesignsDetails(List<ImportedBaseDesignDetails> request);

        #endregion Base Design API Service Interface

        #region Customer Type API Service Interface
        Task<IEnumerable<CustomerTypeResponse>> GetCustomerTypesList(SearchCustomerTypeRequest request);
        Task<int> SaveCustomerType(CustomerTypeRequest customerTypeRequest);
        Task<CustomerTypeResponse?> GetCustomerTypeDetailsById(long id);
        Task<IEnumerable<CustomerTypeDataValidationErrors>> ImportCustomerTypesDetails(List<ImportedCustomerTypeDetails> request);

        #endregion Customer Type API Service Interface

        #region Leave Type API Service Interface
        Task<IEnumerable<LeaveTypeResponse>> GetLeaveTypesList(SearchLeaveTypeRequest request);
        Task<int> SaveLeaveType(LeaveTypeRequest leaveTypeRequest);
        Task<LeaveTypeResponse?> GetLeaveTypeDetailsById(long id);
        Task<IEnumerable<LeaveTypeDataValidationErrors>> ImportLeaveTypesDetails(List<ImportedLeaveTypeDetails> request);

        #endregion Leave Type API Service Interface

        #region Master Data
        Task<IEnumerable<SelectListResponse>> GetCustomerTypesForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetCustomersForSelectList(CustomerSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetStatusMasterForSelectList(string statusCode);
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
        Task<int> SaveFold(FoldSaveParameters request);
        Task<IEnumerable<FoldDetailsResponse>> GetFoldList(FoldSearchParameters request);
        Task<FoldDetailsResponse?> GetFoldDetailsById(long id);
        Task<IEnumerable<FoldFailToImportValidationErrors>> ImportFoldsDetails(List<FoldImportSaveParameters> request);

        #endregion

        #region Flap
        Task<int> SaveFlap(FlapSaveParameters request);
        Task<IEnumerable<FlapDetailsResponse>> GetFlapList(FlapSearchParameters request);
        Task<FlapDetailsResponse?> GetFlapDetailsById(long id);
        Task<IEnumerable<FlapFailToImportValidationErrors>> ImportFlapsDetails(List<FlapImportSaveParameters> request);

        #endregion

        #region Title GSM
        Task<int> SaveTitleGSM(TitleGSMSaveParameters request);
        Task<IEnumerable<TitleGSMDetailsResponse>> GetTitleGSMList(TitleGSMSearchParameters request);
        Task<TitleGSMDetailsResponse?> GetTitleGSMDetailsById(long id);
        Task<IEnumerable<TitleGSMFailToImportValidationErrors>> ImportTitleGSMsDetails(List<TitleGSMImportSaveParameters> request);

        #endregion

        #region Flap GSM
        Task<int> SaveFlapGSM(FlapGSMSaveParameters request);
        Task<IEnumerable<FlapGSMDetailsResponse>> GetFlapGSMList(FlapGSMSearchParameters request);
        Task<FlapGSMDetailsResponse?> GetFlapGSMDetailsById(long id);
        Task<IEnumerable<FlapGSMFailToImportValidationErrors>> ImportFlapGSMsDetails(List<FlapGSMImportSaveParameters> request);

        #endregion

        #region Inner GSM
        Task<int> SaveInnerGSM(InnerGSMSaveParameters request);
        Task<IEnumerable<InnerGSMDetailsResponse>> GetInnerGSMList(InnerGSMSearchParameters request);
        Task<InnerGSMDetailsResponse?> GetInnerGSMDetailsById(long id);
        Task<IEnumerable<InnerGSMFailToImportValidationErrors>> ImportInnerGSMsDetails(List<InnerGSMImportSaveParameters> request);

        #endregion

        #region Title Process
        Task<int> SaveTitleProcess(TitleProcessSaveParameters request);
        Task<IEnumerable<TitleProcessDetailsResponse>> GetTitleProcessList(TitleProcessSearchParameters request);
        Task<TitleProcessDetailsResponse?> GetTitleProcessDetailsById(long id);
        Task<IEnumerable<TitleProcessFailToImportValidationErrors>> ImportTitleProcesssDetails(List<TitleProcessImportSaveParameters> request);

        #endregion

        #region Expense Type
        Task<int> SaveExpenseType(ExpenseTypeRequest request);
        Task<IEnumerable<ExpenseTypeResponse>> GetExpenseTypeList(SearchExpenseTypeRequest request);
        Task<ExpenseTypeResponse?> GetExpenseTypeDetailsById(long id);

        #endregion
    }
}
