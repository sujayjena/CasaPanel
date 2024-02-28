using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasaAPI.Models.VendorModel;
using static CasaAPI.Models.SubVendorModel;
using static CasaAPI.Models.CompanyTypeModel;
using static CasaAPI.Models.VendorGroupModel;
using static CasaAPI.Models.TDSNatureModel;
using static CasaAPI.Models.AllVendorModel;
using static CasaAPI.Models.GendorModel;
using CasaAPI.Models.Enums;

namespace CasaAPI.Repositories
{
    public class VendorRepository : BaseRepository, IVendorRepository
    {
        private IConfiguration _configuration;
        public VendorRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Vendor Type
        public async Task<int> SaveVendor(VendorSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VendorId", parameters.VendorId);
            queryParameters.Add("@VendorType", parameters.VendorType.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveVendorDetails", queryParameters);
        }
        public async Task<IEnumerable<VendorDetailsResponse>> GetVendorsList(VendorSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<VendorDetailsResponse>("GetVendorsList", queryParameters);
        }
        public async Task<VendorDetailsResponse?> GetVendorDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<VendorDetailsResponse>("GetVendorDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<VendorFailToImportValidationErrors>> ImportVendorsDetails(List<VendorImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlVendorData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<VendorFailToImportValidationErrors>("SaveImportVendorDetails", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetVendorForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetVendorMasterForSelectList", queryParameters);
        }
        #endregion

        #region Sub Vendor Type
        public async Task<int> SaveSubVendor(SubVendorSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@SubVendorId", parameters.SubVendorId);
            queryParameters.Add("@SubVendorType", parameters.SubVendorType.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveSubVendorDetails", queryParameters);
        }
        public async Task<IEnumerable<SubVendorDetailsResponse>> GetSubVendorsList(SubVendorSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<SubVendorDetailsResponse>("GetSubVendorsList", queryParameters);
        }
        public async Task<SubVendorDetailsResponse?> GetSubVendorDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<SubVendorDetailsResponse>("GetSubVendorDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<SubVendorFailToImportValidationErrors>> ImportSubVendorsDetails(List<SubVendorImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlSubVendorData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<SubVendorFailToImportValidationErrors>("SaveImportSubVendorDetails", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetSubVendorForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetSubVendorMasterForSelectList", queryParameters);
        }
        #endregion

        #region Company Type
        public async Task<int> SaveCompanyType(CompanyTypeSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@CompanyTypeId", parameters.CompanyTypeId);
            queryParameters.Add("@CompanyType", parameters.CompanyType.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveCompanyTypeDetails", queryParameters);
        }
        public async Task<IEnumerable<CompanyTypeDetailsResponse>> GetCompanyTypesList(CompanyTypeSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<CompanyTypeDetailsResponse>("GetCompanyTypesList", queryParameters);
        }
        public async Task<CompanyTypeDetailsResponse?> GetCompanyTypeDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<CompanyTypeDetailsResponse>("GetCompanyTypeDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<CompanyTypeFailToImportValidationErrors>> ImportCompanyTypesDetails(List<CompanyTypeImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlCompanyTypeData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<CompanyTypeFailToImportValidationErrors>("SaveImportCompanyTypeDetails", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetCompanyTypeForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetCompanyTypeMasterForSelectList", queryParameters);
        }
        #endregion

        #region Vendor Group
        public async Task<int> SaveVendorGroup(VendorGroupSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@VendorGroupId", parameters.VendorGroupId);
            queryParameters.Add("@VendorGroup", parameters.VendorGroup.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveVendorGroupDetails", queryParameters);
        }
        public async Task<IEnumerable<VendorGroupDetailsResponse>> GetVendorGroupsList(VendorGroupSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<VendorGroupDetailsResponse>("GetVendorGroupsList", queryParameters);
        }
        public async Task<VendorGroupDetailsResponse?> GetVendorGroupDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<VendorGroupDetailsResponse>("GetVendorGroupDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<VendorGroupFailToImportValidationErrors>> ImportVendorGroupsDetails(List<VendorGroupImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlVendorGroupData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<VendorGroupFailToImportValidationErrors>("SaveImportVendorGroupDetails", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetVendorGroupForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetVendorGroupMasterForSelectList", queryParameters);
        }
        #endregion

        #region TDS Nature
        public async Task<int> SaveTDSNature(TDSNatureSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@TDSNatureId", parameters.TDSNatureId);
            queryParameters.Add("@TDSNature", parameters.TDSNature.SanitizeValue().ToUpper());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveTDSNatureDetails", queryParameters);
        }
        public async Task<IEnumerable<TDSNatureDetailsResponse>> GetTDSNaturesList(TDSNatureSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<TDSNatureDetailsResponse>("GetTDSNaturesList", queryParameters);
        }
        public async Task<TDSNatureDetailsResponse?> GetTDSNatureDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<TDSNatureDetailsResponse>("GetTDSNatureDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<TDSNatureFailToImportValidationErrors>> ImportTDSNaturesDetails(List<TDSNatureImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlTDSNatureData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<TDSNatureFailToImportValidationErrors>("SaveImportTDSNatureDetails", queryParameters);
        }

        public async Task<IEnumerable<SelectListResponse>> GetTDSNatureForSelectList(CommonSelectListRequestModel parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@IsActive", parameters.IsActive);
            return await ListByStoredProcedure<SelectListResponse>("GetTDSNatureMasterForSelectList", queryParameters);
        }
        #endregion

        #region All Vendor
        public async Task<int> SaveAllVendor(AllVendorSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@VendorId", parameters.VendorId);
            queryParameters.Add("@CompanyTypeId", parameters.CompanyTypeId);
            queryParameters.Add("@SubVendorId", parameters.SubVendorId);
            queryParameters.Add("@VendorGroupId", parameters.VendorGroupId);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveAllVendorDetails", queryParameters);
        }
        public async Task<IEnumerable<AllVendorDetailsResponse>> GetAllVendorsList(AllVendorSearchParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<AllVendorDetailsResponse>("GetAllVendorsList", queryParameters);
        }
        public async Task<AllVendorDetailsResponse?> GetAllVendorDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<AllVendorDetailsResponse>("GetAllVendorDetailsById", queryParameters)).FirstOrDefault();
        }
        public async Task<IEnumerable<AllVendorFailToImportValidationErrors>> ImportAllVendorsDetails(List<AllVendorImportSaveParameters> parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            string xmlBrandData = ConvertListToXml(parameters);
            queryParameters.Add("@XmlAllVendorData", xmlBrandData);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await ListByStoredProcedure<AllVendorFailToImportValidationErrors>("SaveImportAllVendorDetails", queryParameters);
        }

        #endregion
        
    }
}
