using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Repositories;
using static CasaAPI.Models.BloodModel;
using static CasaAPI.Models.BrandModel;
using static CasaAPI.Models.CategoryModel;
using static CasaAPI.Models.Collection_PanelModel;
using static CasaAPI.Models.ContactTypeModel;
using static CasaAPI.Models.CuttingSizeModel;
using static CasaAPI.Models.GendorModel;
using static CasaAPI.Models.PanelTypeModel;
using static CasaAPI.Models.PunchModel;
using static CasaAPI.Models.SurfaceModel;
using static CasaAPI.Models.ThicknessModel;
using static CasaAPI.Models.TileSizeModel;
using static CasaAPI.Models.TileTypeModel;
using static CasaAPI.Models.TypeModel;
using static CasaAPI.Models.WeekCloseModel;

namespace CasaAPI.Services
{
    public class AdminService : IAdminService
    {
        private IAdminRepository _adminRepository;
        private IFileManager _fileManager;
        public AdminService(IAdminRepository adminRepository, IFileManager fileManager)
        {
            _adminRepository = adminRepository;
            _fileManager = fileManager;
        }
        #region Size
        public async Task<int> SaveSize(SizeSaveParameters request)
        {
            return await _adminRepository.SaveSize(request);
        }
        public async Task<IEnumerable<SizeDetailsResponse>> GetSizesList(SizeSearchParameters request)
        {
            return await _adminRepository.GetSizesList(request);
        }
        public async Task<SizeDetailsResponse?> GetSizeDetailsById(long id)
        {
            return await _adminRepository.GetSizeDetailsById(id);
        }
        public async Task<IEnumerable<SizeFailToImportValidationErrors>> ImportSizesDetails(List<SizeImportSaveParameters> request)
        {
            return await _adminRepository.ImportSizesDetails(request);
        }
        
        #endregion

        #region Brand
        public async Task<int> SaveBrand(BrandSaveParameters request)
        {
            return await _adminRepository.SaveBrand(request);
        }
        public async Task<IEnumerable<BrandDetailsResponse>> GetBrandsList(BrandSearchParameters request)
        {
            return await _adminRepository.GetBrandsList(request);
        }
        public async Task<BrandDetailsResponse?> GetBrandDetailsById(long id)
        {
            return await _adminRepository.GetBrandDetailsById(id);
        }
        public async Task<IEnumerable<BrandFailToImportValidationErrors>> ImportBrandsDetails(List<BrandImportSaveParameters> request)
        {
            return await _adminRepository.ImportBrandsDetails(request);
        }
        #endregion

        #region Collection
        public async Task<int> SaveCollection_Panel(Collection_PanelSaveParameters request)
        {
            return await _adminRepository.SaveCollection_Panel(request);
        }
        public async Task<IEnumerable<CollectionDetails_PanelResponse>> GetCollectionsList_Panel(Collection_PanelSearchParameters request)
        {
            return await _adminRepository.GetCollectionsList_Panel(request);
        }
        public async Task<CollectionDetails_PanelResponse?> GetCollectionDetailsById_Panel(long id)
        {
            return await _adminRepository.GetCollectionDetailsById_Panel(id);
        }
        public async Task<IEnumerable<Collection_PanelFailToImportValidationErrors>> ImportCollectionsDetails_Panel(List<Collection_PanelImportSaveParameters> request)
        {
            return await _adminRepository.ImportCollectionsDetails_Panel(request);
        }
        #endregion

        #region Category
        public async Task<int> SaveCategory(CategorySaveParameters request)
        {
            return await _adminRepository.SaveCategory(request);
        }
        public async Task<IEnumerable<CategoryDetailsResponse>> GetCategorysList(CategorySearchParameters request)
        {
            return await _adminRepository.GetCategorysList(request);
        }
        public async Task<CategoryDetailsResponse?> GetCategoryDetailsById(long id)
        {
            return await _adminRepository.GetCategoryDetailsById(id);
        }
        public async Task<IEnumerable<CategoryFailToImportValidationErrors>> ImportCategorysDetails(List<CategoryImportSaveParameters> request)
        {
            return await _adminRepository.ImportCategorysDetails(request);
        }
        #endregion

