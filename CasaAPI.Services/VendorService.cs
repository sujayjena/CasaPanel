using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CasaAPI.Models.CompanyTypeModel;
using static CasaAPI.Models.SubVendorModel;
using static CasaAPI.Models.VendorModel;
using static CasaAPI.Models.VendorGroupModel;
using static CasaAPI.Models.TDSNatureModel;
using static CasaAPI.Models.AllVendorModel;
using static CasaAPI.Models.GendorModel;

namespace CasaAPI.Services
{
    public class VendorService: IVendorService
    {
        private IVendorRepository _vendorRepository;
        private IFileManager _fileManager;
        public VendorService(IVendorRepository driverRepository, IFileManager fileManager)
        {
            _vendorRepository = driverRepository;
            _fileManager = fileManager;
        }

        #region Vendor        
        public async Task<int> SaveVendor(VendorSaveParameters request)
        {
            return await _vendorRepository.SaveVendor(request);
        }
        public async Task<IEnumerable<VendorDetailsResponse>> GetVendorsList(VendorSearchParameters request)
        {
            return await _vendorRepository.GetVendorsList(request);
        }
        public async Task<VendorDetailsResponse?> GetVendorDetailsById(long id)
        {
            return await _vendorRepository.GetVendorDetailsById(id);
        }

        public async Task<IEnumerable<VendorFailToImportValidationErrors>> ImportVendorsDetails(List<VendorImportSaveParameters> request)
        {
            return await _vendorRepository.ImportVendorsDetails(request);
        }
        public async Task<IEnumerable<SelectListResponse>> GetVendorForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _vendorRepository.GetVendorForSelectList(parameters);
        }

        #endregion

        #region Sub Vendor Type        
        public async Task<int> SaveSubVendor(SubVendorSaveParameters request)
        {
            return await _vendorRepository.SaveSubVendor(request);
        }
        public async Task<IEnumerable<SubVendorDetailsResponse>> GetSubVendorsList(SubVendorSearchParameters request)
        {
            return await _vendorRepository.GetSubVendorsList(request);
        }
        public async Task<SubVendorDetailsResponse?> GetSubVendorDetailsById(long id)
        {
            return await _vendorRepository.GetSubVendorDetailsById(id);
        }

        public async Task<IEnumerable<SubVendorFailToImportValidationErrors>> ImportSubVendorsDetails(List<SubVendorImportSaveParameters> request)
        {
            return await _vendorRepository.ImportSubVendorsDetails(request);
        }
        public async Task<IEnumerable<SelectListResponse>> GetSubVendorForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _vendorRepository.GetSubVendorForSelectList(parameters);
        }

        #endregion

        #region Company Type        
        public async Task<int> SaveCompanyType(CompanyTypeSaveParameters request)
        {
            return await _vendorRepository.SaveCompanyType(request);
        }
        public async Task<IEnumerable<CompanyTypeDetailsResponse>> GetCompanyTypesList(CompanyTypeSearchParameters request)
        {
            return await _vendorRepository.GetCompanyTypesList(request);
        }
        public async Task<CompanyTypeDetailsResponse?> GetCompanyTypeDetailsById(long id)
        {
            return await _vendorRepository.GetCompanyTypeDetailsById(id);
        }

        public async Task<IEnumerable<CompanyTypeFailToImportValidationErrors>> ImportCompanyTypesDetails(List<CompanyTypeImportSaveParameters> request)
        {
            return await _vendorRepository.ImportCompanyTypesDetails(request);
        }
        public async Task<IEnumerable<SelectListResponse>> GetCompanyTypeForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _vendorRepository.GetCompanyTypeForSelectList(parameters);
        }

        #endregion

        #region Vendor Group        
        public async Task<int> SaveVendorGroup(VendorGroupSaveParameters request)
        {
            return await _vendorRepository.SaveVendorGroup(request);
        }
        public async Task<IEnumerable<VendorGroupDetailsResponse>> GetVendorGroupsList(VendorGroupSearchParameters request)
        {
            return await _vendorRepository.GetVendorGroupsList(request);
        }
        public async Task<VendorGroupDetailsResponse?> GetVendorGroupDetailsById(long id)
        {
            return await _vendorRepository.GetVendorGroupDetailsById(id);
        }

        public async Task<IEnumerable<VendorGroupFailToImportValidationErrors>> ImportVendorGroupsDetails(List<VendorGroupImportSaveParameters> request)
        {
            return await _vendorRepository.ImportVendorGroupsDetails(request);
        }
        public async Task<IEnumerable<SelectListResponse>> GetVendorGroupForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _vendorRepository.GetVendorGroupForSelectList(parameters);
        }

        #endregion

        #region TDS Nature        
        public async Task<int> SaveTDSNature(TDSNatureSaveParameters request)
        {
            return await _vendorRepository.SaveTDSNature(request);
        }
        public async Task<IEnumerable<TDSNatureDetailsResponse>> GetTDSNaturesList(TDSNatureSearchParameters request)
        {
            return await _vendorRepository.GetTDSNaturesList(request);
        }
        public async Task<TDSNatureDetailsResponse?> GetTDSNatureDetailsById(long id)
        {
            return await _vendorRepository.GetTDSNatureDetailsById(id);
        }

        public async Task<IEnumerable<TDSNatureFailToImportValidationErrors>> ImportTDSNaturesDetails(List<TDSNatureImportSaveParameters> request)
        {
            return await _vendorRepository.ImportTDSNaturesDetails(request);
        }
        public async Task<IEnumerable<SelectListResponse>> GetTDSNatureForSelectList(CommonSelectListRequestModel parameters)
        {
            return await _vendorRepository.GetTDSNatureForSelectList(parameters);
        }

        #endregion

        #region All Vendor        
        public async Task<int> SaveAllVendor(AllVendorSaveParameters request)
        {
            return await _vendorRepository.SaveAllVendor(request);
        }
        public async Task<IEnumerable<AllVendorDetailsResponse>> GetAllVendorsList(AllVendorSearchParameters request)
        {
            return await _vendorRepository.GetAllVendorsList(request);
        }
        public async Task<AllVendorDetailsResponse?> GetAllVendorDetailsById(long id)
        {
            return await _vendorRepository.GetAllVendorDetailsById(id);
        }

        public async Task<IEnumerable<AllVendorFailToImportValidationErrors>> ImportAllVendorsDetails(List<AllVendorImportSaveParameters> request)
        {
            return await _vendorRepository.ImportAllVendorsDetails(request);
        }

        #endregion

        
    }
}
