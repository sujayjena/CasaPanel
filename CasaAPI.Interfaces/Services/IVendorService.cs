using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasaAPI.Models.CompanyTypeModel;
using static CasaAPI.Models.VendorModel;
using static CasaAPI.Models.SubVendorModel;
using static CasaAPI.Models.VendorGroupModel;
using static CasaAPI.Models.TDSNatureModel;
using static CasaAPI.Models.AllVendorModel;
using static CasaAPI.Models.GendorModel;

namespace CasaAPI.Interfaces.Services
{
    public interface IVendorService
    {
        #region Vendor Type
        Task<int> SaveVendor(VendorSaveParameters parameters);
        Task<IEnumerable<VendorDetailsResponse>> GetVendorsList(VendorSearchParameters parameters);
        Task<VendorDetailsResponse?> GetVendorDetailsById(long id);
        Task<IEnumerable<VendorFailToImportValidationErrors>> ImportVendorsDetails(List<VendorImportSaveParameters> parameters);
        Task<IEnumerable<SelectListResponse>> GetVendorForSelectList(CommonSelectListRequestModel parameters);
        #endregion

        #region Sub Vendor Type
        Task<int> SaveSubVendor(SubVendorSaveParameters parameters);
        Task<IEnumerable<SubVendorDetailsResponse>> GetSubVendorsList(SubVendorSearchParameters parameters);
        Task<SubVendorDetailsResponse?> GetSubVendorDetailsById(long id);
        Task<IEnumerable<SubVendorFailToImportValidationErrors>> ImportSubVendorsDetails(List<SubVendorImportSaveParameters> parameters);
        Task<IEnumerable<SelectListResponse>> GetSubVendorForSelectList(CommonSelectListRequestModel parameters);
        #endregion

        #region Company Type
        Task<int> SaveCompanyType(CompanyTypeSaveParameters parameters);
        Task<IEnumerable<CompanyTypeDetailsResponse>> GetCompanyTypesList(CompanyTypeSearchParameters parameters);
        Task<CompanyTypeDetailsResponse?> GetCompanyTypeDetailsById(long id);
        Task<IEnumerable<CompanyTypeFailToImportValidationErrors>> ImportCompanyTypesDetails(List<CompanyTypeImportSaveParameters> parameters);
        Task<IEnumerable<SelectListResponse>> GetCompanyTypeForSelectList(CommonSelectListRequestModel parameters);
        #endregion

        #region Vendor Group
        Task<int> SaveVendorGroup(VendorGroupSaveParameters parameters);
        Task<IEnumerable<VendorGroupDetailsResponse>> GetVendorGroupsList(VendorGroupSearchParameters parameters);
        Task<VendorGroupDetailsResponse?> GetVendorGroupDetailsById(long id);
        Task<IEnumerable<VendorGroupFailToImportValidationErrors>> ImportVendorGroupsDetails(List<VendorGroupImportSaveParameters> parameters);
        Task<IEnumerable<SelectListResponse>> GetVendorGroupForSelectList(CommonSelectListRequestModel parameters);
        #endregion

        #region TDS Natur
        Task<int> SaveTDSNature(TDSNatureSaveParameters parameters);
        Task<IEnumerable<TDSNatureDetailsResponse>> GetTDSNaturesList(TDSNatureSearchParameters parameters);
        Task<TDSNatureDetailsResponse?> GetTDSNatureDetailsById(long id);
        Task<IEnumerable<TDSNatureFailToImportValidationErrors>> ImportTDSNaturesDetails(List<TDSNatureImportSaveParameters> parameters);
        Task<IEnumerable<SelectListResponse>> GetTDSNatureForSelectList(CommonSelectListRequestModel parameters);
        #endregion

        #region All Vendor
        Task<int> SaveAllVendor(AllVendorSaveParameters parameters);
        Task<IEnumerable<AllVendorDetailsResponse>> GetAllVendorsList(AllVendorSearchParameters parameters);
        Task<AllVendorDetailsResponse?> GetAllVendorDetailsById(long id);
        Task<IEnumerable<AllVendorFailToImportValidationErrors>> ImportAllVendorsDetails(List<AllVendorImportSaveParameters> parameters);
        #endregion

       
    }
}