        #region Type
        public async Task<int> SaveType(TypeSaveParameters request)
        {
            return await _adminRepository.SaveType(request);
        }
        public async Task<IEnumerable<TypeDetailsResponse>> GetTypesList(TypeSearchParameters request)
        {
            return await _adminRepository.GetTypesList(request);
        }
        public async Task<TypeDetailsResponse?> GetTypeDetailsById(long id)
        {
            return await _adminRepository.GetTypeDetailsById(id);
        }
        public async Task<IEnumerable<TypeFailToImportValidationErrors>> ImportTypesDetails(List<TypeImportSaveParameters> request)
        {
            return await _adminRepository.ImportTypesDetails(request);
        }
        #endregion

        #region Panel Type
        public async Task<int> SavePanelType(PanelTypeSaveParameters request)
        {
            return await _adminRepository.SavePanelType(request);
        }
        public async Task<IEnumerable<PanelTypeDetailsResponse>> GetPanelTypesList(PanelTypeSearchParameters request)
        {
            return await _adminRepository.GetPanelTypesList(request);
        }
        public async Task<PanelTypeDetailsResponse?> GetPanelTypeDetailsById(long id)
        {
            return await _adminRepository.GetPanelTypeDetailsById(id);
        }
        public async Task<IEnumerable<PanelTypeFailToImportValidationErrors>> ImportPanelTypesDetails(List<PanelTypeImportSaveParameters> request)
        {
            return await _adminRepository.ImportPanelTypesDetails(request);
        }
        #endregion

        #region Punch
        public async Task<int> SavePunch(PunchSaveParameters request)
        {
            return await _adminRepository.SavePunch(request);
        }
        public async Task<IEnumerable<PunchDetailsResponse>> GetPunchsList(PunchSearchParameters request)
        {
            return await _adminRepository.GetPunchsList(request);
        }
        public async Task<PunchDetailsResponse?> GetPunchDetailsById(long id)
        {
            return await _adminRepository.GetPunchDetailsById(id);
        }
        public async Task<IEnumerable<PunchFailToImportValidationErrors>> ImportPunchsDetails(List<PunchImportSaveParameters> request)
        {
            return await _adminRepository.ImportPunchsDetails(request);
        }
        #endregion

        #region Surface
        public async Task<int> SaveSurface(SurfaceSaveParameters request)
        {
            return await _adminRepository.SaveSurface(request);
        }
        public async Task<IEnumerable<SurfaceDetailsResponse>> GetSurfacesList(SurfaceSearchParameters request)
        {
            return await _adminRepository.GetSurfacesList(request);
        }
        public async Task<SurfaceDetailsResponse?> GetSurfaceDetailsById(long id)
        {
            return await _adminRepository.GetSurfaceDetailsById(id);
        }
        public async Task<IEnumerable<SurfaceFailToImportValidationErrors>> ImportSurfacesDetails(List<SurfaceImportSaveParameters> request)
        {
            return await _adminRepository.ImportSurfacesDetails(request);
        }
        #endregion

        #region Thickness
        public async Task<int> SaveThickness(ThicknessSaveParameters request)
        {
            return await _adminRepository.SaveThickness(request);
        }
        public async Task<IEnumerable<ThicknessDetailsResponse>> GetThicknessesList(ThicknessSearchParameters request)
        {
            return await _adminRepository.GetThicknessesList(request);
        }
        public async Task<ThicknessDetailsResponse?> GetThicknessDetailsById(long id)
        {
            return await _adminRepository.GetThicknessDetailsById(id);
        }
        public async Task<IEnumerable<ThicknessFailToImportValidationErrors>> ImportThicknessesDetails(List<ThicknessImportSaveParameters> request)
        {
            return await _adminRepository.ImportThicknessesDetails(request);
        }
        #endregion

        #region TileSize
        public async Task<int> SaveTileSize(TileSizeSaveParameters request)
        {
            return await _adminRepository.SaveTileSize(request);
        }
        public async Task<IEnumerable<TileSizeDetailsResponse>> GetTileSizesList(TileSizeSearchParameters request)
        {
            return await _adminRepository.GetTileSizesList(request);
        }
        public async Task<TileSizeDetailsResponse?> GetTileSizeDetailsById(long id)
        {
            return await _adminRepository.GetTileSizeDetailsById(id);
        }
        public async Task<IEnumerable<TileSizeFailToImportValidationErrors>> ImportTileSizesDetails(List<TileSizeImportSaveParameters> request)
        {
            return await _adminRepository.ImportTileSizesDetails(request);
        }
        #endregion

