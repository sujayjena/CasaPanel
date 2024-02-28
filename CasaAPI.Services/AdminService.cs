using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Repositories;
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
        public async Task<int> SaveCollection(CollectionSaveParameters request)
        {
            return await _adminRepository.SaveCollection(request);
        }
        public async Task<IEnumerable<CollectionDetailsResponse>> GetCollectionsList(CollectionSearchParameters request)
        {
            return await _adminRepository.GetCollectionsList(request);
        }
        public async Task<CollectionDetailsResponse?> GetCollectionDetailsById(long id)
        {
            return await _adminRepository.GetCollectionDetailsById(id);
        }
        public async Task<IEnumerable<CollectionFailToImportValidationErrors>> ImportCollectionsDetails(List<CollectionImportSaveParameters> request)
        {
            return await _adminRepository.ImportCollectionsDetails(request);
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


        #region Blood
        public async Task<int> SaveBlood(BloodSaveParameters request)
        {
            return await _adminRepository.SaveBlood(request);
        }
        public async Task<IEnumerable<BloodDetailsResponse>> GetBloodsList(BloodSearchParameters request)
        {
            return await _adminRepository.GetBloodsList(request);
        }
        public async Task<BloodDetailsResponse?> GetBloodDetailsById(long id)
        {
            return await _adminRepository.GetBloodDetailsById(id);
        }
        public async Task<IEnumerable<BloodFailToImportValidationErrors>> ImportBloodsDetails(List<BloodImportSaveParameters> request)
        {
            return await _adminRepository.ImportBloodsDetails(request);
        }
        public async Task<IEnumerable<SelectListResponse>> GetBloodForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _adminRepository.GetBloodForSelectList(parameters);
        }
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
        #endregion
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

        #region Role
        public async Task<int> SaveRole(RoleSaveParameters request)
        {
            return await _adminRepository.SaveRole(request);
        }
        public async Task<IEnumerable<RoleDetailsResponse>> GetRoleList(RoleSearchParameters request)
        {
            return await _adminRepository.GetRoleList(request);
        }

        public async Task<RoleDetailsResponse?> GetRoleDetailsById(long id)
        {
            return await _adminRepository.GetRoleDetailsById(id);
        }
        public async Task<IEnumerable<RoleFailToImportValidationErrors>> ImportRoleDetails(List<RoleImportSaveParameters> request)
        {
            return await _adminRepository.ImportRoleDetails(request);
        }

        #endregion

        #region ReportingHierarchy

        public async Task<IEnumerable<SelectListResponse>> GetRoleHierarchyDetailsByRoleId(long roleId)
        {
            return await _adminRepository.GetRoleHierarchyDetailsByRoleId(roleId);
        }
        public async Task<int> SaveReportingHierarchy(ReportingHierarchySaveParameters request)
        {
            return await _adminRepository.SaveReportingHierarchy(request);
        }
        public async Task<IEnumerable<ReportingHierarchyDetailsResponse>> GetReportingHierarchyList(ReportingHierarchySearchParameters request)
        {
            return await _adminRepository.GetReportingHierarchyList(request);
        }

        public async Task<ReportingHierarchyDetailsResponse?> GetReportingHierarchyDetailsById(long id)
        {
            return await _adminRepository.GetReportingHierarchyDetailsById(id);
        }
        public async Task<IEnumerable<ReportingHierarchyFailToImportValidationErrors>> ImportReportingHierarchyDetails(List<ReportingHierarchyImportSaveParameters> request)
        {
            return await _adminRepository.ImportReportingHierarchyDetails(request);
        }

        #endregion

        #region Employee
        public async Task<int> SaveEmployee(EmployeeSaveParameters request)
        {
            return await _adminRepository.SaveEmployee(request);
        }
        public async Task<IEnumerable<EmployeeDetailsResponse>> GetEmployeeList(EmployeeSearchParameters request)
        {
            return await _adminRepository.GetEmployeeList(request);
        }

        public async Task<EmployeeDetailsResponse?> GetEmployeeDetailsById(long id)
        {
            return await _adminRepository.GetEmployeeDetailsById(id);
        }
        public async Task<IEnumerable<EmployeeDetailsResponse>> GetEmployeeListByRoleId(long roleId)
        {
            return await _adminRepository.GetEmployeeListByRoleId(roleId);
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
    }

}
