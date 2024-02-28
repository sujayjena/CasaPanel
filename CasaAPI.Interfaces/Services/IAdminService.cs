using CasaAPI.Models;
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

        #region Collection
        Task<int> SaveCollection(CollectionSaveParameters request);
        Task<IEnumerable<CollectionDetailsResponse>> GetCollectionsList(CollectionSearchParameters request);
        Task<CollectionDetailsResponse?> GetCollectionDetailsById(long id);
        Task<IEnumerable<CollectionFailToImportValidationErrors>> ImportCollectionsDetails(List<CollectionImportSaveParameters> request);

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
        Task<IEnumerable<SelectListResponse>> GetCollectionForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetCategoryForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetTypeForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetPunchForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetSurfaceForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetThicknessForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetTileForSelectList(CommonSelectListRequestModel parameters);
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

        #region Blood
        Task<int> SaveBlood(BloodSaveParameters request);
        Task<IEnumerable<BloodDetailsResponse>> GetBloodsList(BloodSearchParameters request);
        Task<BloodDetailsResponse?> GetBloodDetailsById(long id);
        Task<IEnumerable<BloodFailToImportValidationErrors>> ImportBloodsDetails(List<BloodImportSaveParameters> request);

        Task<IEnumerable<SelectListResponse>> GetBloodForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetSubVendorForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetContactTypeForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetReferralForSelectList(CommonSelectListRequestModel parameters);
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

        #region Role
        Task<int> SaveRole(RoleSaveParameters request);
        Task<IEnumerable<RoleDetailsResponse>> GetRoleList(RoleSearchParameters request);
        Task<RoleDetailsResponse?> GetRoleDetailsById(long id);
        Task<IEnumerable<RoleFailToImportValidationErrors>> ImportRoleDetails(List<RoleImportSaveParameters> request);
        #endregion

        #region ReportingHierarchy
        Task<IEnumerable<SelectListResponse>> GetRoleHierarchyDetailsByRoleId(long roleId);
        Task<int> SaveReportingHierarchy(ReportingHierarchySaveParameters request);
        Task<IEnumerable<ReportingHierarchyDetailsResponse>> GetReportingHierarchyList(ReportingHierarchySearchParameters request);
        Task<ReportingHierarchyDetailsResponse?> GetReportingHierarchyDetailsById(long id);
        Task<IEnumerable<ReportingHierarchyFailToImportValidationErrors>> ImportReportingHierarchyDetails(List<ReportingHierarchyImportSaveParameters> request);
        #endregion

        #region Employee
        Task<int> SaveEmployee(EmployeeSaveParameters request);
        Task<IEnumerable<EmployeeDetailsResponse>> GetEmployeeList(EmployeeSearchParameters request);
        Task<EmployeeDetailsResponse?> GetEmployeeDetailsById(long id);
        Task<IEnumerable<EmployeeDetailsResponse>> GetEmployeeListByRoleId(long roleId);
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
    }
}