        #region Master Data
        public async Task<IEnumerable<SelectListResponse>> GetSizeForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetSizeForSelectList(parameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetBrandForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetBrandForSelectList(parameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetCollectionForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetCollectionForSelectList(parameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetCategoryForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetCategoryForSelectList(parameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetTypeForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetTypeForSelectList(parameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetPunchForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetPunchForSelectList(parameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetSurfaceForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetSurfaceForSelectList(parameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetThicknessForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetThicknessForSelectList(parameters);
        }
        public async Task<IEnumerable<SelectListResponse>> GetTileForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetTileForSelectList(parameters);
        }
        #endregion

        #region Manage Product Design
        public async Task<IEnumerable<ProductDesignResponse>> GetProductDesignList(ProductDesignSearchParameters request)
        {
            return await _adminRepository.GetProductDesignList(request);
        }

        public async Task<ProductDesignDetailsResponse?> GetProductDesignDetailsById(long id)
        {
            return await _adminRepository.GetProductDesignDetailsById(id);
        }

        public async Task<IEnumerable<ProductDesignFilesResponse>> GetProductDesignFiles(long productDesignId)
        {
            IEnumerable<ProductDesignFilesResponse> lstProductDesignFiles = await _adminRepository.GetProductDesignFiles(productDesignId);

            foreach (ProductDesignFilesResponse item in lstProductDesignFiles)
            {
                item.FileContent = _fileManager.GetProductDesignFiles(item.StoredFilesName);
            }

            return lstProductDesignFiles;
        }

        public async Task<int> SaveProductDesign(ProductDesignsSaveModel productDesignsSaveModel)
        {
            if (productDesignsSaveModel?.ProductDesignFiles != null)
            {
                foreach (ProductDesignFiles prodf in productDesignsSaveModel?.ProductDesignFiles)
                {
                    if (prodf?.DesignFile != null)
                    {
                        prodf.UploadedFilesName = prodf.UploadedFilesName;
                        prodf.StoredFilesName = prodf.DesignFile;
                    }
                }
            }
            return await _adminRepository.SaveProductDesign(productDesignsSaveModel);
        }

        public async Task<IEnumerable<ProductDesignValidationErrors>> ImportProductDesignData(List<ImportProductDesign> parameters)
        {
            return await _adminRepository.ImportProductDesignData(parameters);
        }

        #endregion

        #region Manage Box Size
        public async Task<IEnumerable<ManageBoxSizeResponse>> GetManageBoxSizeList(ManageBoxSizeSearchParameters parameters)
        {
            return await _adminRepository.GetManageBoxSizeList(parameters);
        }
        public async Task<ManageBoxSizeResponse?> GetManageBoxSizeById(long id)
        {
            return await _adminRepository.GetManageBoxSizeById(id);
        }

        public async Task<int> SaveManageBoxSize(ManageBoxSizeModel parameters)
        {
            return await _adminRepository.SaveManageBoxSize(parameters);
        }

        public async Task<IEnumerable<ManageBoxSizeValidationErrors>> ImportManageBoxSize(List<ImportManageBoxSize> parameters)
        {
            return await _adminRepository.ImportManageBoxSize(parameters);
        }
        #endregion

        public async Task<IEnumerable<SelectListResponse>> GetSubVendorForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetSubVendorForSelectList(parameters);
        }
        public async Task<IEnumerable<SelectListResponse>> GetContactTypeForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetContactTypeForSelectList(parameters);
        }
        public async Task<IEnumerable<SelectListResponse>> GetReferralForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetReferralForSelectList(parameters);
        }

        #region TileType
        public async Task<int> SaveTileType(TileTypeSaveParameters request)
        {
            return await _adminRepository.SaveTileType(request);
        }
        public async Task<IEnumerable<TileTypeDetailsResponse>> GetTileTypesList(TileTypeSearchParameters request)
        {
            return await _adminRepository.GetTileTypesList(request);
        }
        public async Task<TileTypeDetailsResponse?> GetTileTypeDetailsById(long id)
        {
            return await _adminRepository.GetTileTypeDetailsById(id);
        }
        public async Task<IEnumerable<TileTypeFailToImportValidationErrors>> ImportTileTypesDetails(List<TileTypeImportSaveParameters> request)
        {
            return await _adminRepository.ImportTileTypesDetails(request);
        }
        #endregion

        #region ContactType
        public async Task<int> SaveContactType(ContactTypeSaveParameters request)
        {
            return await _adminRepository.SaveContactType(request);
        }
        public async Task<IEnumerable<ContactTypeDetailsResponse>> GetContactTypesList(ContactTypeSearchParameters request)
        {
            return await _adminRepository.GetContactTypesList(request);
        }
        public async Task<ContactTypeDetailsResponse?> GetContactTypeDetailsById(long id)
        {
            return await _adminRepository.GetContactTypeDetailsById(id);
        }

        public async Task<IEnumerable<ContactTypeFailToImportValidationErrors>> ImportContactTypesDetails(List<ContactTypeImportSaveParameters> request)
        {
            return await _adminRepository.ImportContactTypesDetails(request);
        }
        #endregion

        #region WeekClose
        public async Task<int> SaveWeekClose(WeekCloseSaveParameters request)
        {
            return await _adminRepository.SaveWeekClose(request);
        }
        public async Task<IEnumerable<WeekCloseDetailsResponse>> GetWeekClosesList(WeekCloseSearchParameters request)
        {
            return await _adminRepository.GetWeekClosesList(request);
        }
        public async Task<WeekCloseDetailsResponse?> GetWeekCloseDetailsById(long id)
        {
            return await _adminRepository.GetWeekCloseDetailsById(id);
        }

        public async Task<IEnumerable<WeekCloseFailToImportValidationErrors>> ImportWeekClosesDetails(List<WeekCloseImportSaveParameters> request)
        {
            return await _adminRepository.ImportWeekClosesDetails(request);
        }
        #endregion

        #region Gendor
        public async Task<int> SaveGendor(GendorSaveParameters request)
        {
            return await _adminRepository.SaveGendor(request);
        }
        public async Task<IEnumerable<GendorDetailsResponse>> GetGendorsList(GendorSearchParameters request)
        {
            return await _adminRepository.GetGendorsList(request);
        }
        public async Task<GendorDetailsResponse?> GetGendorDetailsById(long id)
        {
            return await _adminRepository.GetGendorDetailsById(id);
        }

        public async Task<IEnumerable<GendorFailToImportValidationErrors>> ImportGendorsDetails(List<GendorImportSaveParameters> request)
        {
            return await _adminRepository.ImportGendorsDetails(request);
        }
        #endregion

        #region CuttingSize
        public async Task<int> SaveCuttingSize(CuttingSizeSaveParameters request)
        {
            return await _adminRepository.SaveCuttingSize(request);
        }
        public async Task<IEnumerable<CuttingSizeDetailsResponse>> GetCuttingSizesList(CuttingSizeSearchParameters request)
        {
            return await _adminRepository.GetCuttingSizesList(request);
        }

        public async Task<CuttingSizeDetailsResponse?> GetCuttingSizeDetailsById(long id)
        {
            return await _adminRepository.GetCuttingSizeDetailsById(id);
        }
        public async Task<IEnumerable<CuttingSizeFailToImportValidationErrors>> ImportCuttingSizeDetails(List<CuttingSizeImportSaveParameters> request)
        {
            return await _adminRepository.ImportCuttingSizeDetails(request);
        }
        public async Task<IEnumerable<SelectListResponse>> GetCuttingSizeForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetCuttingSizeForSelectList(parameters);
        }
        #endregion

        #region Referral
        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
        public async Task<int> SaveReferral(ReferralSaveParameters request)
        {
            return await _adminRepository.SaveReferral(request);
        }
        public async Task<IEnumerable<ReferralDetailsResponse>> GetReferralList(ReferralSearchParameters request)
        {
            return await _adminRepository.GetReferralList(request);
        }
        public async Task<IEnumerable<ReferralFailToImportValidationErrors>> ImportReferralDetails(List<ReferralImportSaveParameters> request)
        {
            return await _adminRepository.ImportReferralDetails(request);
        }
        public async Task<ReferralDetailsResponse?> GetReferralDetailsById(long id)
        {
            return await _adminRepository.GetReferralDetailsById(id);
        }
        #endregion

        #region Page
        public async Task<int> SavePage(PageSaveParameters request)
        {
            return await _adminRepository.SavePage(request);
        }
        public async Task<IEnumerable<PageDetailsResponse>> GetPageList(PageSearchParameters request)
        {
            return await _adminRepository.GetPageList(request);
        }

        public async Task<PageDetailsResponse?> GetPageDetailsById(long id)
        {
            return await _adminRepository.GetPageDetailsById(id);
        }

        public async Task<IEnumerable<RollPermissionDetailsResponse>> GetRollPermissionDetailsByRoleId(long roleId)
        {
            return await _adminRepository.GetRollPermissionDetailsByRoleId(roleId);
        }

        public async Task<IEnumerable<RollPermissionDetailsResponse>> GetRollPermissionList(RollPermissionSearchParameters request)
        {
            return await _adminRepository.GetRollPermissionList(request);
        }
        public async Task<int> UpdateRolePermission(List<RolePermissionUpdateParameters> rolePermission)
        {
            return await _adminRepository.UpdateRolePermission(rolePermission);
        }
        public async Task<int> UpdateEmployeePermission(List<EmployeePermissionUpdateParameters> employeePermission)
        {
            return await _adminRepository.UpdateEmployeePermission(employeePermission);
        }

        public async Task<IEnumerable<EmployeePermissionDetailsResponse>> GetEmployeePermissionDetailsByEmployeeId(long employeeId)
        {
            return await _adminRepository.GetEmployeePermissionDetailsByEmployeeId(employeeId);
        }
        public async Task<IEnumerable<EmployeePermissionDetailsResponse>> GetEmployeePermissionList(EmployeePermissionSearchParameters request)
        {
            return await _adminRepository.GetEmployeePermissionList(request);
        }

        #endregion


        #region sales
        public async Task<IEnumerable<Users>> GetUsersList()
        {
            return await _adminRepository.GetUsersList();
        }

        #region Product API Service
        public async Task<IEnumerable<ProductResponse>> GetProductsList(SearchProductRequest request)
        {
            return await _adminRepository.GetProductsList(request);
        }

        public async Task<int> SaveProduct(ProductRequest productRequest)
        {
            return await _adminRepository.SaveProduct(productRequest);
        }

        public async Task<IEnumerable<ProductDataValidationErrors>> ImportProductsDetails(List<ImportedProductDetails> request)
        {
            return await _adminRepository.ImportProductsDetails(request);
        }

        public async Task<ProductResponse?> GetProductDetailsById(long id)
        {
            return await _adminRepository.GetProductDetailsById(id);
        }
        #endregion Product API Service

        #region Design Type API Service
        public async Task<IEnumerable<DesignTypeResponse>> GetDesignTypesList(SearchDesignTypeRequest request)
        {
            return await _adminRepository.GetDesignTypesList(request);
        }

        public async Task<int> SaveDesignType(DesignTypeRequest designTypeRequest)
        {
            return await _adminRepository.SaveDesignType(designTypeRequest);

        }
        public async Task<DesignTypeResponse?> GetDesignTypeDetailsById(long id)
        {
            return await _adminRepository.GetDesignTypeDetailsById(id);
        }
        public async Task<IEnumerable<DesignTypeDataValidationErrors>> ImportDesignTypesDetails(List<ImportedDesignTypeDetails> request)
        {
            return await _adminRepository.ImportDesignTypesDetails(request);
        }
        #endregion Design Type API Service

        #region Series API Service
        public async Task<IEnumerable<SeriesResponse>> GetSeriesList(SearchSeriesRequest request)
        {
            return await _adminRepository.GetSeriesList(request);
        }

        public async Task<int> SaveSeries(SeriesRequest seriesRequest)
        {
            return await _adminRepository.SaveSeries(seriesRequest);

        }
        public async Task<SeriesResponse?> GetSeriesDetailsById(long id)
        {
            return await _adminRepository.GetSeriesDetailsById(id);
        }
        public async Task<IEnumerable<SeriesDataValidationErrors>> ImportSeriesDetails(List<ImportedSeriesDetails> request)
        {
            return await _adminRepository.ImportSeriesDetails(request);
        }
        #endregion Series API Service


        #region Base Design API Service
        public async Task<IEnumerable<BaseDesignResponse>> GetBaseDesignsList(SearchBaseDesignRequest request)
        {
            return await _adminRepository.GetBaseDesignsList(request);
        }

        public async Task<int> SaveBaseDesign(BaseDesignRequest baseDesignRequest)
        {
            return await _adminRepository.SaveBaseDesign(baseDesignRequest);

        }
        public async Task<BaseDesignResponse?> GetBaseDesignDetailsById(long id)
        {
            return await _adminRepository.GetBaseDesignDetailsById(id);
        }
        public async Task<IEnumerable<BaseDesignDataValidationErrors>> ImportBaseDesignsDetails(List<ImportedBaseDesignDetails> request)
        {
            return await _adminRepository.ImportBaseDesignsDetails(request);
        }
        #endregion Base Design API Service


        #region Customer Type API Service
        public async Task<IEnumerable<CustomerTypeResponse>> GetCustomerTypesList(SearchCustomerTypeRequest request)
        {
            return await _adminRepository.GetCustomerTypesList(request);
        }

        public async Task<int> SaveCustomerType(CustomerTypeRequest customerTypeRequest)
        {
            return await _adminRepository.SaveCustomerType(customerTypeRequest);
        }
        public async Task<CustomerTypeResponse?> GetCustomerTypeDetailsById(long id)
        {
            return await _adminRepository.GetCustomerTypeDetailsById(id);
        }
        public async Task<IEnumerable<CustomerTypeDataValidationErrors>> ImportCustomerTypesDetails(List<ImportedCustomerTypeDetails> request)
        {
            return await _adminRepository.ImportCustomerTypesDetails(request);
        }
        #endregion Customer Type API Service


        #region Leave Type API Service
        public async Task<IEnumerable<LeaveTypeResponse>> GetLeaveTypesList(SearchLeaveTypeRequest request)
        {
            return await _adminRepository.GetLeaveTypesList(request);
        }

        public async Task<int> SaveLeaveType(LeaveTypeRequest leaveTypeRequest)
        {
            return await _adminRepository.SaveLeaveType(leaveTypeRequest);

        }
        public async Task<LeaveTypeResponse?> GetLeaveTypeDetailsById(long id)
        {
            return await _adminRepository.GetLeaveTypeDetailsById(id);
        }
        public async Task<IEnumerable<LeaveTypeDataValidationErrors>> ImportLeaveTypesDetails(List<ImportedLeaveTypeDetails> request)
        {
            return await _adminRepository.ImportLeaveTypesDetails(request);
        }
        #endregion Leave Type API Service

        #region Master Data
       
        public async Task<IEnumerable<SelectListResponse>> GetCustomerTypesForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetCustomerTypesForSelectList(parameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetCustomersForSelectList(CustomerSelectListRequestModel parameters)
        {
            return await _adminRepository.GetCustomersForSelectList(parameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetStatusMasterForSelectList(string statusCode)
        {
            return await _adminRepository.GetStatusMasterForSelectList(statusCode);
        }

        public async Task<IEnumerable<SelectListResponse>> GetReportingToEmployeeForSelectList(ReportingToEmpListParameters parameters)
        {
            return await _adminRepository.GetReportingToEmployeeForSelectList(parameters);
        }

        public async Task<IEnumerable<CustomerContactsListForFields>> GetCustomerContactsListForFields(CustomerContactsListRequest parameters)
        {
            return await _adminRepository.GetCustomerContactsListForFields(parameters);
        }
        #endregion

        #region Blood Group Master
        public async Task<int> SaveBloodGroupDetails(BloodGroupRequestModel parameters)
        {
            return await _adminRepository.SaveBloodGroupDetails(parameters);
        }

        public async Task<IEnumerable<BloodGroupResponseModel>> GetBloodGroupList(SearchBloodGroupRequestModel parameters)
        {
            return await _adminRepository.GetBloodGroupList(parameters);
        }

        public async Task<BloodGroupResponseModel?> GetBloodGroupDetails(long id)
        {
            return await _adminRepository.GetBloodGroupDetails(id);
        }
        #endregion

        #region Collection Master
        public async Task<int> SaveCollectionMasterDetails(SaveCollectionRequestModel parameters)
        {
            return await _adminRepository.SaveCollectionMasterDetails(parameters);
        }

        public async Task<IEnumerable<CollectionResponseModel>> GetCollectionMasterList(SearchCollectionRequestModel parameters)
        {
            return await _adminRepository.GetCollectionMasterList(parameters);
        }

        public async Task<CollectionResponseModel?> GetCollectionMasterDetails(int id)
        {
            return await _adminRepository.GetCollectionMasterDetails(id);
        }
        public async Task<IEnumerable<CollectionDataValidationErrors>> ImportCollection(List<ImportedCollection> request)
        {
            return await _adminRepository.ImportCollection(request);
        }
        #endregion
        #endregion
    }

}
