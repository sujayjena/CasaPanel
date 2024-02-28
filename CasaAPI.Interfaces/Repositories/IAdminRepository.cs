using CasaAPI.Models;
using static CasaAPI.Models.BloodModel;
using static CasaAPI.Models.BrandModel;
using static CasaAPI.Models.CategoryModel;
using static CasaAPI.Models.CollectionModel;
using static CasaAPI.Models.ContactTypeModel;
using static CasaAPI.Models.CuttingSizeModel;
using static CasaAPI.Models.GendorModel;
using static CasaAPI.Models.PunchModel;
using static CasaAPI.Models.SurfaceModel;
using static CasaAPI.Models.ThicknessModel;
using static CasaAPI.Models.TileSizeModel;
using static CasaAPI.Models.TileTypeModel;
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

        #region Collection
        Task<int> SaveCollection(CollectionSaveParameters parameters);
        Task<IEnumerable<CollectionDetailsResponse>> GetCollectionsList(CollectionSearchParameters parameters);
        Task<CollectionDetailsResponse?> GetCollectionDetailsById(long id);
        Task<IEnumerable<CollectionFailToImportValidationErrors>> ImportCollectionsDetails(List<CollectionImportSaveParameters> parameters);

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
        Task<IEnumerable<SelectListResponse>> GetCollectionForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetCategoryForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetTypeForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetPunchForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetSurfaceForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetThicknessForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetTileForSelectList(CommonSelectListRequestModel parameters);
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

        #region Blood
        Task<int> SaveBlood(BloodSaveParameters parameters);
        Task<IEnumerable<BloodDetailsResponse>> GetBloodsList(BloodSearchParameters parameters);
        Task<BloodDetailsResponse?> GetBloodDetailsById(long id);
        Task<IEnumerable<BloodFailToImportValidationErrors>> ImportBloodsDetails(List<BloodImportSaveParameters> parameters);

        Task<IEnumerable<SelectListResponse>> GetBloodForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetSubVendorForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetContactTypeForSelectList(CommonSelectListRequestModel parameters);
        Task<IEnumerable<SelectListResponse>> GetReferralForSelectList(CommonSelectListRequestModel parameters);
        #endregion

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

        #region Role
        Task<int> SaveRole(RoleSaveParameters parameters);
        Task<IEnumerable<RoleDetailsResponse>> GetRoleList(RoleSearchParameters parameters);
        Task<RoleDetailsResponse?> GetRoleDetailsById(long id);
        Task<IEnumerable<RoleFailToImportValidationErrors>> ImportRoleDetails(List<RoleImportSaveParameters> parameters);
        #endregion

        #region ReportingHierarchy
        Task<IEnumerable<SelectListResponse>> GetRoleHierarchyDetailsByRoleId(long roleId);
        Task<int> SaveReportingHierarchy(ReportingHierarchySaveParameters parameters);
        Task<IEnumerable<ReportingHierarchyDetailsResponse>> GetReportingHierarchyList(ReportingHierarchySearchParameters parameters);
        Task<ReportingHierarchyDetailsResponse?> GetReportingHierarchyDetailsById(long id);
        Task<IEnumerable<ReportingHierarchyFailToImportValidationErrors>> ImportReportingHierarchyDetails(List<ReportingHierarchyImportSaveParameters> parameters);
        #endregion

        #region Employee
        Task<int> SaveEmployee(EmployeeSaveParameters parameters);
        Task<IEnumerable<EmployeeDetailsResponse>> GetEmployeeList(EmployeeSearchParameters parameters);
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
    }
}
